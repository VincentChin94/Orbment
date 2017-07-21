using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    //public int m_ringOfFireDPS = 5;
    //public float m_tickInterval = 0.5f;
    //private float m_elapsed = 0.0f;
    //private StatusEffectManager m_statusEffectManager;
    // Use this for initialization
    void Start()
    {
        //m_statusEffectManager = GameObject.FindObjectOfType<StatusEffectManager>();
    }
    void OnTriggerStay(Collider col)
    {
        if(col.CompareTag("Enemy"))
        {

            Entity entity = col.GetComponent<Entity>();

            if (entity != null)
            {
                entity.m_setOnFire = true;
            }

        }
    }
}
