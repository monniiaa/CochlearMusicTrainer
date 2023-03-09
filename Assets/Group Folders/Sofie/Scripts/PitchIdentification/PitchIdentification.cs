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
            pitchDifference = 5;
        } 
    }

    enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

}
