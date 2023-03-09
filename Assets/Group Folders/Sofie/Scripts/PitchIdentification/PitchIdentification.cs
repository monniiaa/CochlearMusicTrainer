using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchIdentification : MonoBehaviour
{
    public DataManager dataManager;
    public UIMenu pitchModeMenu;
    private int maxLevel = 4;
    private int currentLevel = 1;
    private int currentScore = 0;
    private GameData data;
    private string path = "PitchIdentification";

    private Difficulty difficulty;
    private GameObject[] modes;
    private int pitchDifference = 0;

    private void Start()
    {
        data = dataManager.CreateGameData(maxLevel, currentScore, currentLevel);
        dataManager.SaveDataToJson(data, path);
        pitchModeMenu.ActivateLevel(data.level);
        pitchModeMenu.ShowLevelInfo(data.level, data.levelScore[data.level]);
    }
    
    private void SetMode()
    {
        if(currentLevel <= 2)
        {
            difficulty = Difficulty.Easy; 
            modes[0].SetActive(true);
            modes[1].SetActive(false);
            modes[2].SetActive(false);
        }
        else if(currentLevel >2 && currentLevel <= 3)
        {
            difficulty = Difficulty.Medium;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        } else if(currentLevel > 3)
        {
            difficulty = Difficulty.Hard;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        }
    }

    private void SetPitchDifference()
    {
        if(difficulty == Difficulty.Easy)
        {
            Speaker[] speakers = GameObject.FindObjectsOfType<Speaker>();
            Speaker highestSpeaker = null;
            for (int i = 0; i < speakers.Length; i++)
            {
                speakers[i].pitch = (Random.Range(1, 5));
                for(int j = i + 1; j< speakers.Length;)
                {
                     while(speakers[i].pitch  == speakers[j].pitch) { }
                }
                if (speakers[i].pitch > highestSpeaker.pitch)
                {
                    highestSpeaker = speakers[i];
                }
                
            }

        } 
    }

    enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

}
