using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamage : Perk
{
    private Player m_player;
    // Use this for initialization
    void Start()
    {
        this.m_id = PerkID.IncreaseDamage;
        
        m_player = GameObject.FindObjectOfType<Player>();
    }


    public override void Activate()
    {
        m_player.m_currDamagePoints++;
    }
}
