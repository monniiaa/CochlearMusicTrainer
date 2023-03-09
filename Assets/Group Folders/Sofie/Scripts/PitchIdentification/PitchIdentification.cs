using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchIdentification : MonoBehaviour
{

    private int maxLevel = 4;
    private GameData data;
    private string path = "PitchIdentification";

    private Difficulty difficulty;
    [SerializeField]
    private GameObject[] modes;

    private Speaker highestSpeaker;
    private bool correctPick;

    private int currentLevel;
    private int currentScore = 0;

    private int tries = 0;
  

    private void Start()
    {
        data =  DataManager.ReadJson(path);
        currentLevel = data.level;
        SetMode();
        SetPitchDifference();
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
        Speaker[] speakers = GameObject.FindObjectsOfType<Speaker>();
        highestSpeaker = speakers[0];
        for (int i = 0; i < speakers.Length; i++)
        {
            speakers[i].pitch = (Random.Range(1, 5));
            speakers[i].SetPitch();
            for (int j = 0; j < speakers.Length;j++)
            {
                if(j != i)
                {
                    while (speakers[i].pitch == speakers[j].pitch)
                    {
                        speakers[i].pitch = (Random.Range(1, 5));
                        speakers[i].SetPitch();
                    }
                }
            }
            if (speakers[i].pitch > highestSpeaker.pitch)
            {
                highestSpeaker = speakers[i];
            }
        }
    }
    public void SpeakerPicked(Speaker speaker)
    {
        tries++;
        if(speaker == highestSpeaker)
        {
            currentScore += 1;
            data.levelScore[currentLevel-1] = currentScore;
        }
        switch (tries)
        {
            case 1:
                //TODO: Cool speaker animation to look like new speakers pop up + indication of correct answer
                SetPitchDifference();
                break;
            case 2:
                //TODO:  Cool speaker animation to look like new speakers pop up + indication of correct answer
                SetPitchDifference();
                break;
            case 3:
                currentLevel++;
                data.level = currentLevel;
                DataManager.SaveDataToJson(data, path);
                SceneManager.LoadScene("Menu");
                break;

        }
    }

    enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

}
