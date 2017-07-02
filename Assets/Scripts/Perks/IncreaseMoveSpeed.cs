using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMoveSpeed : PerkUpgrader
{
    private Player m_player;
    public int m_speedIncrease = 10;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();
    }

    public override void upgrade()
    {
        m_player.m_playerMoveSpeed += m_speedIncrease;
    }

}
