using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserInterfaceScreens
{
    MainMenu,
    LevelSelect,
    Pause,
    LevelBuilder,
    None
}

public class UIManager : MonoBehaviour
{
    public GameManager GM;

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
    public GameObject PausePanel;
    public GameObject LevelBuilderPanel;

    private int levelIndex;
    private string levelName;

    // Use this for initialization
    void Start () {
         // Add Levels
        UnityEngine.UI.Dropdown dropDownBox = LevelDropDown.GetComponent<UnityEngine.UI.Dropdown>();
        dropDownBox.ClearOptions();
        dropDownBox.AddOptions(GM.GetLevelList());
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
        if(levelName != null)
            GM.Board.SaveBoard("Levels/" + levelName + ".xml");
    }

    public void LoadLevel()
    {
        if (levelName != null)
            GM.Board.LoadBoard("Levels/" + levelName + ".xml");
    }

    public void SetBuilderLevelName(string name)
    {
        levelName = name;
    }

    public void ShowMainMenu()
    {
        Screen = UserInterfaceScreens.MainMenu;
    }

    public void ShowLevelSelect()
    {
        Screen = UserInterfaceScreens.LevelSelect;
    }

    public void ShowLevelBuilder()
    {
        Screen = UserInterfaceScreens.LevelBuilder;
    }

    public void setLevel(int index)
    {
        levelIndex = index;
    }
    public void PlayClicked()
    {
        GM.LoadLevel(GM.GetLevel(levelIndex));
    }

    public void ShowPause()
    {
        Screen = UserInterfaceScreens.Pause;
    }

    public void Resume()
    {
        HideAll();
    }

    public void HideAll()
    {
        Screen = UserInterfaceScreens.None;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void setPanel(UserInterfaceScreens target, bool active)
    {
        if (target == UserInterfaceScreens.MainMenu)
            MainMenuPanel.SetActive(active);
        else if (target == UserInterfaceScreens.LevelSelect)
            LevelSelectPanel.SetActive(active);
        else if (target == UserInterfaceScreens.Pause)
            PausePanel.SetActive(active);
        else if (target == UserInterfaceScreens.LevelBuilder)
            LevelBuilderPanel.SetActive(active);
    }
}
