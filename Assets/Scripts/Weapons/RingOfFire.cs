using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    public int m_ringOfFireDPS = 5;
    public float m_tickInterval = 0.5f;
    private float m_elapsed = 0.0f;
    // Use this for initialization
    void Start()
    {

    }
    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Enemy"))
        {
            Health healthScript = col.GetComponent<Health>();

            if(healthScript != null)
            {
                m_elapsed += Time.deltaTime;
                if (m_elapsed >= m_tickInterval)
                {
                    m_elapsed = m_elapsed % m_tickInterval;
                    healthScript.m_currHealth -= m_ringOfFireDPS;
                }
                

            }
        }
    }
}
