using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Bullet
{

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {
            m_explosionManager.RequestExplosion(collision.collider.transform.position, this.transform.forward, Explosion.ExplosionType.Lightning, m_damage);
        }


        if (!collision.collider.CompareTag(m_id))
        {
            Disable();
        }
    }





}
