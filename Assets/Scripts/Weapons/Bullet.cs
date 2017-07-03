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
    public float m_projectileSpeed = 50;
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

    protected ExplosionManager m_explosionManager;
    protected Health m_enemyHealth = null;

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
    protected virtual void Start()
    {
        m_explosionManager = GameObject.FindObjectOfType<ExplosionManager>();
        m_trail = this.GetComponent<TrailRenderer>();
 

    }

    protected void OnEnable()
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
    protected void FixedUpdate()
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
    protected void Disable()
    {

        m_timer = 0.0f;
        m_enemyHealth = null;
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

    protected void CameraCheck()
    {
        //disable when object leaves camera view
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.x < -m_cullOffset || screenPosition.x > Screen.width + m_cullOffset || screenPosition.y < -m_cullOffset || screenPosition.y > Screen.height + m_cullOffset)
        {
            Disable();
        }

    }




    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (m_id == "")
        {
            return;
        }

        if (!collision.collider.CompareTag(m_id))
        {
             m_enemyHealth = collision.collider.GetComponent<Health>();
            //do base damage
            if (m_enemyHealth != null)
            {
                m_enemyHealth.m_currHealth -= m_damage;
                m_enemyHealth.m_recentDamageTaken = m_damage;


            }
        }
           




    }
}
