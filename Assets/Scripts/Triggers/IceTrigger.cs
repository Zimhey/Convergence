using System.Collections;
using UnityEngine;

public class IceTrigger : MonoBehaviour {

	//Game object to store character in
	public GameObject Character;
    

	//Function that modifies the movement of the character
	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Character has entered Ice trigger.");
        if (other.gameObject.GetComponent<Movement>().shaking)
        {
            return;
        }
        //stores current character object
        Character = other.gameObject;
		Debug.Log (Character);
        AudioSource audio = Character.GetComponent<AudioSource>();
        AudioClip clip = Character.GetComponent<Movement>().slide;
        if (!audio.clip == clip || !audio.isPlaying)
        {
            audio.clip = clip;
            audio.Play();
        }

		if (Character.tag == "SingleMove") {

			//stores the script associated with single move boi to be able to call move functions from it
			SingleMovePlayer script = Character.GetComponent<SingleMovePlayer>();

			//Retrieve the last direction inputted by the player
			int xDir = script.getLastHoriz ();
            Debug.Log(xDir+ " is x");
			int yDir = script.getLastVert ();
            Debug.Log(yDir + " is y");

            Vector3 endAdjust = new Vector2(xDir, yDir);

            if(Physics2D.Linecast(gameObject.transform.position, gameObject.transform.position + endAdjust, script.BlockingLayer).transform == null)
            {
                script.moveQueue.Enqueue(gameObject.transform.position + endAdjust);
            }
			//Move the character in the last specified direction
			//script.AttemptMove (xDir, yDir);
		}
        else if(Character.tag == "DoubleMove")
        {
            DoubleMovePlayer script = Character.GetComponent<DoubleMovePlayer>();

            int xDir = script.getLastHoriz();
            Debug.Log(xDir + " is x");
            int yDir = script.getLastVert();
            Debug.Log(yDir + " is y");

            Vector3 endAdjust = new Vector2(xDir, yDir);

            if (Physics2D.Linecast(gameObject.transform.position, gameObject.transform.position + endAdjust, script.BlockingLayer).transform == null)
            {
                script.moveQueue.Enqueue(gameObject.transform.position + endAdjust);
            }
        }
        else if(Character.tag == "InverseMove")
        {
            InverseMovePlayer script = Character.GetComponent<InverseMovePlayer>();

            int xDir = script.getLastHoriz();
            Debug.Log(xDir + " is x");
            int yDir = script.getLastVert();
            Debug.Log(yDir + " is y");

            Vector3 endAdjust = new Vector2(xDir, yDir);

            if (Physics2D.Linecast(gameObject.transform.position, gameObject.transform.position + endAdjust, script.BlockingLayer).transform == null)
            {
                script.moveQueue.Enqueue(gameObject.transform.position + endAdjust);
            }
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {
        AudioSource audio = other.gameObject.GetComponent<AudioSource>();
        audio.clip = other.gameObject.GetComponent<Movement>().move;
        audio.Play();
        
    }
}
