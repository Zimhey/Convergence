using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public enum TileTypes
{
    Ground,
	Pit,
    Wall,
	Teleport1,
	Teleport2,
    Teleport3,
    Teleport4,
	Gate,
	Switch,
	Slide,
	Ice,
	Start1,
    Start2,
    Start3,
    Start4,
    StartSlide,
	Goal,
    ErrorTile
}

public class InvalidTileException : System.Exception
{
    public InvalidTileException(string message) : base(message)
    {
    }
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

    [HideInInspector]
    public GameObject BoardObject;

    // Tiles
    public GameObject[] GroundTiles;
    public GameObject[] WallTiles;
	public GameObject[] IceTiles;
	public GameObject[] Teleport1Tiles;
	public GameObject[] Teleport2Tiles;
    public GameObject[] Teleport3Tiles;
    public GameObject[] Teleport4Tiles;
    public GameObject[] GateTiles;
	public GameObject[] SwitchTiles;
	public GameObject[] PitTiles;
	public GameObject[] Start1Tiles;
    public GameObject[] Start2Tiles;
    public GameObject[] Start3Tiles;
    public GameObject[] Start4Tiles;
    public GameObject[] GoalTiles;
    public GameObject ErrorTile;

    public GameObject Start1Player;
    public GameObject Start2Player;
    public GameObject Start3Player;
    public GameObject Start4Player;


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
        RemoveBoard();
        BoardObject = new GameObject("Board");
        for(int i = 0; i < Board.Rows; i++)
        {
            for(int j = 0; j < Board.Columns; j++)
            {
                GameObject obj;
                try
                {
                    obj = Instantiate(RandomTile(Board.Tiles[i][j]));

                }
                catch(InvalidTileException)
                {
                    Board.Tiles[i][j] = TileTypes.ErrorTile;
                    obj = Instantiate(RandomTile(Board.Tiles[i][j]));
                }
                obj.transform.parent = BoardObject.transform;
                obj.transform.position = new Vector3(i, j);

            }
        }
    }

    public List<Movement> SpawnPlayers()
    {
        List<Movement> players = new List<Movement>();

        for (int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Columns; j++)
            {
                switch(Board.Tiles[i][j])
                {
                    case TileTypes.Start1:
                        players.Add(SpawnPlayer(i, j, Start1Player));
                        break;
                    case TileTypes.Start2:
                        players.Add(SpawnPlayer(i, j, Start2Player));
                        break;
                    case TileTypes.Start3:
                        players.Add(SpawnPlayer(i, j, Start3Player));
                        break;
                    case TileTypes.Start4:
                        players.Add(SpawnPlayer(i, j, Start4Player));
                        break;
                }
            }
        }
        return players;
    }

    public Movement SpawnPlayer(int i, int j, GameObject prefab)
    {
        Vector3 pos = new Vector3(i, j, 0);
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        return obj.GetComponent<Movement>();
    }

    private GameObject RandomTile(TileTypes type)
    {
        switch(type)
        {
            case TileTypes.Ground:
                return ArrayToRandomTile(GroundTiles);
            case TileTypes.Wall:
                return ArrayToRandomTile(WallTiles);
            case TileTypes.Ice:
				return ArrayToRandomTile(IceTiles);
            case TileTypes.Pit:
				return ArrayToRandomTile(PitTiles);
            case TileTypes.Teleport1:
				return ArrayToRandomTile(Teleport1Tiles);
            case TileTypes.Teleport2:
				return ArrayToRandomTile(Teleport2Tiles);
            case TileTypes.Teleport3:
                return ArrayToRandomTile(Teleport3Tiles);
            case TileTypes.Teleport4:
                return ArrayToRandomTile(Teleport4Tiles);
            case TileTypes.Gate:
                return ArrayToRandomTile(GateTiles);
            case TileTypes.Switch:
                return ArrayToRandomTile(SwitchTiles);
            case TileTypes.Start1:
                return ArrayToRandomTile(Start1Tiles);
            case TileTypes.Start2:
                return ArrayToRandomTile(Start2Tiles);
            case TileTypes.Start3:
                return ArrayToRandomTile(Start3Tiles);
            case TileTypes.Start4:
                return ArrayToRandomTile(Start4Tiles);
            case TileTypes.Goal:
                return ArrayToRandomTile(GoalTiles);
            case TileTypes.ErrorTile:
                return ErrorTile;
            default:
                throw new InvalidTileException("Invalid Tile: " + type);
        }
    }

    public GameObject ArrayToRandomTile(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)];
    } 

    public void RemoveBoard()
    {
        if (BoardObject != null)
            Destroy(BoardObject);
    }

    public void ClickedMapPoint(Vector2 point, bool left)
    {
        try
        {
            int x = Mathf.RoundToInt(point.x);
            int y = Mathf.RoundToInt(point.y);
            if(left)
                Board.Tiles[x][y] = NextTileType(Board.Tiles[x][y]);
            else
                Board.Tiles[x][y] = AlternateTileType(Board.Tiles[x][y]);
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
            case TileTypes.Teleport2:
            case TileTypes.Teleport3:
            case TileTypes.Teleport4:
                return TileTypes.Gate;
			case TileTypes.Gate:
				return TileTypes.Switch;
			case TileTypes.Switch:
				return TileTypes.Start1;
			case TileTypes.Start1:
            case TileTypes.Start2:
            case TileTypes.Start3:
            case TileTypes.Start4:
                return TileTypes.Goal;
			case TileTypes.Goal:
				return TileTypes.Ground;
            case TileTypes.ErrorTile:
                return TileTypes.Ground;
            default:
                return type;
        }
    }

    public TileTypes AlternateTileType(TileTypes type)
    {
        switch(type)
        {
            // Starts
            case TileTypes.Start1:
                return TileTypes.Start2;
            case TileTypes.Start2:
                return TileTypes.Start3;
            case TileTypes.Start3:
                return TileTypes.Start4;
            case TileTypes.Start4:
                return TileTypes.Start1;
            // Teleports
            case TileTypes.Teleport1:
                return TileTypes.Teleport2;
            case TileTypes.Teleport2:
                return TileTypes.Teleport3;
            case TileTypes.Teleport3:
                return TileTypes.Teleport4;
            case TileTypes.Teleport4:
                return TileTypes.Teleport1;
            default:
                return type;
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
        if(GameManager.Instance.State == GameState.LevelBuilding)
        {
            if (Input.GetMouseButtonDown(0))
                ClickedMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
            if (Input.GetMouseButtonDown(1))
                ClickedMapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
            // TODO Right click Level Builder
        }

    }
}
