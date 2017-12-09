using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMovePlayer : Movement {

    //two variables to store the last inputed direction of the player
    protected override void Start()
    {
        FindCorner();
        base.Start();
    }

    //two variables to store the last inputed direction of the player
    public override void FindCorner()
    {
        Vector3 bottomRightScreen = new Vector3(Screen.width - 1, 1, 0);
        bottomRightScreen = GameObject.FindObjectOfType<Camera>().ScreenToWorldPoint(bottomRightScreen);
        bottomRightScreen = new Vector3((int)bottomRightScreen.x, (int)bottomRightScreen.y);

        corner = bottomRightScreen;
        Debug.Log("corner x: " + corner.x + " corner y: " + corner.y);

    }

    public override void AttemptMove(int xDir, int yDir)
    {
        shaking = false;
        //as long as there is player input for direction, store it
        if (xDir != 0 || yDir != 0)
        {
            lastHoriz = xDir;
            lastVert = yDir;
        }

        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        
        bool canMove = Move(xDir, yDir, out hit);

        if (!canMove)
        {
            OnCantMove();
            moving = true;
        }

       /* if (canMove)
        {
            AttemptMove(xDir, yDir);
        }
        */
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
        if(!IsWithin((int)end.x, 0, GameManager.Instance.BM.Board.Rows - 1) || !IsWithin((int)end.y, 0, GameManager.Instance.BM.Board.Columns - 1)){
            
            return false;
        }
        else if (hit.transform == null)
        {
            while (hit.transform == null)
            {
                if (lastHoriz != 0)
                {
                    xTrans += xDir;
                    end = start + new Vector2(xTrans, yTrans);
                }
                else if (lastVert != 0)
                {
                    yTrans += yDir;
                    end = start + new Vector2(xTrans, yTrans);
                }
                hit = Physics2D.Linecast(start, end, BlockingLayer);
                if (!IsWithin((int)end.x, 0, GameManager.Instance.BM.Board.Rows-1) || !IsWithin((int)end.y, 0, GameManager.Instance.BM.Board.Columns-1))
                {
                    break;
                }
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

    public static bool IsWithin(int val, int min, int max)
    {
        return (min <= val && val <= max);
    }

    

  
}
