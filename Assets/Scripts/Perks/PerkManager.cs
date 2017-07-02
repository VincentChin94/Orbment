using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour {
	public Player m_player;
	public Health m_playerHealth;
	public Mana m_playerMana;
	private ExpManager m_XpManager;

	[SerializeField]
	private PerkType m_PerkTypeSelection;

	/// <summary>
	/// reference to the level up ui
	/// </summary>
	private LevelUpUI m_Luu;

	public GameObject m_UpgradeAvailableText = null;

	[SerializeField]
	public Perk[] m_StartingPerks = new Perk[3];

	[SerializeField]
	public List<Perk> m_perks = new List<Perk>();

	/// <summary>
	/// list of the currently shown perks
	/// </summary>
	private Perk[] m_ShownPerks = new Perk[3];

	/// <summary>
	/// list of usable perks
	/// </summary>
	private List<Perk> m_UsablePerks = new List<Perk>();

	private bool m_SelectingStartingPerks = true;


	private int m_AmtPerkUpgrades = -1;
	private bool m_ShowingUI = false;

	void Awake() {
		m_Luu = FindObjectOfType<LevelUpUI>();
		m_XpManager = FindObjectOfType<ExpManager>();

		m_player = GameObject.FindObjectOfType<Player>();

		if (m_player != null) {
			m_playerHealth = m_player.gameObject.GetComponent<Health>();
			m_playerMana = m_player.gameObject.GetComponent<Mana>();
		}

		showPerk(m_StartingPerks[0], 0);
		showPerk(m_StartingPerks[1], 1);
		showPerk(m_StartingPerks[2], 2);
		m_Luu.showUI();
		m_ShowingUI = true;
		m_UpgradeAvailableText.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		if(m_AmtPerkUpgrades == -1) {
			return;
		}
		if (m_ShowingUI) {
			KeyCode code = KeyCode.E;
			if (Input.GetKey(code)) {
				m_Luu.showUI();
				m_UpgradeAvailableText.SetActive(false);
				Time.timeScale = 0.0f;
			} else {
				m_Luu.hideUI();
				m_UpgradeAvailableText.SetActive(true);
				Time.timeScale = 1.0f;
			}

		}

	}

	private void genPerkList() {
		updateUseablePerks();
		//weight based on rarity
		pickPerks();

		showPerk(m_ShownPerks[0], 0);
		showPerk(m_ShownPerks[1], 1);
		showPerk(m_ShownPerks[2], 2);

		m_Luu.showUI();
	}

	private void showPerk(Perk a_Perk, int a_Index) {
		m_ShownPerks[a_Index] = a_Perk;
		m_Luu.setPerkInfo(a_Perk, a_Index);
	}

	private void updateUseablePerks() {
		m_UsablePerks.Clear();
		for (int i = 0; i < m_perks.Count; i++) {
			Perk bq = m_perks[i];
			bool shouldAdd = false;

			//math y0
			shouldAdd = bq.m_Unique ? bq.m_TimesUpgraded == 0 : true & bq.m_minLevel <= m_AmtPerkUpgrades & (bq.m_type == m_PerkTypeSelection | bq.m_type == PerkType.Normal);

			if (shouldAdd) {
				m_UsablePerks.Add(bq);
			}
			//print(bq.m_name + " - " + shouldAdd);
		}
	}
	private void pickPerks() {
		//reset perks
		m_ShownPerks[0] = null;
		m_ShownPerks[1] = null;
		m_ShownPerks[2] = null;
		float perkWeightMax = 0.0f;
		for (int i = 0; i < m_UsablePerks.Count; i++) {
			perkWeightMax += m_UsablePerks[i].m_PerkWeight;
		}

		for (int i = 0; i < 3; i++) {
			while (m_ShownPerks[i] == null && m_UsablePerks.Count != 0) {
				float randWeight = UnityEngine.Random.Range(0, perkWeightMax);
				float weightCalc = 0.0f;
				for (int q = 0; q < m_UsablePerks.Count; q++) {
					weightCalc += m_UsablePerks[q].m_PerkWeight;
					if (weightCalc >= randWeight) {

						//set perk
						m_ShownPerks[i] = m_UsablePerks[q];
						//remove perk from weight list, and remove it from the perk list
						perkWeightMax -= m_UsablePerks[q].m_PerkWeight;
						m_UsablePerks.Remove(m_UsablePerks[q]);

						break;
					}
				}

			}
		}

	}

	public void perkSelected(int a_PerkIndex) {
		if (m_SelectingStartingPerks) {
			m_PerkTypeSelection = m_StartingPerks[a_PerkIndex].m_type;
            m_SelectingStartingPerks = false;
		}
		if (m_ShownPerks[a_PerkIndex].m_UpgradeScript == null) {
			Debug.LogError("Perk " + m_ShownPerks[a_PerkIndex].m_name + "Does not have a Perk Upgrader(Upgrade Script) script on it");
		} else {
			m_ShownPerks[a_PerkIndex].m_UpgradeScript.upgrade();
		}
		m_ShownPerks[a_PerkIndex].m_TimesUpgraded++;

		m_AmtPerkUpgrades++;

		if(m_AmtPerkUpgrades != m_XpManager.m_playerLevel) {
			genPerkList();
			m_ShowingUI = true;
			return;
		}
		m_Luu.hideUI();
		m_ShowingUI = false;
		m_UpgradeAvailableText.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void leveledUp() {
		if (!m_ShowingUI) {
			genPerkList();
			m_ShowingUI = true;
		}
	}
}
