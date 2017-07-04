﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCam : MonoBehaviour
{
    public float m_camMoveSpeed = 10;
    public float m_camRotSpeed = 10;
    public GameObject m_target;
    private Vector3 offset;

    private bool m_shake = false;
    private float m_shakeTimer = 0.0f;
    private float m_shakeDuration = 0.0f;
    private float m_shakeAmount = 0.0f;
    // Use this for initialization
    void Start()
    {
        offset = this.transform.position - m_target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target != null)
        {
          
           this.transform.position = Vector3.MoveTowards(this.transform.position, m_target.transform.position + offset, m_camMoveSpeed);
           this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(m_target.transform.position - this.transform.position), m_camRotSpeed);

        }

        if(m_shake)
        {
            m_shakeTimer += Time.deltaTime;

            Shake(m_shakeAmount, m_shakeDuration);

            if(m_shakeTimer >= m_shakeDuration)
            {
                m_shake = false;
                m_shakeTimer = 0.0f;
            }
        }
    }

    public void Shake(float a_shakeAmount, float a_shakeDuration)
    {
        m_shakeAmount = a_shakeAmount;
        m_shakeDuration = a_shakeDuration;
       
        m_shake = true;
        this.transform.position += Random.onUnitSphere * a_shakeAmount;
    }


}
