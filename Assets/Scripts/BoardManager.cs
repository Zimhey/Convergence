using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
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
	Start1,
	Goal
}

[System.Serializable]
public struct BoardInfo
{
    public int Rows;
    public int Columns;

    public TileTypes[][] Tiles;
}

public class BoardManager : MonoBehaviour
{
    public BoardInfo Board;

    public GameObject BoardObject;
    public GameObject[] GroundTiles;
    public GameObject[] WallTiles;
	public GameObject[] IceTiles;
	public GameObject[] Teleport1Tiles;
	public GameObject[] Teleport2Tiles;
	public GameObject[] GateTiles;
	public GameObject[] SwitchTiles;
	public GameObject[] PitTiles;
	public GameObject[] Start1Tiles;
	public GameObject[] GoalTiles;

    public GameObject ErrorTile;

    private void Resize()
    {
        Board.Tiles = new TileTypes[Board.Rows][];

        for(int i = 0; i < Board.Columns; i++)
        {
            Board.Tiles[i] = new TileTypes[Board.Columns];
        }
    }

    public void SaveBoard(string name)
    {
        XmlSerializer ser = new XmlSerializer(typeof(BoardInfo));

        using (StreamWriter writer = new StreamWriter(name))
        {
            ser.Serialize(writer, Board);
        }
    }

    public void LoadBoard(string name)
    {
        XmlSerializer ser = new XmlSerializer(typeof(BoardInfo));
        FileStream fs = new FileStream(name, FileMode.Open);
        XmlReader reader = XmlReader.Create(fs);

        Board = (BoardInfo)ser.Deserialize(reader);
        RemoveBoard();
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        BoardObject = new GameObject("Board");
        for(int i = 0; i < Board.Rows; i++)
        {
            for(int j = 0; j < Board.Columns; j++)
            {
                GameObject obj = Instantiate(RandomTile(Board.Tiles[i][j]));
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
			case TileTypes.Start1:
				return Start1Tiles[Random.Range(0, Start1Tiles.Length)];
			case TileTypes.Goal:
				return GoalTiles[Random.Range(0, GoalTiles.Length)];
			default:
		        return ErrorTile;
        }
    }

    public void RemoveBoard()
    {
        if (BoardObject != null)
            Destroy(BoardObject);
    }

    public void ClickedMapPoint(Vector2 point)
    {
        try
        {
            int x = Mathf.RoundToInt(point.x);
            int y = Mathf.RoundToInt(point.y);
            Debug.Log("Clicked: " + x + " " + y);
            Board.Tiles[x][y] = NextTileType(Board.Tiles[x][y]);
            RemoveBoard();
            SpawnBoard();
        }
        catch (System.IndexOutOfRangeException) { };

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
				return TileTypes.Start1;
			case TileTypes.Start1:
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
