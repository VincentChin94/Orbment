using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffed : StatusEffect
{
    public int m_damageMultplier = 2;

    private int m_originalMult = 1;
    private Player m_player;

    // Use this for initialization
    void Start()
    {
        this.m_type = Status.Buffed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_health != null)
        {
            m_health.m_isBuffed = true;

            m_player = m_health.GetComponent<Player>();

            if (m_player != null)
            {
                m_originalMult = m_player.m_currDamageMult;
                m_player.m_currDamageMult = m_damageMultplier;
            }
        }


    }

    void OnDisable()
    {
        if(m_health != null)
        {
            m_health.m_isBuffed = false;
        }
        
        if (m_player != null)
        {
            m_player.m_currDamageMult = m_originalMult;
        }
    }
    // Update is called once per frame
    protected override void Update()
    {

        base.Update();

        if(m_health != null)
        {
            if(!m_health.HealthBelowPercentCheck(10))
            {
                m_health.m_isBuffed = false;
                ReturnToSender();
            }
        }
    }
}
