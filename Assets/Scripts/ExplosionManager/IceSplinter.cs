using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSplinter : MonoBehaviour
{
    public float m_seekRadius = 5;
    private Bullet[] m_shards;
    
    void Start()
    {
        m_shards = this.GetComponentsInChildren<Bullet>();

    }
    void OnEnable()
    {
        if(m_shards == null)
        {
            return;
        }
        //reset position
        for(int i = 0; i< m_shards.Length; ++i)
        {
            m_shards[i].gameObject.transform.position = this.transform.position;
            Vector3 direction = Random.onUnitSphere;
            direction.y = 0;
            m_shards[i].m_direction = direction.normalized;
            m_shards[i].gameObject.SetActive(true);
        }

        //Collider[] insideSphere = Physics.OverlapSphere(this.transform.position, m_seekRadius);


    }
}
