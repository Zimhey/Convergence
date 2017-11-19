using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Ground,
    Pit,
    Wall,
    Teleport,
    Gate,
    Switch,
    Slide,
    Ice,
    Rotator
    */
public enum TileTypes
{
    Ground,
    Wall
}

public class BoardManager : MonoBehaviour
{
    public int Rows;
    public int Columns;

    public TileTypes[][] Tiles;

    public GameObject BoardObject;
    public GameObject[] GroundTiles;
    public GameObject[] WallTiles;
    public GameObject ErrorTile;

    private void Resize()
    {
        Tiles = new TileTypes[Rows][];

        for(int i = 0; i < Columns; i++)
        {
            Tiles[i] = new TileTypes[Columns];
        }
    }

    public void SpawnBoard()
    {
        BoardObject = new GameObject("Board");
        for(int i = 0; i < Rows; i++)
        {
            for(int j = 0; j < Columns; j++)
            {
                GameObject obj = Instantiate(RandomTile(Tiles[i][j]));
                obj.transform.parent = BoardObject.transform;
                obj.transform.position = new Vector3(i, j);
            }
        }
    }

    private GameObject RandomTile(TileTypes type)
    {
        switch(type)
        {
            case TileTypes.Ground:
                return GroundTiles[Random.Range(0, GroundTiles.Length)];
            case TileTypes.Wall:
                return WallTiles[Random.Range(0, WallTiles.Length)];
            default:
                return ErrorTile;
        }
    }

    public void RemoveBoard()
    {
        Destroy(BoardObject);
    }

    public void ClickedMapPoint(Vector2 point)
    {
        // TODO translate into array indices
        int x = Mathf.RoundToInt(point.x);
        int y = Mathf.RoundToInt(point.y);
        Debug.Log("Clicked: " + x + " " + y);
        Tiles[x][y] = NextTileType(Tiles[x][y]);
        RemoveBoard();
        SpawnBoard();
    }

    public TileTypes NextTileType(TileTypes type)
    {
        switch(type)
        {
            case TileTypes.Ground:
                return TileTypes.Wall;
            case TileTypes.Wall:
                return TileTypes.Ground;
            default:
                return TileTypes.Ground;
        }
    }

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        Resize();
        SpawnBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ClickedMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
