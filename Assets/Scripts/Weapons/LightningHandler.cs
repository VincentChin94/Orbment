using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LightningHandler : MonoBehaviour
{

    private const float m_Range = 10.0f;
    private const float m_TimeBetweenHits = 0.05f;
    public float m_BaseDamage = 10.0f;
    private bool m_firstTarget = true;
    //list of objects hit by this lighting
    private List<GameObject> m_Hit = new List<GameObject>();

    private GameObject m_LastHit = null;

    private float m_TimeLastHit = 0;

    private LineRenderer m_LineRenderer;


    public void startAttack(GameObject a_Enemy)
    {
        //print("Start");
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.startWidth = 0.1f;
        m_LineRenderer.endWidth = 0.1f;

        m_LineRenderer.startColor = new Color(0.5f, 0, 1);
        m_LineRenderer.endColor = new Color(0.5f, 0, 1);

        m_LineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        m_LineRenderer.SetPosition(0, Vector3.zero);
        m_LineRenderer.SetPosition(1, Vector3.zero);

        //setting alignment via code is impossible in 5.5, requires 5.6
        //set alignment to local
        //also requires transform to be rotated on the x by 90
        //m_LineRenderer.alignment = ??;

        attackEnemy(a_Enemy);
        resetTimer();

    }

    public void Update()
    {
        if (m_LastHit == null)
        {
            return;
        }
        if (Time.time - m_TimeLastHit > m_TimeBetweenHits)
        {
            resetTimer();
            findNearby();

        }
        ////maybe reset line renderPositions here??
        if (Time.time - m_TimeLastHit > 0.5f)
        {
            m_LineRenderer.SetPosition(0, Vector3.zero);
            m_LineRenderer.SetPosition(1, Vector3.zero);
        }


    }

    public void attackEnemy(GameObject a_Object)
    {
        //print("ATTACK " +  a_Object.transform.name);
        if (m_LastHit != null)
        {
            m_LineRenderer.SetPosition(0, m_LastHit.transform.position);
        }
        else
        {
            m_LineRenderer.SetPosition(0, a_Object.transform.position);
        }
        m_LineRenderer.SetPosition(1, a_Object.transform.position);

        m_Hit.Add(a_Object);

        m_LastHit = a_Object;

        if(!m_firstTarget)
        {
            dealDamage();
        }
        

        m_BaseDamage *= 0.75f;
        if (m_BaseDamage <= 0.01f)
        {
            //damage is too low, lightning will stop
            m_LastHit = null;
        }

        m_firstTarget = false;
    }

    private void findNearby()
    {
        Collider[] nearby = Physics.OverlapSphere(m_LastHit.transform.position, m_Range);

        //go through and find closest, remove any from the m_Hiy list
        GameObject closest = null;
        float closestDist = 0;
        for (int i = 0; i < nearby.Length; i++)
        {
            GameObject go = nearby[i].gameObject;
            if (go.GetComponent<EnemyAttack>() == null)
            {
                continue;
            }
            bool hasBeenHitBefore = false;
            for (int q = 0; q < m_Hit.Count; q++)
            {
                if (go == m_Hit[q])
                {
                    hasBeenHitBefore = true;
                    break;
                }
            }
            if (hasBeenHitBefore)
            {
                continue;
            }
            float distance = Vector3.Distance(m_LastHit.transform.position, go.transform.position);

            if (closestDist > distance || closest == null)
            {
                closest = go;
                closestDist = distance;
                continue;
            }
        }

        //if cant find any, make m_LastHit null, script will wait for it's deletion
        if (closest == null)
        {
            m_LastHit = null;
            return;
        }

        //call attackEnemy on it
        attackEnemy(closest);
    }

    private void dealDamage()
    {
        Health enemyHealth = m_LastHit.GetComponent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.m_currHealth -= m_BaseDamage;
        }

        resetTimer();
    }

    private void resetTimer()
    {
        m_TimeLastHit = Time.time;
    }

}
