using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMovePlayer : Movement {

    protected override void Start()
    {
        base.Start();
    }

    //two variables to store the last inputed direction of the player
    public override void FindCorner(Vector3 goalLocation)
    {
        Vector3 offset = new Vector3(0.5f, 0.5f);
        Vector3 upperRightGoal = goalLocation + offset;
        corner = upperRightGoal;

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
