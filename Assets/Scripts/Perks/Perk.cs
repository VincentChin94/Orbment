using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkType {
	Normal,//non specific
	Fire,//for fire mage class
	Ice,//for ice mage class
	Lightning,//for lightning mage

}

[System.Serializable]
public class Perk
{



    protected enum PerkRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,

    }

    //name
    public string m_name = "UnamedPerk";
	//type of perk
	public PerkType m_type;
    //rarity of perk
    protected PerkRarity m_rarity;


	/// <summary>
	/// higher == more likely
	/// </summary>
	public float m_PerkWeight;

	//minimum level for chance of perk to appear
	public int m_minLevel = 0;

	/// <summary>
	/// is the perk unique?
	/// </summary>
	public bool m_Unique;

	/// <summary>
	/// how many times this perk has been upgraded
	/// </summary>
	[HideInInspector]
	public int m_TimesUpgraded = 0;

	/// <summary>
	/// image for this perk
	/// </summary>
	public Sprite m_PerkImage;

	/// <summary>
	/// which script will run when this perk is upgraded
	/// </summary>
	public PerkUpgrader m_UpgradeScript;

}
