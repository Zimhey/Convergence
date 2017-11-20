using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserInterfaceScreens
{
    MainMenu,
    LevelSelect,
    Pause,
    None
}

public class UIManager : MonoBehaviour
{
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

    private void setPanel(UserInterfaceScreens target, bool active)
    {
        if (target == UserInterfaceScreens.MainMenu)
            MainMenuPanel.SetActive(active);
        else if (target == UserInterfaceScreens.LevelSelect)
            LevelSelectPanel.SetActive(active);
        else if (target == UserInterfaceScreens.Pause)
            PausePanel.SetActive(active);
    }

    public GameObject MainMenuPanel;
    public GameObject LevelSelectPanel;
    public GameObject LevelDropDown;
    public GameObject PausePanel;
    public GameManager GM;

    private int levelIndex;

    // Use this for initialization
    void Start () {
         // Add Levels
        UnityEngine.UI.Dropdown dropDownBox = LevelDropDown.GetComponent<UnityEngine.UI.Dropdown>();
        dropDownBox.ClearOptions();
        dropDownBox.AddOptions(GM.GetLevelList());
        dropDownBox.RefreshShownValue();
        levelIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMainMenu()
    {
        Screen = UserInterfaceScreens.MainMenu;
    }

    public void ShowLevelSelect()
    {
        Screen = UserInterfaceScreens.LevelSelect;
    }

    public void setLevel(int index)
    {
        levelIndex = index;
    }
    public void PlayClicked()
    {
        
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
}
