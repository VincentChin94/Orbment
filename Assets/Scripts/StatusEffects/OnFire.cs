﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : StatusEffect
{
    public float m_lifetime = 3.0f;
    public float m_fireDmgTknPerSec = 5.0f;
    private float m_timer = 0.0f;
    private float m_ticker = 0.0f;


    // Use this for initialization
    void Start()
    {
        this.m_type = Status.OnFire;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_health != null)
        {
            m_health.m_onFire = true;
        }
    }
    // Update is called once per frame
    protected override void Update()
    {

        base.Update();

        m_timer += Time.deltaTime;
        m_ticker += Time.deltaTime;

        if (m_ticker >= 1.0f)
        {
            if (m_health != null)
            {

                m_health.m_currHealth -= m_fireDmgTknPerSec;
            }
            m_ticker = 0.0f;

        }

        if (m_timer >= m_lifetime)
        {
            if (m_health != null)
            {
                m_health.m_onFire = false;
            }

            ReturnToSender();

            m_timer = 0.0f;
        }
    }




}
