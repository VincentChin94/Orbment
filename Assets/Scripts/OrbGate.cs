using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGate : MonoBehaviour {

	private Player m_Player;
	private Animator m_Animator;

	public int m_NumOfOrbsForOpen = 10;

	//probs useless :/
	private bool m_HasOpened = false;



	public void Awake() {
		m_Player = FindObjectOfType<Player>();
		m_Animator = GetComponentInChildren<Animator>();
	}


	//public void Update() {
	//		checkIfShouldOpen();
	//}

	public void OnTriggerStay(Collider other) {
		//maybe check layer instead? or if it has the Player script
		if (other.gameObject == m_Player.gameObject) {
			checkIfShouldOpen();
		}
	}

	private void checkIfShouldOpen() {
		if (m_HasOpened) {
			return;
		}
		if (m_Player.m_orbsCollected >= m_NumOfOrbsForOpen) {
			m_Animator.SetTrigger("OpenGate");
			m_HasOpened = true;
		}
	}
}
