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

    private Player m_player;

    private List<GameObject> m_fireExplosionPool = new List<GameObject>();
    private List<GameObject> m_IceExplosionPool = new List<GameObject>();
    private List<GameObject> m_lightningExplosionPool = new List<GameObject>();

    private IsoCam m_camera;

    public int m_poolAmount = 20;

    // Use this for initialization
    void Start()
    {
        //object pooling
        m_player = GameObject.FindObjectOfType<Player>();
        m_camera = GameObject.FindObjectOfType<IsoCam>();

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
            if (m_camera != null && a_type != ExplosionType.Lightning)
            {
                float shockWave = 100.0f / Vector3.Distance(explosion.transform.position, m_player.transform.position);
                if(explosion.transform.position == m_player.transform.position)
                {
                    shockWave = 50.0f;
                }
                m_camera.Shake(shockWave, 0.2f);
            }
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
