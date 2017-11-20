using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Loading,
    WaitForInput,
    WaitForMoveComplete,
    Pause
}

public class GameManager : MonoBehaviour
{
    public BoardManager Board;
    private GameState state;
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
                if (value == GameState.MainMenu)
                    ShowMainMenu();
                if (value == GameState.Pause)
                    OnGamePause();

            }
            state = value;
        }
    }

    private bool levelListInitialized = false;
    private Dictionary<string, string> levelList;
    public Dictionary<string, string> LevelList
    {
        get
        {
            if (!levelListInitialized)
                initLevelList();
            return levelList;
        }
    }


    //public List<SingleMovePlayer> characters;

    // Use this for initialization
    void Start()
    {
        State = GameState.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameState.MainMenu:
                break;
            case GameState.Loading:
                break;
            case GameState.WaitForInput:
                break;
            case GameState.WaitForMoveComplete:
                break;
            case GameState.Pause:
                break;
        }
    }

    public void LoadLevel(string level)
    {
        State = GameState.Loading;
        SceneManager.LoadScene(LevelList[level]);
        State = GameState.WaitForInput;
    }


    private void OnGamePause()
    {
        //https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
    }

    private void ShowMainMenu()
    {

    }

    public string GetLevel(int index)
    {
        return GetLevelList()[index];
    }

    public List<string> GetLevelList()
    {
        return new List<string>(LevelList.Keys);
    }

    private void initLevelList()
    {
        levelList = new Dictionary<string, string>();
        levelList.Add("Example Scene", "Scenes/Levels/ExampleScene");
        // TODO add more levels
        levelListInitialized = true;
    }
}
