using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunPerk : PerkUpgrader
{
    private Player m_player;

    public void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();

    }

    public override void upgrade()
    {
        if (m_player != null)
        {
            m_player.m_hasStunPerk = true;
        }

    }
}
