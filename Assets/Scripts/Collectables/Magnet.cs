using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float m_attraction = 100;
    private Rigidbody m_rigidBody;
    // Use this for initialization
    void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerVacuum"))
        {
            GameObject player = other.gameObject;
            float distance = Vector3.Distance(player.transform.position, this.transform.position);
            Vector3 dir = player.transform.position - this.transform.position;
            dir.Normalize();

            Vector3 forceVect = Vector3.zero; 
            if (distance > 0.5f)
            {
                m_rigidBody.velocity = Vector3.zero;
                forceVect = (dir * m_attraction) / (distance * distance);
                m_rigidBody.AddForce(forceVect, ForceMode.Acceleration);
            }



            //this.transform.position += forceVect * Time.deltaTime;
        }
    }

}
