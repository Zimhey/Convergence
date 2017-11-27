using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 	Start,
 	Goal,
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
	Pit,
    Wall,
	Teleport1,
	Teleport2,
	Gate,
	Switch,
	Slide,
	Ice,
	Start,
	Goal
}

public class BoardManager : MonoBehaviour
{
    public int Rows;
    public int Columns;

    public TileTypes[][] Tiles;

    public GameObject BoardObject;
    public GameObject[] GroundTiles;
    public GameObject[] WallTiles;
	public GameObject[] IceTiles;
	public GameObject[] Teleport1Tiles;
	public GameObject[] Teleport2Tiles;
	public GameObject[] GateTiles;
	public GameObject[] SwitchTiles;
	public GameObject[] PitTiles;
	public GameObject[] StartTiles;
	public GameObject[] GoalTiles;

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
			case TileTypes.Ice:
				return IceTiles[Random.Range(0, IceTiles.Length)];
			case TileTypes.Pit:
				return PitTiles[Random.Range(0, PitTiles.Length)];
			case TileTypes.Teleport1:
				return Teleport1Tiles[Random.Range(0, Teleport1Tiles.Length)];
			case TileTypes.Teleport2:
				return Teleport2Tiles[Random.Range(0, Teleport2Tiles.Length)];
			case TileTypes.Gate:
				return GateTiles[Random.Range(0, GateTiles.Length)];
			case TileTypes.Switch:
				return SwitchTiles[Random.Range(0, SwitchTiles.Length)];
			case TileTypes.Start:
				return StartTiles[Random.Range(0, StartTiles.Length)];
			case TileTypes.Goal:
				return GoalTiles[Random.RandomRange(0, GoalTiles.Length)];
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
                return TileTypes.Ice;
			case TileTypes.Ice:
				return TileTypes.Pit;
			case TileTypes.Pit:
				return TileTypes.Teleport1;
			case TileTypes.Teleport1:
				return TileTypes.Teleport2;
			case TileTypes.Teleport2:
				return TileTypes.Gate;
			case TileTypes.Gate:
				return TileTypes.Switch;
			case TileTypes.Switch:
				return TileTypes.Start;
			case TileTypes.Start:
				return TileTypes.Goal;
			case TileTypes.Goal:
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
