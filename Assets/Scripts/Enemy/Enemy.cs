using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public enum EnemyType
    {
        Hunter,
        Defender,
        Protector
    }

    public EnemyType m_type;

    public int m_healthPerLevel = 50;
    public int m_damagePerLevel = 5;
    public int m_xpPerLevel = 5;
    // Use this for initialization

    new private void Start()
    {
        base.Start();
    }


    void OnEnable()
    {
        
        if(m_expManager != null)
        {
            //health scaling
            m_currLevel = Random.Range(m_expManager.m_playerLevel, m_expManager.m_playerLevel + 2);
            m_currHealth = m_currLevel * m_healthPerLevel;
            m_maxHealth = m_currHealth;

            //damage scaling
            m_currDamage = m_currLevel * m_damagePerLevel;

            //exp scaling
            m_experienceValue = m_currLevel * m_xpPerLevel;
        }
    }

    // Update is called once per frame
    new void Update()
    {
        if (m_currHealth <= 0)
        {

            m_expManager.m_playerExperience += m_experienceValue;
            if (m_killStreakManager != null)
            {
                m_killStreakManager.AddKill();

            }

        }

        base.Update();

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            Bullet m_bulletScript = collision.collider.GetComponent<Bullet>();

            if (m_bulletScript != null && m_bulletScript.m_id == "Player")
            {
                m_agent.SetDestination(collision.collider.transform.position-m_bulletScript.m_direction);
            }
        }
    }
}
