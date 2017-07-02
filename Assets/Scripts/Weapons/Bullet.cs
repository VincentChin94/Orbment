using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("ShooterID")]
    public string m_id;
    [Header("Direction Vector")]
    public Vector3 m_direction;
    [Header("Damage")]
    public float m_damage = 5;
    [Header("Projecile Speed")]
    public float m_projectileSpeed = 10;
    [HideInInspector]
    public BaseWeapon m_weaponFiredFrom;
    [Header("Culling Offset")]
    public int m_cullOffset = 500;

    [Header("Projectile Lifetime")]
    public float m_lifetime = 2.0f;
    private float m_timer = 0.0f;
    private MeshRenderer m_renderer;
    private Collider m_collider;
    private Light m_light;
    private TrailRenderer m_trail;

    private ExplosionManager m_explosionManager;

    /// <summary>
    /// Area of Effect
    /// </summary>
    /// 
    [Header("Area Of Effect")]
    public bool m_fireSplash = false;
    public bool m_iceSplit = false;

    [Header("STUN")]
    [Range(0, 100)]
    public float m_StunChance = 25.0f;
    public bool m_hasStunPerk = false;

    public enum ProjectileType
    {
        Normal,
        FireBall,
        IceShard,
        Lightning,
        Bouncing,
    }

    public ProjectileType m_type;






    // Use this for initialization
    void Start()
    {
        m_explosionManager = GameObject.FindObjectOfType<ExplosionManager>();
        m_trail = this.GetComponent<TrailRenderer>();
 

    }

    private void OnEnable()
    {
        m_light = this.GetComponent<Light>();
        m_collider = this.GetComponent<Collider>();
        m_renderer = this.GetComponent<MeshRenderer>();
        m_renderer.enabled = true;
        m_collider.enabled = true;
        if (m_light != null)
        {
            m_light.enabled = true;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        this.transform.position += m_direction * m_projectileSpeed * Time.deltaTime;
        CameraCheck();
        m_timer += Time.deltaTime;

        //cull timer
        if (m_timer > m_lifetime)
        {
            Disable();
        }
    }

    //is is camera view?
    private void Disable()
    {

        m_timer = 0.0f;
        this.gameObject.SetActive(false);
        if (m_trail != null)
        {
            m_trail.Clear();
        }


        if (m_weaponFiredFrom != null)
        {
            this.transform.position = m_weaponFiredFrom.transform.position;
            this.transform.parent = m_weaponFiredFrom.transform;
        }
    }

    void CameraCheck()
    {
        //disable when object leaves camera view
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.x < -m_cullOffset || screenPosition.x > Screen.width + m_cullOffset || screenPosition.y < -m_cullOffset || screenPosition.y > Screen.height + m_cullOffset)
        {
            Disable();
        }

    }




    private void OnCollisionEnter(Collision collision)
    {

        if (m_id == "")
        {
            return;
        }

        if (!collision.collider.CompareTag(m_id))
        {
            Health enemyHealth = collision.collider.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.m_currHealth -= m_damage;
                enemyHealth.m_recentDamageTaken = m_damage;


            }
            switch (m_type)
            {
                case ProjectileType.Normal:
                    {
                        //nothing
                        Disable();
                        break;
                    }
                case ProjectileType.FireBall:
                    {
                        //set on fire
                        if (enemyHealth != null)
                        {

                            //ran
                            if (m_hasStunPerk && Random.Range(0.0f, 100.0f) <= m_StunChance)
                            {
                                enemyHealth.m_causeStun = true;
                            }

                            enemyHealth.m_setOnFire = true;
                        }


                        //if AOE toggled on 
                        if (m_fireSplash)
                        {
                            m_explosionManager.RequestExplosion(collision.contacts[0].point, ExplosionManager.ExplosionType.Fire);

                        }

                        Disable();


                        break;
                    }
                case ProjectileType.Lightning:
                    {
                        //change to create prefab?!
                        GameObject go = new GameObject("Lightning script");
                        LightningHandler lh = go.AddComponent<LightningHandler>();
                        lh.m_BaseDamage = this.m_damage;
                        if (enemyHealth != null)
                        {
                            lh.startAttack(enemyHealth.gameObject);
                        }

                        Destroy(go, 0.2f);

                        Disable();
                        break;
                    }

                case ProjectileType.Bouncing:
                    {

                        Collider[] nearby = Physics.OverlapSphere(this.transform.position, 10.0f);
                        float closest = Mathf.Infinity;
                        Collider closestCollider = null;
                        foreach (Collider col in nearby)
                        {
                            if (!col.CompareTag(m_id) && col != collision.collider)
                            {
                                float distance = Vector3.Distance(col.transform.position, this.transform.position);
                                if (distance < closest)
                                {
                                    closestCollider = col;
                                    closest = distance;
                                }

                            }
                        }

                        if (closestCollider != null)
                        {
                            Vector3 newDIr = closestCollider.transform.position - this.transform.position;
                            this.m_direction = newDIr.normalized;
                            this.m_damage -= this.m_damage * 0.2f;
                        }

                        if (m_damage <= 1.0f || closestCollider == null)
                        {
                            Disable();

                        }


                        break;
                    }

                case ProjectileType.IceShard:
                    {
                        if (enemyHealth != null)
                        {
                            enemyHealth.m_causeSlow = true;
                        }

                        if (m_iceSplit)
                        {
                            m_explosionManager.RequestExplosion(this.transform.position, ExplosionManager.ExplosionType.Ice);

                        }


                        Disable();
                        break;
                    }
            }





        }



    }
}
