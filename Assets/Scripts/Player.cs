using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
public class Player : MonoBehaviour
{

    public float m_playerMoveSpeed = 10;
    public float m_playerRotSpeed = 10;


    public float m_playerFiringInterval = 0.1f;
    public float m_currentDamagePerProjectile = 10.0f;
    public Transform m_shootPoint;
    public float m_manaRegenRate = 10.0f;
    public float m_shootManaCost = 10.0f;
    public float m_dashSpeed = 10.0f;
    public float m_dashTime = 0.2f;

    

    public List<BaseWeapon> m_weapons;
    public BaseWeapon m_currWeapon;


    //projectiles
    public GameObject m_currentProjectile;
    //public GameObject[] m_projectiles;

    //orb count
    public int m_orbsCollected = 0;


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




    [Header("AOE")]
    public bool m_hasFireSplash = false;
    public bool m_hasIceSplit = false;

    //RamboMode
    [Header("Rambo Mode")]
    public bool m_hasRamboPerk = false;
    public float m_ramboModeDmgMult = 2.0f;
    public float m_ramboModePercentageThreshold = 10.0f;
    private float m_originalDamage = 0.0f;
    private bool m_ramboActive = false;
    private Component m_halo;


    //Elemental Rings
    [Header("Elemental Rings")]
    public bool m_hasRingOfFirePerk = false;
    public float m_ringOfFirePercentThreshold = 25.0f;
    public GameObject m_ringOfFireParticles;
    private bool m_ringOfFireActive = false;

    public bool m_hasLightningFieldPerk = false;
    public float m_lightningFieldPercentThreshold = 25.0f;
    public GameObject m_lightningField;
    private bool m_lightningFieldActive = false;

    [Header("Stun")]
    public bool m_hasStunPerk = false;




    private IsoCam m_camera;

    void Start()
    {
        m_camera = GameObject.FindObjectOfType<IsoCam>();

        m_weapons = new List<BaseWeapon>();
        m_charCont = this.GetComponent<CharacterController>();
        m_manaPool = this.GetComponent<Mana>();
        m_health = this.GetComponent<Health>();

        m_halo = this.GetComponent("Halo");

        m_shootPlane = LayerMask.GetMask("ShootPlane");
        //Cursor.visible = false;

        m_dashTrail = this.GetComponent<TrailRenderer>();

        if(m_dashTrail != null)
        {
            m_dashTrail.enabled = false;
        }
    }


    void Update()
    {
        //stick to ground helper
        //if(this.transform.position.y != m_startingHeight)
        //{
        //    Vector3 heightDifference =  Vector3.up * (m_startingHeight - this.transform.position.y);
        //    this.transform.position += heightDifference;
        //}

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
                        m_currWeapon.m_hasFireSplash = this.m_hasFireSplash;
                        m_currWeapon.m_hasIceSplit = this.m_hasIceSplit;
                        m_currWeapon.m_hasStunPerk = this.m_hasStunPerk;

                        m_currWeapon.Fire(this.transform.forward, m_currentDamagePerProjectile);

                        if(m_camera != null)
                        {
                            m_camera.Shake(0.1f, m_playerFiringInterval);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_dashTrail != null)
            {
                m_dashTrail.enabled = true;
               
            }
            
            m_dashDirection = m_movement.normalized;
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
            m_charCont.Move(m_movement * m_playerMoveSpeed * Time.deltaTime);
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
        if (m_hasRamboPerk && HealthBelowPercentCheck(m_ramboModePercentageThreshold) && !m_ramboActive)
        {
            m_originalDamage = m_currentDamagePerProjectile;
            m_currentDamagePerProjectile *= m_ramboModeDmgMult;
            m_halo.GetType().GetProperty("enabled").SetValue(m_halo, true, null);

            m_ramboActive = true;

        }
        
        if(m_ramboActive && HealthAbovePercentCheck(m_ramboModePercentageThreshold))
        {
            m_currentDamagePerProjectile = m_originalDamage;
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
        if (m_hasRingOfFirePerk && HealthBelowPercentCheck(m_ringOfFirePercentThreshold) && !m_ringOfFireActive)
        {

            
            m_ringOfFireActive = true;

        }
        
        if(m_ringOfFireActive && HealthAbovePercentCheck(m_ringOfFirePercentThreshold))
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
        if (m_hasLightningFieldPerk && HealthBelowPercentCheck(m_lightningFieldPercentThreshold) && !m_lightningFieldActive)
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

        float ratio = m_dashTimer / m_dashTime;

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





}
