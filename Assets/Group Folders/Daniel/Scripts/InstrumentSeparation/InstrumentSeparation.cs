using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstrumentSeparation : MonoBehaviour
{
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject endGameResults;

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
        instructions.SetActive(true);
        endGameResults.SetActive(false);
        game.SetActive(false);
    }

    public void StartGame()
    {
        instructions.SetActive(false);
        endGameResults.SetActive(false);
        game.SetActive(true);
    }

    public void EndGame()
    {
        instructions.SetActive(false);
        game.SetActive(false);
        endGameResults.SetActive(true);
    }

    IEnumerator WaitForFrame()
    {
        yield return new WaitForEndOfFrame();
    }
}
