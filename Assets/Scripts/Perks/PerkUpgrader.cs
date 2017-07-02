using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUpgrader : MonoBehaviour {

	public virtual void upgrade() {
		Debug.LogError("Perk("+GetType().Name+") on object ("+transform.name+") does not overwrite upgrade function!");
	}


}
