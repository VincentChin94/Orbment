using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    public Texture2D m_manaBarTexture;
    public Texture2D m_emptyBarTexture;
    public int m_manaBarWidth = 500;
    public float m_currentMana = 100.0f;
    public float m_maxMana = 100.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect((Screen.width - m_manaBarWidth) / 2, Screen.height-60, m_manaBarWidth, 10), m_emptyBarTexture);
        GUI.DrawTexture(new Rect((Screen.width - m_manaBarWidth) / 2, Screen.height-60, m_manaBarWidth * (m_currentMana / m_maxMana), 10), m_manaBarTexture);
    }


}
