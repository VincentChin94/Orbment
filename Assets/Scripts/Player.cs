using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
public class Player : MonoBehaviour
{
    [Header("Skill Points")]
    public int m_currDamagePoints = 0;
    public int m_currHealthPoints = 0;
    public int m_currSpeedPoints = 0;

    public int m_HealthIncrement = 10;
    public int m_DamageIncrement = 1;
    public int m_SpeedIncrement = 1;

    public int m_currDamageMult = 1;
    public int m_currSpeedMult = 1;



    public int m_baseHealth = 100;
    public int m_baseDamage = 10;
    public int m_baseSpeed = 10;

    private int m_currDamage;
    private int m_currSpeed;

    public float m_playerFiringInterval = 0.1f;

    public Transform m_shootPoint;
    public float m_manaRegenRate = 10.0f;
    public float m_shootManaCost = 10.0f;
    public float m_dashSpeed = 10.0f;
    public float m_dashTime = 0.2f;
    public float m_dashManaCost = 50.0f;



    public List<BaseWeapon> m_weapons;
    public BaseWeapon m_currWeapon;

    //critical hits and damage deviation
    public int m_critPercentChance = 10;
    public float m_critDmgMult = 2.0f;
    public int m_damageDeviation = 3;
    public bool m_hasCrit = false;





    //projectiles
    public GameObject m_currentProjectile;


    //orb count
    public int m_orbsCollected = 0;
    public GameObject m_spentOrbPrefab;
    public int m_poolAmountSpentOrbs = 15;
    private List<GameObject> m_spentOrbs = new List<GameObject>();

    private CharacterController m_charCont;
    private Vector3 m_movement;
    private float mouseX;
    private float mouseY;
    private Vector3 worldpos;
    private float cameraDif;

    private float m_playerFireTimer = 0.0f;
    private int m_shootPlane;

    private TrailRenderer m_dashTrail;
    private Mana m_manaPool;
    private Health m_health;

    //dash
    private bool m_dashing = false;
    private float m_dashTimer = 0.0f;
    private Vector3 m_dashDirection;

    private ExplosionManager m_explosionManager;

    //Perks
    /// <summary>
    /// /////////////////////////////////////////////////////////////
    /// </summary>
    /// 

    public List<PerkID> m_perks = new List<PerkID>();


    //RamboMode
    [Header("Rambo Mode")]
    public int m_ramboModeDmgMult = 2;
    public float m_ramboModePercentageThreshold = 10.0f;
    private bool m_ramboActive = false;
    private Component m_halo;


    //Elemental Rings
    [Header("Elemental Rings")]

    public float m_ringOfFirePercentThreshold = 25.0f;
    public GameObject m_ringOfFireParticles;
    private bool m_ringOfFireActive = false;

    public float m_lightningFieldPercentThreshold = 25.0f;
    public GameObject m_lightningField;
    private bool m_lightningFieldActive = false;

    /// <summary>
    /// ////////////////////////////////////////////////////////
    /// </summary>

    private IsoCam m_camera;

    void Start()
    {
        PoolSpentOrbs();

        m_camera = GameObject.FindObjectOfType<IsoCam>();

        m_weapons = new List<BaseWeapon>();
        m_charCont = this.GetComponent<CharacterController>();
        m_manaPool = this.GetComponent<Mana>();
        m_health = this.GetComponent<Health>();

        m_halo = this.GetComponent("Halo");

        m_shootPlane = LayerMask.GetMask("ShootPlane");
        //Cursor.visible = false;

        m_dashTrail = this.GetComponent<TrailRenderer>();

        if (m_dashTrail != null)
        {
            m_dashTrail.enabled = false;
        }

        m_explosionManager = GameObject.FindObjectOfType<ExplosionManager>();
    }




    void Update()
    {
        //stick to ground helper
        //if(this.transform.position.y != m_startingHeight)
        //{
        //    Vector3 heightDifference =  Vector3.up * (m_startingHeight - this.transform.position.y);
        //    this.transform.position += heightDifference;
        //}

        //update damage and speed

        m_currDamage = m_baseDamage + (m_currDamagePoints * m_DamageIncrement) + Random.Range(-m_damageDeviation, m_damageDeviation);
        m_currSpeed = m_baseSpeed + (m_currSpeedPoints * m_SpeedIncrement);

        
        

        //Check for RamboMode
        RamboModeCheck();
        ElementalRingCheck();

        //mouse hold fire
        if (Input.GetMouseButton(0))
        {
            if (m_playerFireTimer >= m_playerFiringInterval)
            {
                m_playerFireTimer = 0.0f;
            }

            if (m_playerFireTimer == 0.0f)
            {
                if (m_manaPool.m_currentMana - m_shootManaCost >= 0.0f)
                {
                    if (m_currWeapon != null)
                    {

                        m_currWeapon.m_playerRef = this;
                        m_hasCrit = false;
                        if (Random.Range(0, 100) <= m_critPercentChance)
                        {
                            //crit
                            m_currDamage = Mathf.CeilToInt((float)m_currDamage * m_critDmgMult);
                            m_hasCrit = true;
                        }
                        m_currWeapon.Fire(this.transform.forward, m_currDamage * m_currDamageMult, m_hasCrit);

                        if (m_camera != null)
                        {
                            m_camera.Shake(5.0f, m_playerFiringInterval);
                        }
                    }
                    m_manaPool.m_currentMana -= m_shootManaCost;
                }

            }

            m_playerFireTimer += Time.deltaTime;
        }

        //regen mana when mouse up   
        if (!Input.GetMouseButton(0))
        {
            RegenMana();
        }



        //dash
        if (Input.GetKeyDown(KeyCode.Space) && m_manaPool.m_currentMana >= m_dashManaCost && m_movement != Vector3.zero)
        {
            if (m_dashTrail != null)
            {
                m_dashTrail.enabled = true;

            }

            m_dashDirection = m_movement.normalized;
            m_manaPool.m_currentMana -= m_dashManaCost;
            m_dashing = true;

        }

        //get movement input
        m_movement = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");

    }


