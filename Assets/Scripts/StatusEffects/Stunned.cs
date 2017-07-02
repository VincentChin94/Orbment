using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stunned : StatusEffect
{
    public float m_lifetime = 2.0f;
 
    private float m_timer = 0.0f;
    private NavMeshAgent m_agent;
    // Use this for initialization
    void Start()
    {
        this.m_type = Status.Stunned;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_health != null)
        {
            m_health.m_isStunned = true;
        }

        m_agent = this.GetComponentInParent<NavMeshAgent>();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        m_timer += Time.deltaTime;

        if (m_agent != null)
        {
            m_agent.speed = 0.0f;
        }

        if (m_timer >= m_lifetime)
        {
            if (m_health != null)
            {
                m_health.m_isStunned = false;
            }

            if (m_agent != null && m_health != null)
            {
                m_agent.speed = m_health.m_originalMoveSpeed;
            }

            ReturnToSender();

            m_timer = 0.0f;
        }
    }
}
