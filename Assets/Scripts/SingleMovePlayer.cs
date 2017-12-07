using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMovePlayer : Movement {

	//two variables to store the last inputed direction of the player
	int lastHoriz = 0;
	int lastVert = 0;

	//Returns last inputted x direction, will be zero if player inputted up or down
	public int getLastHoriz() 
	{
		return lastHoriz;
	}

	//Returns last inputted y direction, will be zero if player inputted left or right
	public int getLastVert()
	{
		return lastVert;
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
