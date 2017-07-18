using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeProjectile: Perk
{
    public GameObject m_projectile;
    private Player m_player;
    private BaseWeapon m_playerWeapon;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();

        if(m_player != null)
        {
            m_playerWeapon = m_player.m_currWeapon;
        }
        
    }

    public override void Activate()
    {
       if(m_playerWeapon != null && m_projectile != null)
        {
            m_player.m_currentProjectile = m_projectile;
            m_playerWeapon.SetProjectile(m_projectile);
        }
    }
}
