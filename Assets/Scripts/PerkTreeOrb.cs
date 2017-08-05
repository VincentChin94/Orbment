using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkTreeOrb : MonoBehaviour {
	private Vector3[] linePosition;

	public GameObject[] branchLength;
	private GameObject[] perkOrbs;
	public GameObject perkChild;

	int positionAmount;
	bool perkActivated = false;

	LineRenderer lineRend;

	void Start () 
	{
		lineRend = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//Finding all Orbs, Change this later to individual Orb Types. 
		perkOrbs = GameObject.FindGameObjectsWithTag ("perkOrb");

		//If an orb has been clicked on, create a link down the branch. 
		if (perkActivated) 
		{
			//Turns on the glow for all perks in the perk branch. 
			foreach (GameObject perkchildren in branchLength) 
			{
				perkchildren.transform.GetChild(0).gameObject.SetActive(true);
			}
			lineRend.enabled = true;
			lineRend.numPositions = branchLength.Length;
			positionAmount = lineRend.numPositions;

			//Sets the line rend for all branch positions
			for (int i = 0; i < positionAmount; i++) 
			{
				lineRend.SetPosition (i, branchLength [i].transform.position);

			}
		} 
		else 
		{
			lineRend.enabled = false;
		}
	}

	//Enables new branch
	void OnMouseDown()
	{
		//Disables all orbs before enabling the next requested branch
		unClickOrbs ();
		perkActivated = true;
	}

	//Disables All Perk Tree's 
	void unClickOrbs()
	{
		foreach (GameObject orb in perkOrbs) 
		{
			orb.transform.GetChild(0).gameObject.SetActive(false);
			orb.GetComponent<PerkTreeOrb>().perkActivated = false;

		}
	}


}
