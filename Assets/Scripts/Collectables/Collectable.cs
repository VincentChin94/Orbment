using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Player m_playerRef;
    // Use this for initialization
    void Start()
    {
        m_playerRef = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            m_playerRef.m_orbsCollected++;
            //destroy collectable
            GameObject.Destroy(this.gameObject);
        }
    }


}
