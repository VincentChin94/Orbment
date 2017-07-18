using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Bullet
{
    public float m_stunChance = 25.0f;
    private bool m_hasSplashDamagePerk = false;
    private bool m_hasStunPerk = false;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {
            if(!m_hasStunPerk && m_playerRef != null && m_playerRef.m_perks.Contains(PerkID.StunChance))
            {
                //do once
                m_hasStunPerk = true;
            }

            //stun
            if (m_hasStunPerk && Random.Range(0.0f, 100.0f) <= m_stunChance)
            {
                m_enemyHealth.m_causeStun = true;
            }

            m_enemyHealth.m_setOnFire = true;
        }


        //if AOE toggled on 

        if(!m_hasSplashDamagePerk && m_playerRef != null && m_playerRef.m_perks.Contains(PerkID.SplashDamage))
        {
            //do once
            m_hasSplashDamagePerk = true;
        }

        if (m_hasSplashDamagePerk)
        {
            m_explosionManager.RequestExplosion(collision.contacts[0].point, this.transform.forward, Explosion.ExplosionType.Fire, m_damage);

        }

        if (!collision.collider.CompareTag(m_id))
        {
            Disable();
        }
    }



}
