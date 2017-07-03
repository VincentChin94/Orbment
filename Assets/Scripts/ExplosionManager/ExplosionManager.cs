using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    /// <summary>
    /// Object pooled explosions
    /// </summary>
    public enum ExplosionType
    {
        Fire,
        Ice,
        Lightning,
    }


    public GameObject m_iceSplinterPrefab;
    public GameObject m_fireExplosionPrefab;
    public GameObject m_lightningExplosionPrefab;



    private List<GameObject> m_fireExplosionPool = new List<GameObject>();
    private List<GameObject> m_IceExplosionPool = new List<GameObject>();
    private List<GameObject> m_lightningExplosionPool = new List<GameObject>();

    public int m_poolAmount = 20;

    // Use this for initialization
    void Start()
    {
        //object pooling

        for (uint i = 0; i < m_poolAmount; ++i)
        {

            if (m_fireExplosionPrefab != null)
            {
                GameObject fireExplosion = (GameObject)Instantiate(m_fireExplosionPrefab, this.transform);
                fireExplosion.SetActive(false);
                fireExplosion.transform.parent = this.transform;
                m_fireExplosionPool.Add(fireExplosion);

            }

            if (m_iceSplinterPrefab != null)
            {
                GameObject iceSplinter = (GameObject)Instantiate(m_iceSplinterPrefab, this.transform);
                iceSplinter.SetActive(false);
                iceSplinter.transform.parent = this.transform;
                m_IceExplosionPool.Add(iceSplinter);

            }

            if (m_lightningExplosionPrefab != null)
            {
                GameObject lightningBolt = (GameObject)Instantiate(m_lightningExplosionPrefab, this.transform);
                lightningBolt.SetActive(false);
                lightningBolt.transform.parent = this.transform;
                m_lightningExplosionPool.Add(lightningBolt);

            }
        }

    }

    public GameObject RequestExplosion(Vector3 a_position, ExplosionType a_type, float a_damage)
    {
        GameObject explosion = null;

        switch (a_type)
        {
            case ExplosionType.Fire:
                {
                    explosion = FindInactive(m_fireExplosionPool);

                    break;
                }
            case ExplosionType.Ice:
                {
                    explosion = FindInactive(m_IceExplosionPool);
                    break;
                }
            case ExplosionType.Lightning:
                {
                    explosion = FindInactive(m_lightningExplosionPool);
                    break;
                }
        }

        if (explosion != null)
        {
            explosion.transform.position = a_position;
            Explosion explosionScript = explosion.GetComponent<Explosion>();
            if (explosionScript != null)
            {
                explosionScript.m_damage = a_damage;
            }

            explosion.SetActive(true);
        }
        return explosion;
    }

    GameObject FindInactive(List<GameObject> a_pool)
    {
        for (int i = 0; i < a_pool.Count; ++i)
        {
            if(!a_pool[i].activeInHierarchy)
            {
                return a_pool[i];
                
            }
        }

        return null;

    }
}
