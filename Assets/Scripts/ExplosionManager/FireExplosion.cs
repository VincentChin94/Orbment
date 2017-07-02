using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    public float m_damage = 5.0f;


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Health healthScript = col.GetComponent<Health>();

            if (healthScript != null)
            {

                healthScript.m_currHealth -= m_damage;

            }
        }
    }
}
