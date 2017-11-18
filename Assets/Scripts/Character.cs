using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour{

    public enum Direction { UP, DOWN, LEFT, RIGHT};
    public Vector3 pos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.RIGHT);
        }

    }

    abstract public void Move(Direction dir);
}
