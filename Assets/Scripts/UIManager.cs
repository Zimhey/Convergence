using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserInterfaceScreens
{
    MainMenu,
    LevelSelect,
    Loading,
    Pause,
    Win,
    WinCustom,
    Lose,
    LevelBuilder,
    None
}

public class UIManager : MonoBehaviour
{
    public UserInterfaceScreens StartScreen;

    private UserInterfaceScreens screen;
    public UserInterfaceScreens Screen
    {
        get
        {
            return screen;
        }
        set
        {
            setPanel(screen, false);
            setPanel(value, true);
            screen = value;
        }
    }

    public GameObject MainMenuPanel;
    public GameObject LevelSelectPanel;
    public GameObject LevelDropDown;
    public GameObject LoadingPanel;
    public GameObject PausePanel;
    public GameObject WinPanel;
    public GameObject WinCustomPanel;
    public GameObject LosePanel;
    public GameObject LevelBuilderPanel;

    private int levelIndex;
    private string levelName;

    // Use this for initialization
    void Start () {
         // Add Levels
        UnityEngine.UI.Dropdown dropDownBox = LevelDropDown.GetComponent<UnityEngine.UI.Dropdown>();
        dropDownBox.ClearOptions();
        dropDownBox.AddOptions(GameManager.Instance.GetLevelList());
        dropDownBox.RefreshShownValue(); 
        levelIndex = 0;

        screen = StartScreen;
        setPanel(screen, true);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveLevel()
    {
        GameManager.Instance.BM.Board.Name = levelName;
        if(levelName != null)
            GameManager.Instance.BM.SaveBoard("Levels/" + levelName + ".xml");
    }

    public void LoadLevel()
    {
        if (levelName != null)
            GameManager.Instance.BM.LoadBoard("Levels/" + levelName + ".xml");
    }

    public void SetBuilderLevelName(string name)
    {
        levelName = name;
    }

    public void ShowMainMenu()
    {
        Screen = UserInterfaceScreens.MainMenu;
        GameManager.Instance.State = GameState.Menu;
        GameManager.Instance.CleanLevel();
    }

    public void ShowLevelSelect()
    {
        Screen = UserInterfaceScreens.LevelSelect;
    }

    public void ShowLevelBuilder()
    {
        Screen = UserInterfaceScreens.LevelBuilder;
        GameManager.Instance.State = GameState.LevelBuilding;
        GameManager.Instance.BM.SpawnBoard();
    }

    public void setLevel(int index)
    {
        levelIndex = index;
    }

    public void SetRows(string row)
    {
        int x;
        bool isInt = int.TryParse(row, out x);
        if(isInt)
        {
            GameManager.Instance.BM.Resize(x, GameManager.Instance.BM.Board.Columns);
            GameManager.Instance.BM.SpawnBoard();
        }
    }

    public void SetColumns(string column)
    {
        int x;
        bool isInt = int.TryParse(column, out x);
        if (isInt)
        {
            GameManager.Instance.BM.Resize(GameManager.Instance.BM.Board.Rows, x);
            GameManager.Instance.BM.SpawnBoard();
        }
    }

    public void PlayClicked()
    {
        GameManager.Instance.LoadLevel(GameManager.Instance.GetLevel(levelIndex));
    }

    public void NextLevelClicked()
    {
        levelIndex++;
        levelIndex %= GameManager.Instance.LevelList.Count;
        GameManager.Instance.LoadLevel(GameManager.Instance.GetLevel(levelIndex));
    }

    public void DescriptionChanged(string description)
    {
        GameManager.Instance.BM.Board.Description = description;
    }

    public void RetryClicked()
    {
        GameManager.Instance.RetryLevel();
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void setPanel(UserInterfaceScreens panel, bool active)
    {
        switch(panel)
        {
            case UserInterfaceScreens.MainMenu:
                MainMenuPanel.SetActive(active);
                break;
            case UserInterfaceScreens.LevelSelect:
                LevelSelectPanel.SetActive(active);
                break;
            case UserInterfaceScreens.Loading:
                LoadingPanel.SetActive(active);
                break;
            case UserInterfaceScreens.Pause:
                PausePanel.SetActive(active);
                break;
            case UserInterfaceScreens.Win:
                WinPanel.SetActive(active);
                break;
            case UserInterfaceScreens.WinCustom:
                WinCustomPanel.SetActive(active);
                break;
            case UserInterfaceScreens.Lose:
                LosePanel.SetActive(active);
                break;
            case UserInterfaceScreens.LevelBuilder:
                LevelBuilderPanel.SetActive(active);
                break;
        }
    }
}