    void FixedUpdate()
    {



        //mouse aiming
        RaycastHit hit;
        float rayLength = 1000;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength, m_shootPlane))
        {

            Vector3 hitPoint = hit.point;
            hitPoint.y = 0;
            Vector3 playerToMouse = hitPoint - this.transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            this.transform.rotation = newRotation;
            Debug.DrawRay(this.transform.position, this.transform.forward * 10, Color.cyan);

            Debug.DrawLine(this.transform.position, hit.point, Color.red);
        }

        //dashing
        if (m_dashing)
        {
            Dash(m_dashDirection);

        }
        else
        {
            //movement
            m_charCont.Move(m_movement * m_currSpeed * m_currSpeedMult * Time.deltaTime);
        }

    }

    bool HealthBelowPercentCheck(float m_threshold)
    {
        if (m_health == null)
        {
            return false;
        }

        if (m_health.m_currHealth <= m_health.m_maxHealth * (m_threshold / 100.0f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool HealthAbovePercentCheck(float m_threshold)
    {
        if (m_health == null)
        {
            return false;
        }

        if (m_health.m_currHealth > m_health.m_maxHealth * (m_threshold / 100.0f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RamboModeCheck()
    {
        //if rambo perk is true and health is below threshold
        if (m_perks.Contains(PerkID.RamboMode) && HealthBelowPercentCheck(m_ramboModePercentageThreshold) && !m_ramboActive)
        {
            m_currDamageMult = m_ramboModeDmgMult;
            m_halo.GetType().GetProperty("enabled").SetValue(m_halo, true, null);

            m_ramboActive = true;

        }

        if (m_ramboActive && HealthAbovePercentCheck(m_ramboModePercentageThreshold))
        {
            m_currDamageMult = 1;
            m_halo.GetType().GetProperty("enabled").SetValue(m_halo, false, null);
            m_ramboActive = false;
        }

    }

    void ElementalRingCheck()
    {
        //////////////////////////////////////Ring of FIRE
        if (m_ringOfFireParticles == null)
        {
            return;
        }
        //if health is below threshold turn on ring of fire
        if (m_perks.Contains(PerkID.RingOfFire) && HealthBelowPercentCheck(m_ringOfFirePercentThreshold) && !m_ringOfFireActive)
        {


            m_ringOfFireActive = true;

        }

        if (m_ringOfFireActive && HealthAbovePercentCheck(m_ringOfFirePercentThreshold))
        {
            m_ringOfFireActive = false;
        }

        m_ringOfFireParticles.SetActive(m_ringOfFireActive);


        //////////////////////////////////////Lightning Field
        if (m_lightningField == null)
        {
            return;
        }
        //if health is below threshold turn on ring of fire
        if (m_perks.Contains(PerkID.LightningField) && HealthBelowPercentCheck(m_lightningFieldPercentThreshold) && !m_lightningFieldActive)
        {


            m_lightningFieldActive = true;

        }

        if (m_lightningFieldActive && HealthAbovePercentCheck(m_lightningFieldPercentThreshold))
        {
            m_lightningFieldActive = false;
        }

        m_lightningField.SetActive(m_lightningFieldActive);

    }



    void Dash(Vector3 dir)
    {

        m_charCont.Move(dir * m_dashSpeed);

        m_dashTimer += Time.deltaTime;

        if (m_explosionManager != null)
        {
            m_explosionManager.RequestExplosion(this.transform.position, this.transform.forward, Explosion.ExplosionType.AfterImage, 0.0f);
        }

        //float ratio = m_dashTimer / m_dashTime;

        if (m_dashTimer >= m_dashTime)
        {
            m_dashTimer = 0.0f;
            m_dashing = false;

            if (m_dashTrail != null)
            {
                m_dashTrail.Clear();
                m_dashTrail.enabled = false;
            }
        }

    }

    void RegenMana()
    {
        if (m_manaPool.m_currentMana >= m_manaPool.m_maxMana)
        {
            m_manaPool.m_currentMana = m_manaPool.m_maxMana;
        }

        if (m_manaPool.m_currentMana < m_manaPool.m_maxMana)
        {
            m_manaPool.m_currentMana += m_manaRegenRate * Time.deltaTime;
        }

    }



    void PoolSpentOrbs()
    {
        for (int i = 0; i < m_poolAmountSpentOrbs; ++i)
        {
            GameObject obj = GameObject.Instantiate(m_spentOrbPrefab);
            obj.SetActive(false);
            m_spentOrbs.Add(obj);

        }
    }

    public void EmitSpentOrb()
    {
        for (int i = 0; i < m_spentOrbs.Count; ++i)
        {
            if (!m_spentOrbs[i].activeInHierarchy)
            {
                m_spentOrbs[i].transform.position = this.transform.position + Random.onUnitSphere;

                m_spentOrbs[i].SetActive(true);
                return;
            }
        }
    }



}
