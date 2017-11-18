using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMove : Character {

    private void Start()
    {
        pos = gameObject.GetComponent<RectTransform>().localPosition;
    }

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
    public override void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:
                {
                    pos.y += 51.2f;
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    break;
                }
            case Direction.DOWN:
                {
                    pos.y -= 51.2f;
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    break;
                }
            case Direction.LEFT:
                {
                    pos.x -= 51.2f;
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    break;
                }
            case Direction.RIGHT:
                {
                    pos.x += 51.2f;
                    gameObject.GetComponent<RectTransform>().localPosition = pos;
                    break;
                }

        }
    }
}
