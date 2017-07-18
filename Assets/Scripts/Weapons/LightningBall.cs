using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Bullet
{
    public int m_godBoltChance = 10;
    private bool m_hasGodBolt = false;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {
            m_explosionManager.RequestExplosion(collision.collider.transform.position, this.transform.forward, Explosion.ExplosionType.Lightning, m_damage);


            if(!m_hasGodBolt && m_playerRef != null && m_playerRef.m_perks.Contains(PerkID.GodBolt))
            {
                //do once
                m_hasGodBolt = true;
            }

            if(m_hasGodBolt)
            {
                if (Random.Range(0, 100) <= m_godBoltChance)
                {
                    m_explosionManager.RequestExplosion(collision.collider.transform.position, this.transform.forward, Explosion.ExplosionType.GodLightning, m_damage);
                }

            }
            
        }


        if (!collision.collider.CompareTag(m_id))
        {
            Disable();
        }
    }





}
