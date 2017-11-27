using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter2Trigger : MonoBehaviour {

	//Object to hold other teleporter reference
	public GameObject otherTeleport;
	//Game object to store singlemove prefab in
	public GameObject SingleMove;

	//true if teleporter was just used
	private bool Taken = false;

	//Returns whether or not the previous teleporter was taken
	public bool isTaken() {
		return Taken;
	}

	//Indicates, this telporter was not just taken
	public void resetTaken() {
		Taken = false;
	}

	//void doStop() {
	//	Stop = true;
	//}

	void OnTriggerEnter2D(Collider2D other) {
		//Find the specific teleporter in the level
		otherTeleport = GameObject.FindGameObjectWithTag ("Tele1");

		//Used to check the taken variable of the other teleporter
		Teleporter1Trigger script = (Teleporter1Trigger)otherTeleport.GetComponent (typeof(Teleporter1Trigger));
		if (script.isTaken ()) {
			Debug.Log ("Not going to use this!");
			return;
		}

		//Indicate that this teleporter has been used, set flag
		Taken = true;

		//Love me some Debug statements
		Debug.Log (script.isTaken());
		Debug.Log ("Teleporter entered.");

		//Will need code to determine which boy enter the tele

		//Destroy the current single move boy
		Destroy(other.gameObject);

		//Spawn a new boy at the position of the other teleporter
		Instantiate (SingleMove, otherTeleport.transform.position, Quaternion.identity);
	}

	void OnTriggerExit2D(Collider2D other) {
		//Used to set the taken variable of the other teleporter
		Teleporter1Trigger script = (Teleporter1Trigger)otherTeleport.GetComponent (typeof(Teleporter1Trigger));
		script.resetTaken ();
	}
}
