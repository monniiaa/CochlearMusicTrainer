using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMelodyManager : LevelManager
{
    
    private GameDataManager _gameDataManager;
    private int currentScore;
    [SerializeField] private InstrumentSeparation modeManager;
    
    private MelodyContoller melodyContoller;

    public int level;

    private void Awake()
    {
        melodyContoller = GetComponent<MelodyContoller>();
        gameplayAudio = GetComponent<AudioSource>();
        _gameDataManager = GameDataManager.Instance;
        modeManager = InstrumentSeparation.Instance;
        
        path = "PitchDiscrimination";
        gameData = DataManager.ReadJson(path);
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        level = currentLevel;
        
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        melodyContoller.EndEvent += EndRound;
        melodyContoller.MatchEvent += Match;
        StartRound();       
    }
    
    private void OnDisable()
    {
        melodyContoller.EndEvent -= EndRound;
        melodyContoller.MatchEvent -= Match;
    }

    private void Match(bool match)
    {
        if (match)
        {
            gameplayAudio.PlayOneShot(sucessAudio);
        }
    }


    protected override void SetDifficultyChanges()
    {
        switch (currentLevel)
        {
            case 1:
                melodyContoller.SetDifficulty(8, false, false, true);
                SetRoundFunctionality();
                break;
            case 2:
                melodyContoller.SetDifficulty(8, false, false, false);
                SetRoundFunctionality();
                break;
            case 3:
                melodyContoller.SetDifficulty(8, false, true, true);
                SetRoundFunctionality();
                break;
            case 4:
                melodyContoller.SetDifficulty(8, false, true, false);
                SetRoundFunctionality();
                break;
            case 5: 
                melodyContoller.SetDifficulty(8, true, false, true);
                SetRoundFunctionality();
                break;
            case 6:
                melodyContoller.SetDifficulty(8, true, false, false);
                SetRoundFunctionality();
                break;
            case 7:
                melodyContoller.SetDifficulty(8, true, true, true);
                SetRoundFunctionality();
                break;
            case 8:
                melodyContoller.SetDifficulty(8, true, true, false);
                SetRoundFunctionality();
                break;
            case 9:
                melodyContoller.SetDifficulty(10, true, true, true);
                SetRoundFunctionality();
                break;
            case 10: 
                melodyContoller.SetDifficulty(10, true, true, false);
                SetRoundFunctionality();
                break;
        }
    }

    public override void SetRoundFunctionality()
    {
        melodyContoller.StartGame();
    }

    protected override void EndRound()
    {
        if(currentLevel == gameData.level)
        {
            gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
        }           

        if(gameData.levelScore[currentLevel - 1] < currentScore)
        {
            gameData.levelScore[currentLevel - 1] = currentScore;
        }

        DataManager.SaveDataToJson(gameData, path);
        StartCoroutine(WaitBeforeEnd());

    }

    private IEnumerator WaitBeforeEnd()
    {
        JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Melody Identification", DateTime.Now, currentScore, currentLevel));
        yield return new WaitForSeconds(0.5f);
        currentScore = 3;
        ShowStar(currentScore);
        modeManager.EndGame();
    }

    protected override void StartRound()
    {
        SetDifficultyChanges();
    }

    protected override void RestartLevel()
    {
        currentScore = 0;
    }
}
