using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMovePlayer : Movement {

    protected override void Start()
    {
        FindCorner();
        base.Start();
    }

    //two variables to store the last inputed direction of the player
    public override void FindCorner()
    {
        Vector3 upperRightScreen = new Vector3(Screen.width - 1, Screen.height -1, 0);
        upperRightScreen = GameObject.FindObjectOfType<Camera>().ScreenToWorldPoint(upperRightScreen);
        upperRightScreen = new Vector3((int)upperRightScreen.x, (int)upperRightScreen.y);

        corner = upperRightScreen;
        Debug.Log("corner x: " + corner.x + " corner y: " + corner.y);
        
    }

    


    public override void AttemptMove(int xDir, int yDir)
    {

		//as long as there is player input for direction, store it
		if (xDir != 0 || yDir != 0) {
			lastHoriz = xDir;
			lastVert = yDir;
		}

        base.AttemptMove(xDir, yDir);

        /*if (!moving) { 
          Move(xDir, yDir, out hit);
        }*/
    }


   

}
