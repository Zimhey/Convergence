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

		if (!entered) {
			entered = true;
			if (!total) {
				//Populate object array with all types of existing players on the board
				//@TODO fix this with other characters
				Characters = GameObject.FindObjectsOfType<Movement>();
				//Characters = GameObject.FindGameObjectsWithTag ("DoubleMove");
				//Characters = GameObject.FindGameObjectsWithTag ("IceBoi");
				//Characters = GameObject.FindGameObjectsWithTag ("?????");

				//Check length of the array to see how many character's we're dealing with
				numChars = Characters.Length;

				Debug.Log (numChars);
				total = true;
			}

			//The most hideously disgusting if statement you've ever seen
			if (numChars == 4) {
				if (!Char1Entered && !Char2Entered && !Char3Entered && !Char4Entered) {
					Char1Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && !Char2Entered && !Char3Entered && !Char4Entered) {
					Char2Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && Char2Entered && !Char3Entered && !Char4Entered) {
					Char3Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && Char2Entered && Char3Entered && !Char4Entered) {
					Char4Entered = true;
					SceneManager.LoadScene ("LevelBuilder-Backup");
				}
			} else if (numChars == 3) {
				if (!Char1Entered && !Char2Entered && !Char3Entered) {
					Char1Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && !Char2Entered && !Char3Entered) {
					Char2Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && Char2Entered && !Char3Entered) {
					Char3Entered = true;
					SceneManager.LoadScene ("LevelBuilder-Backup");
				}
			} else if (numChars == 2) {
				if (!Char1Entered && !Char2Entered) {
					Char1Entered = true;
					Destroy (other.gameObject);
					entered = false;
				} else if (Char1Entered && !Char2Entered) {
					Char2Entered = true;
					SceneManager.LoadScene ("LevelBuilder-Backup");
				}
			} else {
				SceneManager.LoadScene ("LevelBuilder-Backup");
			}
		}
	}
}
