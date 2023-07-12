using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimbreText : MonoBehaviour
{

    private GameDataManager _gameDataManager;
    int currentLevel;
    
    [SerializeField] private TextMeshProUGUI timbreText;
    

    private void Start()
    {
        _gameDataManager = GameDataManager.Instance;
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        
        SetText();
    }

    private void SetText()
    {
        if (currentLevel >= 4 && currentLevel < 7)
        {
            SetAmountText();
        }
        else
        {
            SetIdentificationText();
        } 
    }

    private void SetIdentificationText()
    {
        timbreText.text = "Din opgave er at vælge de instrumenter der spiller";
    }

    private void SetAmountText()
    {
        timbreText.text = "Din opgave er at vælge antallet af instrumenter der spiller";
    }
    

}
