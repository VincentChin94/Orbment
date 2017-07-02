using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollectable : MonoBehaviour
{
    public GameObject m_collectable;
    public int m_numberOfOrbsDropped = 1;
    // Use this for initialization
    //private GameObject 

    private Health m_healthScript;
    private GameObject[] m_collectables;
    private bool doOnce = false;

    private void Start()
    {
        m_collectables = new GameObject[m_numberOfOrbsDropped];
        m_healthScript = this.GetComponent<Health>();
        for (int i = 0; i < m_numberOfOrbsDropped; ++i)
        {

            m_collectables[i] = GameObject.Instantiate(m_collectable, this.transform.position, Quaternion.identity);
            m_collectables[i].transform.parent = this.transform;
            m_collectables[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (m_healthScript != null && m_healthScript.m_currHealth <= 0 && !doOnce)
        {
            for (int i = 0; i < m_collectables.Length; ++i)
            {
                m_collectables[i].transform.parent = null;
                m_collectables[i].SetActive(true);
            }
            doOnce = true;
        }
    }


}
