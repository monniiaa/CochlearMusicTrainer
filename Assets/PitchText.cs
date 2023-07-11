using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PitchText : MonoBehaviour
{
    private GameDataManager _gameDataManager;
    int currentLevel;
    
    [SerializeField] private TextMeshProUGUI pitchText;
    

    private void Start()
    {
        _gameDataManager = GameDataManager.Instance;
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        
        SetText();
    }

    private void SetText()
    {
        if(currentLevel == 3 || currentLevel == 4)
            SetInstrumentText();
        else if (currentLevel == 10)
        {
            SetSongText();
        } else
        {
            SetToneText();
        }
    }

    private void SetInstrumentText()
    {
        pitchText.text = "Din opgave er at vælge det instrument der spiller mest højfrekvent";
    }

    private void SetSongText()
    {
        pitchText.text = "Din opgave er at vælge den sang der spiller mest højfrekvent";
    }
    

    private void SetToneText()
    {
        pitchText.text = "Din opgave er at vælge den højtaler der spiller den mest højfrekvente tone";
    }
}
