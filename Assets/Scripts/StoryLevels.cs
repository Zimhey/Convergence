using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryLevels : MonoBehaviour
{

    private List<string> levels;
    public List<string> Levels
    {
        get
        {
            if (levels == null)
                InitLevelList();
            return levels;
        }
    }

    private int unlocked;
    public int DebugUnlocked;
    public int Unlocked
    {
        get
        {
            return unlocked;
        }
        set
        {
            if (value <= Levels.Count)
                unlocked = value;
        }
    }

    public List<string> GetAvailableLevels()
    {
        List<string> l = new List<string>();
        for (int i = 0; i < Unlocked; i++)
            l.Add(Levels[i]);
        return l;
    }

    public void BeatLevel(int i)
    {
        if (Unlocked == i)
            Unlocked++;
    }

    public bool IsLevelUnlocked(int i)
    {
        return i < Unlocked;
    }


    private void InitLevelList()
    {
        levels = new List<string>();
        levels.Add("Level A");
        levels.Add("Level B");
        levels.Add("Level C");
        // TODO add all levels
    }

}
