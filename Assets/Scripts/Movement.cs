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

    // Use this for initialization
    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;

        
    }

    public virtual bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        //boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        

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
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            moving = false;
            
        }
    }

    public virtual void AttemptMove(int xDir, int yDir)
    {
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
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            OnCantMove();
            moving = true;
            
        }
    }

    public void ChangeMovement(Vector3 end)
    {

    }

    public void ShakeThatMovement()
    {
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 negShakePosition = new Vector3(currentPosition.x - 0.1f, currentPosition.y);
        Vector3 posShakePosition = new Vector3(currentPosition.x + 0.1f, currentPosition.y);
        moveQueue.Enqueue(negShakePosition);
        moveQueue.Enqueue(posShakePosition);
        moveQueue.Enqueue(negShakePosition);
        moveQueue.Enqueue(currentPosition);
        StartCoroutine(SmoothMovement(moveQueue.Dequeue()));
    }

    public void OnCantMove()
    {
        ShakeThatMovement();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
