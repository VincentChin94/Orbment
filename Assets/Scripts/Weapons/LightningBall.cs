using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Bullet
{
    private Player m_player;

    protected override void Start()
    {
        base.Start();
        m_player = GameObject.FindObjectOfType<Player>();
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {
            m_explosionManager.RequestExplosion(collision.collider.transform.position, this.transform.forward, Explosion.ExplosionType.Lightning, m_damage);

            if(m_player != null && m_player.m_hasGodLightning)
            {
                if(Random.Range(0,100) <= 10)
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
