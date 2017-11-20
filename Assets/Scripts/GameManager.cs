using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameState state;
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

    private void OnGamePause()
    {
        //https://answers.unity.com/questions/904429/pause-and-resume-coroutine-1.html
    }

    private void ShowMainMenu()
    {

    }

    public List<string> GetLevelList()
    {
        List<string> list = new List<string>();
        list.Add("Test");
        list.Add("Ohhhh weow");
        return list;
    }
}
