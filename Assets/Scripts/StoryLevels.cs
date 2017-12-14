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

    public int Unlocked
    {
        get
        {
            return Mathf.Max(1, PlayerPrefs.GetInt("Unlocked"));
        }
        set
        {
            if (value <= Levels.Count)
                PlayerPrefs.SetInt("Unlocked", value);
        }
    }

    public List<string> StoryBeats;
    public UnityEngine.UI.Text LoreText;

    public bool ResetProgress;

    public void Start()
    {
        if (ResetProgress)
            Unlocked = 1;
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
        if (Unlocked == i + 1)
            Unlocked++;
        if(StoryBeats.Count > i)
            LoreText.text = StoryBeats[i];
    }

    public bool IsLevelUnlocked(int i)
    {
        return i < Unlocked;
    }


    private void InitLevelList()
    {
        levels = new List<string>();
        StoryBeats = new List<string>();
        levels.Add("Level A");
        StoryBeats.Add("Tainted are the words of the doubtful.");
        levels.Add("Level B");
        StoryBeats.Add("These bricks are golden like Him.");
        levels.Add("Level C");
        StoryBeats.Add("We suffer only from laziness. We must operate under agency. We must love aggressively.");
        levels.Add("Level D");
        StoryBeats.Add("She left an offering at the temple door. The basket was warped and faded. Doesn't she understand that this will not appease Him?");
        levels.Add("Level E");
        StoryBeats.Add("My father won't allow me to visit the river. I want to dig my toes into the sand and feel the current batter my skin. Instead I am stuck in a temple, counting beads.");
        levels.Add("Level F");
        StoryBeats.Add("I feel the life in my breast dimming, like a used torch. I trust that I will feel the warmth of the sun on my forehead again.");
        levels.Add("Level FG");
        StoryBeats.Add("I told him to trust me. I told him that it did not matter. I told him that we would arise and touch lips. Eventually.");
        levels.Add("Level G");
        StoryBeats.Add("They are all insects. They take my feet in their hands and dig their thumbs into my arches. When I return I will not remember their names.");
        levels.Add("Level H");
        StoryBeats.Add("I am a King with open hands and eyes. I will bring your suffering with me.");
        levels.Add("Level I");
        StoryBeats.Add("They are not building fast enough. I want this monument to scrape the clouds and write my name in the stars.");
        levels.Add("Level J");
        StoryBeats.Add("There is weakness in sadness. My final words will not come for thousands of years.");
        levels.Add("Level K");
        StoryBeats.Add("She was so soft. As if my hands chiseled our fate into her shoulder blades. She would not come with me. I understood.");
        levels.Add("Level KL");
        StoryBeats.Add("I think that they are plotting against me. It is futile. I will only return furious.");
        levels.Add("Level L");
        StoryBeats.Add("I will disappoint my people. I am sick. My head is sick.");
        levels.Add("Level M");
        StoryBeats.Add("Today is the day that I visit the temple. I must commune with my Beast.");
        levels.Add("Level N");
        StoryBeats.Add("He who rules the desert growls as we submit to the underworld.");
        levels.Add("Level O");
        StoryBeats.Add("These scriptures tell me that I will breathe again. My soul will be carried on the backs of my people.");
        levels.Add("Level P");
        StoryBeats.Add("The Beast, east facing, casts his gaze on us all. Fear him. Love him. Worship him.");
        levels.Add("Level Q");
        StoryBeats.Add("This Beast will not eclipse my greatness. Even if the prophecy states he will devour the world.");
        levels.Add("Level R");
        StoryBeats.Add("I shudder to think what he might crush beneath His heavy paws.");
        levels.Add("Level Z");
        StoryBeats.Add("Here He comes.");
    }

}
