using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour {

	//Game object to store all gate tiles in
	public GameObject[] GateTiles;

	//I know this is a Bad Way to do this, but here, we will store the gate sprites
	public Sprite GateClosed;
	public Sprite GateOpen;

	//Stupid flag
	bool entered = false;

	void OnTriggerEnter2D (Collider2D other)
	{
        if (other.gameObject.GetComponent<Movement>().shaking)
        {
            return;
        }
		if (!entered) {
			Debug.Log ("Character has entered switch trigger.");

            gameObject.GetComponent<AudioSource>().Play();

			//stores current gate tile objects
			GateTiles = GameObject.FindGameObjectsWithTag ("Gate");

			//For each gate tile, either change it's layer to Default (0, no collisions) or Blocking layer (8, collisions)
			foreach (GameObject Gate in GateTiles) {
				SpriteRenderer sr = Gate.GetComponent<SpriteRenderer> ();
				Debug.Log (Gate);
				//If the gate is closed, open it
				if (Gate.layer == 8) {
					Debug.Log ("Gate opened.");
					sr.sprite = GateOpen;
					Gate.layer = 0;

					//If the gate is open, close it
				} else {
					Debug.Log ("Gate closed.");
					Gate.layer = 8;
					sr.sprite = GateClosed;
				}
			}
			entered = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("Character has exited switch trigger.");
		entered = false;
	}
}
