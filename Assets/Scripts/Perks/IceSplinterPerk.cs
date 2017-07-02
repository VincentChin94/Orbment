﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSplinterPerk : PerkUpgrader
{

    private Player m_player;

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindObjectOfType<Player>();
    }


    public override void upgrade()
    {
        m_player.m_hasIceSplit = true;
    }
}
