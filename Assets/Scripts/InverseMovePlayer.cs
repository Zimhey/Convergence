﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseMovePlayer : Movement {

    // Use this for initialization
    //two variables to store the last inputed direction of the player
    protected override void Start()
    {
        FindCorner();
        base.Start();
    }

    //two variables to store the last inputed direction of the player
    public override void FindCorner()
    {
        Vector3 bottomLeftScreen = new Vector3(1, 1, 0);
        bottomLeftScreen = GameObject.FindObjectOfType<Camera>().ScreenToWorldPoint(bottomLeftScreen);
        bottomLeftScreen = new Vector3((int)bottomLeftScreen.x, (int)bottomLeftScreen.y);

        corner = bottomLeftScreen;
        Debug.Log("corner x: " + corner.x + " corner y: " + corner.y);

    }

    
    
    public override void AttemptMove(int xDir, int yDir)
    {

        //as long as there is player input for direction, store it
        if (xDir != 0 || yDir != 0)
        {
            lastHoriz =  -xDir;
            lastVert = -yDir;
        }

        base.AttemptMove(-xDir, -yDir);

        /*if (!moving) { 
          Move(xDir, yDir, out hit);
        }*/
    }


    
}
