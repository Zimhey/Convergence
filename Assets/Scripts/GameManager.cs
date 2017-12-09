using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    WaitForInput,
    WaitForMoveComplete,
    Pause,
    LevelBuilding
}

[RequireComponent(typeof(BoardManager))]
[RequireComponent(typeof(StoryLevels))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager(); // TODO Make a prefab to instanciate here
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public UIManager UIM;
    [HideInInspector]
    public StoryLevels Story;
    [HideInInspector]
    public BoardManager BM;
    [HideInInspector]
    public GameObject PlayerRoot;
    [HideInInspector]
    public List<Movement> Players;

    private bool customLevel;

    private GameState state;
    private GameState stateBeforePause;
    public GameState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state != value)
            {
            }
            state = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            BM = GetComponent<BoardManager>();
            Story = GetComponent<StoryLevels>();
            Story.Unlocked = Story.Levels.Count;
            State = GameState.Menu;
        }
        else
        {
            Debug.LogWarning("Warning: Attempted to spawn a second GameManager");
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameState.WaitForInput:
                PollForInput();
                PollForPause();
                break;
            case GameState.WaitForMoveComplete:
                PollForTurnEnd();
                break;
        }
    }

    public void LoadLevel(string name)
    {
        customLevel = false;
        BM.LoadBoard("Levels/Story/" + name + ".xml"); // TODO handle Story in load level
        SetupBoard();
        UIM.Screen = UserInterfaceScreens.None;
    }

    public void LoadCustomLevel(string name)
    {
        customLevel = true;
        BM.LoadBoard("Levels/Custom/" + name + ".xml");
        SetupBoard();
        UIM.Screen = UserInterfaceScreens.None;
    }

    public void RetryLevel()
    {
        // clean up
        CleanLevel();
        SetupBoard();
        UIM.Screen = UserInterfaceScreens.None;
    }

    public void SetupBoard()
    {
        // Spawn Board
        BM.SpawnBoard();
        // Spawn Players
        PlayerRoot = new GameObject("Players");
        Players = BM.SpawnPlayers();
        foreach (Movement p in Players)
            p.transform.parent = PlayerRoot.transform;
        // Begin Game
        State = GameState.WaitForInput;
    }

    public void PollForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stateBeforePause = State;
            State = GameState.Pause;
            // TODO pause game https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
            UIM.Screen = UserInterfaceScreens.Pause;
        }
    }

    public void Resume()
    {
        // TODO pause game https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
        State = stateBeforePause;
        UIM.Screen = UserInterfaceScreens.None;
    }

    public void PollForInput()
    {
        int horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        int vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            foreach(Movement p in Players)
                p.AttemptMove(horizontal, vertical);
            State = GameState.WaitForMoveComplete;
        }
    }

    public void PollForTurnEnd()
    {
        // if there is a player still moving exit the method
        foreach (Movement p in Players)
            if (p.moving)
                return;
        State = GameState.WaitForInput;
    }

    public void GoalReached(GameObject player)
    {
        Movement m = player.GetComponent<Movement>();

        Players.Remove(m);

        if (Players.Count == 0)
        {
            CleanLevel();
            State = GameState.Menu;

            if (customLevel || !GameManager.Instance.Story.IsLevelUnlocked(UIM.levelIndex + 1)) // TODO move levelIndex into GM
                UIM.Screen = UserInterfaceScreens.WinCustom;
            else
            {
                UIM.Screen = UserInterfaceScreens.Win;
                Story.BeatLevel(UIM.levelIndex); // TODO test this
            }

        }
    }

    public void PlayerDied(GameObject player)
    {
        if(State == GameState.WaitForMoveComplete)
        {
            CleanLevel();
            State = GameState.Menu;
            UIM.Screen = UserInterfaceScreens.Lose;
        }
    }

    public void CleanLevel()
    {
        // clean up
        if (PlayerRoot != null)
            Destroy(PlayerRoot);
        BM.RemoveBoard();
    }
}
