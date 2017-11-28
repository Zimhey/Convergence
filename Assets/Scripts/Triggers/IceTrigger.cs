using System.Collections;
using UnityEngine;

public class IceTrigger : MonoBehaviour {

	//Game object to store character in
	public GameObject Character;

	//Function that modifies the movement of the character
	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Character has entered Ice trigger.");

        //stores current character object
        Character = other.gameObject;
		Debug.Log (Character);

		if (Character.tag == "SingleMove") {

			//stores the script associated with single move boi to be able to call move functions from it
			SingleMovePlayer script = (SingleMovePlayer)Character.GetComponent (typeof(SingleMovePlayer));

			//Retrieve the last direction inputted by the player
			int xDir = script.getLastHoriz ();
            Debug.Log(xDir+ " is x");
			int yDir = script.getLastVert ();
            Debug.Log(yDir + " is y");


			//Move the character in the last specified direction
			script.AttemptMove (xDir, yDir);
		}
	}
}
