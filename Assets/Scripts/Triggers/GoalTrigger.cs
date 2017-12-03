using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {

	//Hold all characters in current level
	public Movement[] Characters;

	//Flags for whether or not these characters have entered the goal
	bool Char1Entered = false;
	bool Char2Entered = false;
	bool Char3Entered = false;
	bool Char4Entered = false;

	//Flag to make sure the trigger code only fires once
	bool entered = false;
	//Flag to make sure that the starting number of characters in the level is only counted once
	bool total = false;

	int numChars;

	void OnTriggerEnter2D(Collider2D other) {

        GameManager.Instance.GoalReached(other.gameObject);
		
	}
}
