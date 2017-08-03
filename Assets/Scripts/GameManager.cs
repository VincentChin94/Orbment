using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
	public int healthCurrent;
	public int healthMax;

	public GameObject healthBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.GetComponent<Slider> ().value = healthCurrent;
		healthBar.GetComponent<Slider> ().maxValue = healthMax;
	}
}
