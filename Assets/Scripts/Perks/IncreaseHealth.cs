using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : PerkUpgrader
{

    private Health m_Health;

    public int m_healthIncrease = 10;

    public void Start()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (player != null)
        {
            m_Health = player.gameObject.GetComponent<Health>();
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
