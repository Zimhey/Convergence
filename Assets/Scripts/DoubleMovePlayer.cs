using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMovePlayer : Movement {

    //two variables to store the last inputed direction of the player
    protected override void Start()
    {
        
        base.Start();
    }

    //two variables to store the last inputed direction of the player
    public override void FindCorner(Vector3 goalLocation)
    {
        Vector3 offset = new Vector3(-0.5f, 0.5f);
        Vector3 upperLeftGoal = goalLocation + offset;
        corner = upperLeftGoal;

        Debug.Log("corner x: " + corner.x + " corner y: " + corner.y);

    }



    public override void AttemptMove(int xDir, int yDir)
    {

        //as long as there is player input for direction, store it
        if (xDir != 0 || yDir != 0)
        {
            lastHoriz = xDir;
            lastVert = yDir;
        }

        base.AttemptMove(2*xDir, 2*yDir);

        /*if (!moving) { 
          Move(xDir, yDir, out hit);
        }*/
    }


   
}
