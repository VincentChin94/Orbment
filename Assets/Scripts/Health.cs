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
    public float m_maxHealth = 100;
    public float m_currHealth = 100;
    public float m_experienceValue = 10;
    public Texture m_healthBarTexture;
    public Texture m_emptyBarTexture;
    public float m_recentDamageTaken = 0.0f;

    [Header("Elemental effects")]
    public float m_fireDmgTknPerSec = 5.0f;
    public float m_fireDuration = 5.0f;
    private float m_fireTimer = 0.0f;
    public ParticleSystem m_fireEffect;

    public float m_iceMoveSlowAmount = 0.5f;
    public float m_slowDuration = 5.0f;
    private float m_slowTimer = 0.0f;
    public ParticleSystem m_iceEffect;


    public float m_stunDuration = 5.0f;
    private bool m_isStuned = false;
    private float m_stunTimeStart = 0.0f;
    public ParticleSystem m_stunEffect;


    //Agent
    private NavMeshAgent m_agent;
    //original move speed;
    private float m_originalMoveSpeed = 0.0f;

    //old health;
    private float m_oldHealth = 0.0f;


    private ExpManager m_expManager;
    private DamageNumberManager m_damageNumbersManager;
    //public Transform m_RecentAttacker;




    //status effects
    [Header("Status Effects")]
    public bool isOnFire = false;
    public bool isFrozen = false;
    void Start()
    {
        m_agent = this.GetComponent<NavMeshAgent>();

        if (m_agent != null)
        {
            m_originalMoveSpeed = m_agent.speed;
        }
        m_expManager = GameObject.FindObjectOfType<ExpManager>();
        m_damageNumbersManager = GameObject.FindObjectOfType<DamageNumberManager>();

        m_oldHealth = m_currHealth;
    }

    // Update is called once per frame
    void Update()
    {


        if (m_oldHealth != m_currHealth)
        {
            m_damageNumbersManager.CreateDamageNumber(Mathf.CeilToInt(m_oldHealth - m_currHealth).ToString(), this.transform);
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
                GameObject.Destroy(this.gameObject, Time.deltaTime);
            }
            else
            {
                m_currHealth = 1.0f;
            }

            //handle death state
        }


        if (isOnFire)
        {
            TakeFireDamage();
        }

        if (isFrozen)
        {
            IceSlow();
        }

        if (m_isStuned)
        {
            stunEffect();
        }

        m_oldHealth = m_currHealth;
    }



    void IceSlow()
    {
        if (m_iceEffect != null)
        {
            m_iceEffect.Play();
        }
        if (m_agent != null)
        {
            m_agent.speed = m_iceMoveSlowAmount * m_originalMoveSpeed;
        }

        m_slowTimer += Time.deltaTime;

        if (m_slowTimer >= m_slowDuration)
        {
            if (m_iceEffect != null)
            {
                m_iceEffect.Stop();
            }

            if (m_agent != null)
            {
                m_agent.speed = m_originalMoveSpeed;
            }
            m_slowTimer = 0.0f;
            isFrozen = false;
        }

    }


    void TakeFireDamage()
    {
        if (m_fireEffect != null)
        {
            m_fireEffect.Play();
        }



        m_fireTimer += Time.deltaTime;

        if (m_fireTimer % 1.0f < 0.01f)
        {
            m_currHealth -= m_fireDmgTknPerSec;
            m_damageNumbersManager.CreateDamageNumber((m_fireDmgTknPerSec).ToString(), this.transform);
        }


        if (m_fireTimer >= m_fireDuration)
        {
            if (m_fireEffect != null)
            {
                m_fireEffect.Stop();
            }
            m_fireTimer = 0.0f;
            isOnFire = false;
        }

    }

    private void stunEffect()
    {
        //stun finished
        if (Time.time - m_stunTimeStart > m_stunDuration)
        {
            m_isStuned = false;
            if (m_agent != null)
            {
                m_agent.speed = m_originalMoveSpeed;
            }
            if (m_stunEffect != null)
            {
                m_stunEffect.Stop();
            }
            //allow the enemy to attack again
            EnemyAttack ea = GetComponent<EnemyAttack>();
            if (ea != null)
            {
                ea.m_CanAttack = true;
            }
        }
        else
        {//stun in effect
            if (m_agent != null)
            {
                m_agent.speed = 0.0f;
            }

        }
    }



    void OnGUI()
    {


        if (m_currHealth != m_maxHealth)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);

            GUI.DrawTexture(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 60, m_healthBarWidth, 10), m_emptyBarTexture);
            GUI.DrawTexture(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 60, m_healthBarWidth * (m_currHealth / m_maxHealth), 10), m_healthBarTexture);

        }

        if (isOnFire)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(screenPoint.x - 0.5f * m_healthBarWidth, Screen.height - screenPoint.y - 80, m_healthBarWidth, 20), "ON FIRE!");
        }
    }

    internal void stunEnemy()
    {
        m_stunTimeStart = Time.time;
        //only run this if the enemy is not already stunned
        if (!m_isStuned)
        {
            m_isStuned = true;
            if (m_stunEffect != null)
            {
                m_stunEffect.Play();
            }
            //stop the enemy from attacking
            EnemyAttack ea = GetComponent<EnemyAttack>();
            if (ea != null)
            {
                ea.m_CanAttack = false;
            }
        }
    }
}
