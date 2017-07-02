using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [Header("ShooterID")]
    public string m_id;
    public GameObject m_projectile;
    public uint m_maxBulletsOnScreen = 50;

    public bool m_hasFireSplash = false;
    public bool m_hasIceSplit = false;


    protected List<GameObject> m_projectilePool = new List<GameObject>();
    protected List<Bullet> m_projectileScripts = new List<Bullet>();

    protected List<Bullet> m_activePool = new List<Bullet>();


    private int projectileCount = 0;
    // Use this for initialization
    void Start()
    {

        //object pooling
        if (m_projectile != null)
        {
            for (uint i = 0; i < m_maxBulletsOnScreen; ++i)
            {
                GameObject projectile = (GameObject)Instantiate(m_projectile,this.transform);
                projectile.SetActive(false);
                projectile.transform.parent = this.transform;
                m_projectilePool.Add(projectile);
                Bullet projectileScript = projectile.GetComponent<Bullet>();
                if (projectileScript != null)
                {
                    projectileScript.m_weaponFiredFrom = this;
                    projectileScript.m_id = m_id;
                    m_projectileScripts.Add(projectileScript);
                }
            }
        }
    }

    public virtual void Fire(Vector3 a_direction, float damagePerProjectile)
    {
        //set velocities and directions
    }
    

    //find inactive and insert into active pool
    protected void PoolToActive(Vector3 a_direction, float damagePerProjectile, int numOfProjectilesRequested)
    {
        m_activePool.Clear();
        for (int i = 0; i < m_maxBulletsOnScreen; ++i)
        {
            if (m_projectilePool.Count != 0)
            {
                if (!m_projectilePool[i].activeInHierarchy)
                {
                   
                    m_projectilePool[i].transform.parent = null;
                    m_projectilePool[i].transform.position = this.transform.position;
                    m_projectilePool[i].SetActive(true);

                    //if has AOE
                    m_projectileScripts[i].m_fireSplash = this.m_hasFireSplash;
                    m_projectileScripts[i].m_iceSplit = this.m_hasIceSplit;

                    m_projectileScripts[i].m_damage = damagePerProjectile;
                    //insert to active pool

                    m_activePool.Add(m_projectileScripts[i]);


                    if (projectileCount == numOfProjectilesRequested -1)
                    {
                        projectileCount = 0;
                        break;
                    }
                    projectileCount++;
                }
            }

        }
    }

    public void SetProjectile(GameObject a_projectile)
    {
        //clear pool
        m_projectilePool.Clear();
        m_projectileScripts.Clear();
        
        //set new projectile
        m_projectile = a_projectile;

        Start();

    }


}
