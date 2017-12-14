using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserInterfaceScreens
{
    MainMenu,
    LevelSelect,
    CustomLevelSelect,
    Loading,
    Pause,
    Win,
    Lore,
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
    public GameObject PlayPanel;
    public UnityEngine.UI.Dropdown PlayLevelDropDown;
    public GameObject CustomLevelPanel;
    public UnityEngine.UI.Dropdown CustomLevelDropDown;
    public GameObject LoadingPanel;
    public GameObject PausePanel;
    public GameObject WinPanel;
    public GameObject WinCustomPanel;
    public GameObject LosePanel;
    public GameObject LevelBuilderPanel;
    public GameObject LorePanel;

    [HideInInspector]
    public int levelIndex;
    [HideInInspector]
    public UserInterfaceScreens NextScreen;

    private List<string> customLevels;
    private int customLevelIndex;

    // Use this for initialization
    void Start () {
        Screen = StartScreen;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Builder

    private string levelName;

    public void SaveLevel()
    {
        GameManager.Instance.BM.Board.Name = levelName;
        if(levelName != null)
            GameManager.Instance.BM.SaveBoard("Levels/Custom/" + levelName + ".xml");
    }

    public void LoadLevel()
    {
        if (levelName != null)
            GameManager.Instance.BM.LoadBoard("Levels/Custom/" + levelName + ".xml");
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

    public void ShowCustomSelect()
    {
        Screen = UserInterfaceScreens.CustomLevelSelect;
    }

    public void ShowLevelBuilder()
    {
        Screen = UserInterfaceScreens.LevelBuilder;
        GameManager.Instance.State = GameState.LevelBuilding;
        GameManager.Instance.BM.Resize(10, 10);
        GameManager.Instance.BM.ResetBoard();
        GameManager.Instance.BM.SpawnBoard();
    }

    public void SetStoryLevelIndex(int index)
    {
        levelIndex = index;
    }

    public void SetCustomLevelIndex(int index)
    {
        customLevelIndex = index;
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

    public void StoryPlayClicked()
    {
       GameManager.Instance.LoadLevel(GameManager.Instance.Story.Levels[levelIndex]);
    }

    public void CustomPlayClicked()
    {
        GameManager.Instance.LoadCustomLevel(customLevels[customLevelIndex]);
    }

    public void NextLevelClicked()
    {
        if(GameManager.Instance.Story.IsLevelUnlocked(levelIndex + 1))
            GameManager.Instance.LoadLevel(GameManager.Instance.Story.Levels[++levelIndex]);
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

    public void UpdateStoryLevelDropdown()
    {
        // Add Levels
        PlayLevelDropDown.ClearOptions();
        PlayLevelDropDown.AddOptions(GameManager.Instance.Story.GetAvailableLevels());
        PlayLevelDropDown.RefreshShownValue();
    }

    public void UpdateCustomLevelDropdown()
    {
        customLevels = new List<string>();
        foreach (string file in System.IO.Directory.GetFiles("Levels/Custom/"))
        {
            string name = file.Replace("Levels/Custom/", "");
            name = name.Replace(".xml", "");
            customLevels.Add(name);
        }

        // Add Levels
        CustomLevelDropDown.ClearOptions();
        CustomLevelDropDown.AddOptions(customLevels);
        CustomLevelDropDown.RefreshShownValue();
    }

    public void ShowNextScreen()
    {
        Screen = NextScreen;
    }

    private void setPanel(UserInterfaceScreens panel, bool active)
    {
        switch(panel)
        {
            case UserInterfaceScreens.MainMenu:
                MainMenuPanel.SetActive(active);
                break;
            case UserInterfaceScreens.LevelSelect:
                if (active)
                    UpdateStoryLevelDropdown();
                PlayPanel.SetActive(active);
                break;
            case UserInterfaceScreens.CustomLevelSelect:
                if (active)
                    UpdateCustomLevelDropdown();
                CustomLevelPanel.SetActive(active);
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
            case UserInterfaceScreens.Lore:
                LorePanel.SetActive(active);
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
