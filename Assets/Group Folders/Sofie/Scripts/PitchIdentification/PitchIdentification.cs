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

    private ToneGenerator highestSpeaker;
    private bool correctPick;

    private int currentLevel;
    private int currentScore = 0;

    private int tries = 0;
  

    private void Start()
    {
        data =  DataManager.ReadJson(path);
        currentLevel = data.level;
        SetMode();
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

    public void SetPitchDifference(ToneGenerator[] speakers)
    {

        highestSpeaker = speakers[0];
        for (int i = 0; i < speakers.Length; i++)
        {
            int randFrequency = Random.Range(20,900);
            speakers[i].frequency1 = randFrequency;
            speakers[i].frequency2 = randFrequency;
            for (int j = 0; j < speakers.Length;j++)
            {
                if (j != i)
                {
                    int interval = 200;
                    while ( speakers[i].frequency1 + (interval/2) > speakers[j].frequency1 &&
                        speakers[i].frequency1 -(interval/2) < speakers[j].frequency1 )
                    {
                        int rand= Random.Range(20, 900);
                        speakers[i].frequency1 = rand;
                        speakers[i].frequency2 = rand;
                        Debug.Log("xd");
                    }
                }
            }
            if (speakers[i].frequency1 > highestSpeaker.frequency1)
            {
                highestSpeaker = speakers[i];
            }
        }
    }
    public void SpeakerPicked(ToneGenerator speaker, ToneGenerator[] speakers)
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
                SetPitchDifference(speakers);
                break;
            case 2:
                //TODO:  Cool speaker animation to look like new speakers pop up + indication of correct answer
                SetPitchDifference(speakers);
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
