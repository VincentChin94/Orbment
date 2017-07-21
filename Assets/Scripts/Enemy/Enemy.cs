using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
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
}
