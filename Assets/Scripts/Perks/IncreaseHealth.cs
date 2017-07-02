using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : PerkUpgrader
{

    private Health m_Health;

    public float m_healthIncrease = 10.0f;

    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            m_Health = player.GetComponent<Health>();
        }
    }

    public override void upgrade()
    {
        if(m_Health != null)
        {
            m_Health.m_maxHealth += m_healthIncrease;

            //temporary
            m_Health.m_currHealth = m_Health.m_maxHealth;
        }

    }
}
