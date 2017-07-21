﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Player m_playerRef;
    Mana m_playerMana;
    public enum CollectableType
    {
        YellowOrb,
        GreenOrb,
        BlueOrb,
    }

    public CollectableType m_type;

    public int m_healAmount = 10;
    public int m_manaAmount = 100;


    // Use this for initialization
    void Start()
    {
        m_playerRef = GameObject.FindObjectOfType<Player>();
        if(m_playerRef != null)
        {
      
            m_playerMana = m_playerRef.GetComponent<Mana>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            switch (m_type)
            {
                case CollectableType.YellowOrb:
                    {
                        if(m_playerRef)
                        {
                            m_playerRef.m_orbsCollected++;
                        }
                        

                        break;
                    }

                case CollectableType.GreenOrb:
                    {
                        if (m_playerRef)
                        {
                            m_playerRef.m_currHealth += m_healAmount;
                        }
                        break;
                    }

                case CollectableType.BlueOrb:
                    {
                        m_playerMana.m_currentMana += m_manaAmount;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            this.gameObject.SetActive(false);
        }
    }


}
