using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCam : MonoBehaviour
{
    public float m_camMoveSpeed = 10;
    public float m_camRotSpeed = 10;
    public GameObject m_target;
    private Vector3 offset;
    // Use this for initialization
    void Start()
    {
        offset = this.transform.position - m_target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, m_target.transform.position + offset, m_camMoveSpeed);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(m_target.transform.position - this.transform.position), m_camRotSpeed);

        }
    }
}
