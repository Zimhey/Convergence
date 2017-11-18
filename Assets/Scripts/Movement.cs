using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { UP, DOWN, LEFT, RIGHT}


public class Movement : MonoBehaviour {

    public Direction direction = Direction.DOWN;
    private int speed;
    private Vector3 pos;
}
