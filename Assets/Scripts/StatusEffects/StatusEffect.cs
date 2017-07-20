using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{

    public enum Status
    {
        OnFire,
        Stunned,
        Slowed,
        Buffed,
    }
    protected Health m_health;
    public Status m_type;
    public Transform m_manager;

    protected virtual void OnEnable()
    {
        m_health = this.GetComponentInParent<Health>();
    }

    protected virtual void Update()
    {

        if (m_health != null && m_health.m_currHealth <= 0.0f)
        {
            ReturnToSender();
        }
    }

    protected void ReturnToSender()
    {
        if (m_manager != null)
        {
            this.transform.position = m_manager.position;
            this.transform.SetParent(m_manager);
        }

        this.gameObject.SetActive(false);
    }
}
