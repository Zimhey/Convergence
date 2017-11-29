using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter1Trigger : MonoBehaviour {

	//Object to hold other teleporter reference
	public GameObject[] otherTeleports;
    public GameObject otherTeleport;
	//Game object to store singlemove prefab in
	public GameObject player;

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
		otherTeleports = GameObject.FindGameObjectsWithTag(gameObject.tag);

        foreach(GameObject teleport in otherTeleports)
        {
            if(teleport != gameObject)
            {
                otherTeleport = teleport;
            }
        }

        //Used to check the taken variable of the other teleporter
        Teleporter1Trigger script = otherTeleport.GetComponent<Teleporter1Trigger>();
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
        player = other.gameObject;

        if(player.tag == "SingleMove")
        {
            SingleMovePlayer playerScript = player.GetComponent<SingleMovePlayer>();
            int xDir = playerScript.getLastHoriz();
            int yDir = playerScript.getLastVert();
            Vector3 newDestination = otherTeleport.gameObject.transform.position + new Vector3(xDir, yDir);

            playerScript.StopCoroutine(playerScript.coroutine);
            playerScript.gameObject.transform.SetPositionAndRotation(otherTeleport.transform.position, playerScript.gameObject.transform.rotation);
            playerScript.moveQueue.Clear();
            playerScript.moveQueue.Enqueue(newDestination);
            playerScript.StartCoroutine(playerScript.SmoothMovement(playerScript.moveQueue.Dequeue()));
        }

		
	}

	void OnTriggerExit2D(Collider2D other) {
        //Used to set the taken variable of the other teleporter
        Teleporter1Trigger script = otherTeleport.GetComponent<Teleporter1Trigger>();
		script.resetTaken ();
	}
}

