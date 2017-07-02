using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
   
    public int m_poolAmount = 50;
    //originals
    public GameObject[] m_statusEffects;

    private List<GameObject> m_pool = new List<GameObject>();
    private List<StatusEffect> m_scriptPool = new List<StatusEffect>();
    // Use this for initialization
    void Start()
    {
        foreach(GameObject obj in m_statusEffects)
        {
            for(int i = 0; i < m_poolAmount; ++i)
            {
                GameObject effectObj = Instantiate(obj, this.transform);
                effectObj.SetActive(false);
                effectObj.transform.parent = this.transform;

                m_pool.Add(effectObj);

                StatusEffect effectScript = effectObj.GetComponent<StatusEffect>();
                if(effectScript != null)
                {
                    effectScript.m_manager = this.transform;
                    m_scriptPool.Add(effectScript);
                }
                
            }
        }

    }

    public void RequestEffect(Transform obj, StatusEffect.Status a_type)
    {
        GameObject effect = FindInactive(a_type);

        if(effect != null)
        {
            effect.transform.position = obj.position;
            effect.transform.SetParent(obj);
            effect.SetActive(true);
        }
    }

    GameObject FindInactive( StatusEffect.Status a_type)
    {
        for (int i = 0; i < m_pool.Count; ++i)
        {
            if (!m_pool[i].activeInHierarchy && m_scriptPool[i].m_type == a_type)
            {
                return m_pool[i];

            }
        }

        return null;

    }

}
