using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Health : MonoBehaviour
{
    public bool m_godMode = false;
    public bool isPlayer = false;
    public int m_healthBarWidth = 100;
    public int m_maxHealth = 100;
    public int m_currHealth = 100;
    public float m_experienceValue = 10;
    public Texture m_healthBarTexture;
    public Texture m_emptyBarTexture;
    public int m_recentDamageTaken = 0;

    //Agent
    private NavMeshAgent m_agent;
    //original move speed;
    [HideInInspector]
    public float m_originalMoveSpeed = 0.0f;

    //old health;
    private float m_oldHealth = 0.0f;


    private ExpManager m_expManager;
    private DamageNumberManager m_damageNumbersManager;
    private StatusEffectManager m_statusEffectManager;


    private IsoCam m_camera;

    //public Transform m_RecentAttacker;

    [HideInInspector]
    public bool m_setOnFire = false, m_causeStun = false, m_causeSlow = false;


    //status effects
    [Header("Status Effects")]
    public bool m_onFire = false;
    public bool m_isSlowed = false;
    public bool m_isStunned = false;

    void Start()
    {
        m_agent = this.GetComponent<NavMeshAgent>();

        if (m_agent != null)
        {
            m_originalMoveSpeed = m_agent.speed;
        }
        m_expManager = GameObject.FindObjectOfType<ExpManager>();
        m_damageNumbersManager = GameObject.FindObjectOfType<DamageNumberManager>();
        m_statusEffectManager = GameObject.FindObjectOfType<StatusEffectManager>();
       

        m_oldHealth = m_currHealth;

        if(isPlayer)
        {
            m_camera = GameObject.FindObjectOfType<IsoCam>();

        }
    }

    // Update is called once per frame
    void Update()
    {


        if (m_oldHealth != m_currHealth)
        {
            m_damageNumbersManager.CreateDamageNumber(Mathf.CeilToInt(m_oldHealth - m_currHealth).ToString(), this.transform);
            if(m_camera != null && m_currHealth < m_oldHealth)
            {
                //shake cam if player hurt
                m_camera.FlashRed(0.5f);
                m_camera.Shake(10.0f, 0.1f);

            }
        }

        //if dead

        if (m_currHealth <= 0)
        {

            if (!isPlayer)
            {
                m_expManager.m_playerExperience += m_experienceValue;
            }

            if (!m_godMode)
            {
                this.gameObject.SetActive(false);
                //GameObject.Destroy(this.gameObject, Time.deltaTime);
            }
            else
            {
                m_currHealth = 1;
            }

            //handle death state
        }

        ///STATUS EFFECTS
        //if set on fire and previously not on fire. to avoid effect stacking
        if (m_setOnFire && !m_onFire)
        {
            m_statusEffectManager.RequestEffect(this.transform, StatusEffect.Status.OnFire);
            m_setOnFire = false;

        }

        if (m_causeSlow && !m_isSlowed)
        {
            m_statusEffectManager.RequestEffect(this.transform, StatusEffect.Status.Slowed);
            m_causeSlow = false;
        }

        if (m_causeStun && !m_isStunned)
        {
            m_statusEffectManager.RequestEffect(this.transform, StatusEffect.Status.Stunned);
            m_causeStun = false;
        }

        m_oldHealth = m_currHealth;
    }




    void OnGUI()
    {


        if (m_currHealth != m_maxHealth)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);

            GUI.DrawTexture(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 60, m_healthBarWidth, 10), m_emptyBarTexture);
            GUI.DrawTexture(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 60, m_healthBarWidth * ((float)m_currHealth / (float)m_maxHealth), 10), m_healthBarTexture);

        }

        if (m_onFire)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 80, m_healthBarWidth, 20), "ON FIRE!");
        }

        if (m_isStunned)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 80, m_healthBarWidth, 20), "STUNNED!");
        }
    }


    
}
