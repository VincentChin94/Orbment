using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{

    public Texture2D m_expBarTexture;
    public Texture2D m_emptyBarTexture;
    public int m_expBarWidth = 500;
    public float m_playerExperience = 0.0f;
    public float m_playerMaxXP = 100.0f;
    public int m_playerLevel = 1;

	private PerkManager m_PerkManager;



    // Use this for initialization
    void Start()
    {
		m_PerkManager = FindObjectOfType<PerkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_playerExperience >= m_playerMaxXP)
        {
            LevelUp();

        }
    }


    private void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width - m_expBarWidth)/2, 0, m_expBarWidth, 10), m_emptyBarTexture);
        GUI.DrawTexture(new Rect((Screen.width - m_expBarWidth)/2, 0, m_expBarWidth * (m_playerExperience/m_playerMaxXP), 10), m_expBarTexture);
    }


    void LevelUp()
    {
        
        m_playerExperience = m_playerExperience - m_playerMaxXP;
        m_playerMaxXP += 0.25f*m_playerMaxXP;
        m_playerLevel++;
		//m_PerkManager.genPerkList();
		m_PerkManager.leveledUp();
		//LevelUpUI.m_Singleton.showUI();
	}


  
}
