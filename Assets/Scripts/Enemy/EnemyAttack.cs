﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(FindObjectsInRadius))]
public class EnemyAttack : MonoBehaviour
{
    public int m_damage = 20;
    public float m_attackInterval = 1.0f;
    private FindObjectsInRadius m_foir;
    private float m_attackTimer = 0.0f;

    public bool m_CanAttack = true;

    // Use this for initialization
    void Start()
    {
       
        m_foir = this.GetComponent<FindObjectsInRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_CanAttack) {
            return;
        }
        if (m_foir != null && m_foir.inRange)
        {
            this.transform.LookAt(m_foir.m_target);
            if (m_attackTimer >= m_attackInterval)
            {
                m_attackTimer = 0.0f;
            }

            if (m_attackTimer == 0.0f)
            {
                if (m_foir.m_target != null)
                {
                    Entity player = m_foir.m_target.GetComponent<Entity>();

                    if (player != null)
                    {
                        player.m_currHealth -= m_damage;
                       
                    }
                }
            }


            m_attackTimer += Time.deltaTime;
        }

    }
}
