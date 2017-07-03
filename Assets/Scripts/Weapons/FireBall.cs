using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{


    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {

            //stun
            if (m_hasStunPerk && Random.Range(0.0f, 100.0f) <= m_StunChance)
            {
                m_enemyHealth.m_causeStun = true;
            }

            m_enemyHealth.m_setOnFire = true;
        }


        //if AOE toggled on 
        if (m_fireSplash)
        {
            m_explosionManager.RequestExplosion(collision.contacts[0].point, ExplosionManager.ExplosionType.Fire, m_damage);

        }

        if (!collision.collider.CompareTag(m_id))
        {
            Disable();
        }
    }



}
