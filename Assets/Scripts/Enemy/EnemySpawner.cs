using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] m_enemyPrefabs;
    public int m_poolAmount = 20;
    public float m_maxRange = 10;
    private List<GameObject> m_pooledEnemies = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        if(m_enemyPrefabs.Length == 0)
        {
            return;
        }

        for(int i = 0; i < m_poolAmount; ++i)
        {
            int enemyType = Random.Range(0, m_enemyPrefabs.Length);

            if (m_enemyPrefabs[enemyType] != null)
            {
                Vector3 newPosition = Random.insideUnitSphere * Random.Range(0, m_maxRange);
                newPosition.y = 0;
                GameObject newEnemy = GameObject.Instantiate(m_enemyPrefabs[enemyType],newPosition, Quaternion.identity);
                m_pooledEnemies.Add(newEnemy);
                
                
            }

               
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
