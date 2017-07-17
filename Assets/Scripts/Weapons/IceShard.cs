using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        //set on fire
        if (m_enemyHealth != null)
        {
            m_enemyHealth.m_causeSlow = true;
        }

        if (m_iceSplit)
        {
            m_explosionManager.RequestExplosion(this.transform.position, this.transform.forward, Explosion.ExplosionType.Ice, m_damage);
        }

        if (!collision.collider.CompareTag(m_id))
        {
            Disable();
        }
    }
}
