﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMovePlayer : Movement {

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

    private void Update()
    {
        //If it's not the player's turn, exit the function.
        //if (!GameManager.instance.playersTurn) return;
        //Used to store the horizontal move direction.
        int horizontal = 0;
        //Used to store the vertical move direction.
        int vertical = 0;

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //Check if moving horizontally, if so set vertical to zero.
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {

            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove(horizontal, vertical);
        }
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


    protected override void OnCantMove()
    {

    }
}