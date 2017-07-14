using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class LightningField : MonoBehaviour
{
    public int m_lightningFieldDPS = 5;
    public float m_tickInterval = 0.5f;
    private float m_elapsed = 0.0f;

    private LineRenderer m_line;
    private List<Vector3> m_positions = new List<Vector3>();
    // Use this for initialization
    void Start()
    {
        m_line = this.GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        m_line.numPositions = 0;
        int numEnemies = 1;
        m_positions.Clear();
       
        m_positions.Add(this.transform.position);
       Collider[] inSphere =  Physics.OverlapSphere(this.transform.position, 10);


        for (int i = 0; i < inSphere.Length; ++i )
        {
            if(inSphere[i].CompareTag("Enemy"))
            {
                m_positions.Add(inSphere[i].transform.position);
                m_positions.Add(this.transform.position);
                numEnemies += 2;
            }
        }


        m_line.numPositions = numEnemies;
        m_line.SetPositions(m_positions.ToArray());


    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Health healthScript = col.GetComponent<Health>();

            if (healthScript != null)
            {
                m_elapsed += Time.deltaTime;
                if (m_elapsed >= m_tickInterval)
                {
                    m_elapsed = m_elapsed % m_tickInterval;
                    healthScript.m_currHealth -= m_lightningFieldDPS;
                }


            }
        }
    }
}
