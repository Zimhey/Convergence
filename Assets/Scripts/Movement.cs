using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask BlockingLayer;

    public BoxCollider2D boxCollider;
    public Rigidbody2D rb2D;
    public float inverseMoveTime;
    public bool moving = false;
    public Queue<Vector3> moveQueue = new Queue<Vector3>();
    public Coroutine coroutine;
    public int lastHoriz;
    public int lastVert;
    public Vector3 corner;
    public bool shaking;

    // Use this for initialization
    protected virtual void Start()
    {
        lastHoriz = 0;
        lastVert = 0;
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;

        
    }

    public virtual void FindCorner()
    {
        return;
    }

    public virtual bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, BlockingLayer);

        if (!IsWithin((int)end.x, 0, GameManager.Instance.BM.Board.Rows - 1) || !IsWithin((int)end.y, 0, GameManager.Instance.BM.Board.Columns - 1))
        {
            return false;
        }

        //Re-enable boxCollider after linecast
        // boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null  && !moving)
        {
            moving = true;
            moveQueue.Enqueue(end);
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            coroutine = StartCoroutine(SmoothMovement(moveQueue.Dequeue()));
            
            //Return true to say that Move was successful
            return true;
        }
        
        //If something was hit, return false, Move was unsuccesful.
        return false;
    }


    public IEnumerator SmoothMovement(Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        if (moveQueue.Count != 0)
        {
            
            coroutine = StartCoroutine(SmoothMovement(moveQueue.Dequeue()));
        }
        else
        {
            moving = false;
            if(end == corner)
            {
                GameManager.Instance.GoalReached(gameObject);
            }
        }
    }

    public virtual void AttemptMove(int xDir, int yDir)
    {
        shaking = false;
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.

        bool canMove = Move(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
		if (hit.transform == null) { 
			//If nothing was hit, return and don't execute further code.
			return;
			/*
		if (hit.collider){
			OnTriggerEnter2D (hit.collider, xDir, yDir);
		}
		*/
		}

        if (!canMove)
        {
            moving = true;
            OnCantMove();
        }
    }

    public void ChangeMovement(Vector3 end)
    {

    }

    public void ShakeThatMovement(Vector3 adjustVector)
    {
        shaking = true;
        Vector3 currentPosition = gameObject.transform.position;
        moveQueue.Enqueue(currentPosition - adjustVector);
        moveQueue.Enqueue(currentPosition + adjustVector);
        moveQueue.Enqueue(currentPosition - adjustVector);
        moveQueue.Enqueue(currentPosition);
        StartCoroutine(SmoothMovement(moveQueue.Dequeue()));
    }

    public void OnCantMove()
    {
        
        if(lastHoriz != 0)
        {
            Vector3 adjust = new Vector3(0.1f, 0);
            ShakeThatMovement(adjust);
        }
        else if(lastVert != 0)
        {
            Vector3 adjust = new Vector3(0, 0.1f);
            ShakeThatMovement(adjust);
        }
        
    }

    public void GoalReached()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        moveQueue.Enqueue(corner);
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public static bool IsWithin(int val, int min, int max)
    {
        return (min <= val && val <= max);
    }
}
