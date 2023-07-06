using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundLocalizationMode : MonoBehaviour
{
    [SerializeField] private GameObject easy;
    [SerializeField] private GameObject medium;
    [SerializeField] private GameObject hard;
    
    private string path = "SoundLocalization";
    private GameDataManager gameDataManager;
    private int currentLevel;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
    
        currentLevel = (gameDataManager.currentLevel == 0) ? 1 : gameDataManager.currentLevel;
        Debug.Log(currentLevel);
        SetMode();
    }

    protected void SetMode()
    {
        if (currentLevel <= 3)
        {
            easy.SetActive(true);
            medium.SetActive(false);
            hard.SetActive(false);
        }
        else if (currentLevel > 3 && currentLevel <= 6)
        {
            easy.SetActive(false);
            medium.SetActive(true);
            hard.SetActive(false);
        }
        else if (currentLevel > 6)
        {
            easy.SetActive(false);
            medium.SetActive(false);
            hard.SetActive(true);
        }
    }
}
