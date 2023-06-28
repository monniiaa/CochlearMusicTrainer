using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{
    protected int currentLevel = 1;
    protected int currentScore;
    protected Difficulty difficulty;

    [SerializeField]
    private GameObject[] modes;

    protected GameData gameData;

    protected string path;
    protected int round = 1;

    [SerializeField]
    protected AudioSource gameplayAudio;
    [SerializeField]
    protected AudioClip sucessAudio;
    [SerializeField]
    protected  AudioClip failAudio;
    [SerializeField]
    protected Material sucessMaterial;
    [SerializeField]
    protected Material failMaterial;
    public bool LevelAlreadyCompleted = false;
    
    public GameObject[] starAnimation = new GameObject[4];

    private void Start()
    {
        SetMode();
    }

    protected void SetMode()
    {
        if (currentLevel <= 3)
        {
            difficulty = Difficulty.Easy;
            modes[0].SetActive(true);
            modes[1].SetActive(false);
            modes[2].SetActive(false);
        }
        else if (currentLevel > 3 && currentLevel <= 6)
        {
            difficulty = Difficulty.Medium;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        }
        else if (currentLevel > 6)
        {
            difficulty = Difficulty.Hard;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        }
    }
    
    protected void ShowStar(int score)
    {
        for (int i = 0; i < starAnimation.Length; i++)
        {
            if(i == score) 
            {
                starAnimation[i].SetActive(true);
                continue;
            }
            starAnimation[i].SetActive(false);
        }
    }


    protected abstract void SetDifficultyChanges();

    public abstract void SetRoundFunctionality();

    protected abstract void EndRound();
    protected abstract void StartRound();

    protected abstract void RestartLevel();
}
