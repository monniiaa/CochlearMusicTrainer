using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstrumentSeparation : MonoBehaviour
{
    [SerializeField] private string minigame;
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject endGameResults;

    private JsonManager _jsonManager;
    private DateTime _startTime;
    private DateTime _endTime;
    private bool hasPlayed;

    private static InstrumentSeparation _instance;

    public static InstrumentSeparation Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<InstrumentSeparation>();
            }

            return _instance;
        }
    }

    void Start()
    {
        ShowInstructions();
    }

    public void ShowInstructions()
    {
        _startTime = DateTime.Now;
        instructions.SetActive(true);
        endGameResults.SetActive(false);
        game.SetActive(false);
    }

    public void StartGame()
    {
        if(hasPlayed) _startTime = DateTime.Now;
        instructions.SetActive(false);
        endGameResults.SetActive(false);
        game.SetActive(true);
    }

    public void EndGame()
    {
        _endTime = DateTime.Now;
        JsonManager.WriteDataToFile<PlayTimeData>(new PlayTimeData(minigame, _startTime.ToString("dd/MM/yy H:mm:ss"), _endTime.ToString("dd/MM/yy H:mm:ss")));
        hasPlayed = true;
        instructions.SetActive(false);
        game.SetActive(false);
        endGameResults.SetActive(true);
    }
}
