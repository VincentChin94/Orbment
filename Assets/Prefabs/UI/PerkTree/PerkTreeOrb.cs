using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkTreeOrb : MonoBehaviour {
	Vector3[] linePosition;

	public GameObject[] branchLength;
	GameObject[] perkOrbs;
	public GameObject perkChild;

	int positionAmount;
	bool perkActivated = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		perkOrbs = GameObject.FindGameObjectsWithTag ("perkOrb");


		if (perkActivated) {
			foreach (GameObject perkchildren in branchLength) {
				perkchildren.transform.GetChild(0).gameObject.SetActive(true);
			}
			GetComponent<LineRenderer> ().enabled = true;
			positionAmount = GetComponent<LineRenderer> ().numPositions;
			GetComponent<LineRenderer> ().numPositions = branchLength.Length;

			for (int i = 0; i < positionAmount; i++) {
				GetComponent<LineRenderer> ().SetPosition (i, branchLength [i].transform.position);

			}
		} else {
			GetComponent<LineRenderer> ().enabled = false;
//			foreach (GameObject perkchildren in branchLength) {
//				perkchildren.transform.GetChild(0).gameObject.SetActive(false);
//			}
		}
	}
	void OnMouseDown(){
		unClickOrbs ();

		perkActivated = true;
	}
	void unClickOrbs(){
		foreach (GameObject orb in perkOrbs) {
			orb.transform.GetChild(0).gameObject.SetActive(false);
			orb.GetComponent<PerkTreeOrb>().perkActivated = false;

		}
	}


}
