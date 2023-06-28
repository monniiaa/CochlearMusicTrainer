using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimbreManager : LevelManager
{
    private int maxlevel = 4;

    private GameDataManager _gameDataManager;
    private DateTime startTime;
    public InstrumentSeparation ModeManager;
    void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;
        
        gameplayAudio = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData =  DataManager.ReadJson(path);
        
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        SetMode();
        
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
        StartRound();
    }

    protected override void SetDifficultyChanges()
    {
        
    }

    public override void SetRoundFunctionality()
    {
        
    }

    protected override void EndRound()
    {
        
    }

    protected override void StartRound()
    {
        
    }

    protected override void RestartLevel()
    {
        currentScore = 0;
        round = 1;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
