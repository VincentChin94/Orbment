using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float m_attraction = 100;

    // Use this for initialization
    void Start()
    {

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


            Vector3 forceVect = (dir * m_attraction) / (distance * distance);


            this.transform.position += forceVect * Time.deltaTime;
        }
    }

}
