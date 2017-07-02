﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : BaseWeapon
{
    
    
    public override void Fire(Vector3 a_direction, float damagePerProjectile)
    {

        base.PoolToActive(a_direction, damagePerProjectile, 3);


        for (int i = 0; i < m_activePool.Count; ++i)
        {
            switch (i)
            {
                case 0:
                    m_activePool[i].m_direction = a_direction;
                    break;
                case 1:
                    m_activePool[i].m_direction = Quaternion.Euler(0, 5, 0) * a_direction;
                    break;
                case 2:
                    m_activePool[i].m_direction = Quaternion.Euler(0, -5, 0) * a_direction;
                    break;
            }
        }

    }
}
