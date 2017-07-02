using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : PerkUpgrader
{

    private Player m_player;
    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    public override void upgrade()
    {
        m_player.m_hasFireSplash = true;
    }
}
