using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMovePlayer : Movement {

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

        if (!moving)
        {
            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int)(Input.GetAxisRaw("Vertical"));
        }

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

        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.

        bool canMove = Move(xDir, yDir, out hit);

        if (canMove)
        {
            AttemptMove(xDir, yDir);
        }
    }

    public override bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        int xTrans = xDir;
        int yTrans = yDir;
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, BlockingLayer);


        //Re-enable boxCollider after linecast
        

        while(hit.transform == null)
        {
            if(lastHoriz != 0)
            {
                xTrans += xDir;
                end = start + new Vector2(xTrans, yTrans);
            }
            else if(lastVert != 0)
            {
                yTrans += yDir;
                end = start + new Vector2(xTrans, yTrans);
            }
            hit = Physics2D.Linecast(start, end, BlockingLayer);
        }

        if (lastHoriz != 0)
        {
            xTrans -= xDir;
            end = start + new Vector2(xTrans, yTrans);
        }
        else if (lastVert != 0)
        {
            yTrans -= yDir;
            end = start + new Vector2(xTrans, yTrans);
        }
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        
        Debug.Log(end.x + " " + end.y);

        //Check if anything was hit
        if (hit.transform == null && !moving)
        {
            moving = true;
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            coroutine = StartCoroutine(SmoothMovement(end));

            //Return true to say that Move was successful
            return true;
        }

        //If something was hit, return false, Move was unsuccesful.
        return false;
    }

    

    protected override void OnCantMove()
    {

    }
}
