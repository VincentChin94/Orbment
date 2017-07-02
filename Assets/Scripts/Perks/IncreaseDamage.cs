using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamage : PerkUpgrader
{
    private Player m_player;
    public float m_damageIncrease = 10.0f;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();
    }


    public override void upgrade()
    {
        m_player.m_currentDamagePerProjectile += m_damageIncrease;
    }
}
