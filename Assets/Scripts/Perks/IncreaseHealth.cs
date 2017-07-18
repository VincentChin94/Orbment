using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : Perk
{

    private Player m_player;
    private Health m_playerHealth;
    public void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();

        if(m_player != null)
        {
            m_playerHealth = m_player.GetComponent<Health>();

        }

    }

    public override void Activate()
    {
        m_player.m_currHealthPoints++;
        if(m_playerHealth != null)
        {
            m_playerHealth.m_maxHealth += m_player.m_HealthIncrement;
        }
    }

}
