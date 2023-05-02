using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class InstrumentPlayerObserver : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    private DateTime startTime;
    private Button[] buttons;
    private Dictionary<string, int> instrumentCount = new Dictionary<string, int>();
    private void Awake() 
    {
        buttons = GetComponentsInChildren<Button>();    
    }

    private void Start() 
    {
        startTime = DateTime.Now;
    }

    private void OnEnable() 
    {
        startGameButton.onClick.AddListener(OnStartGame);
        foreach (Button button in buttons)
        {
            instrumentCount.Add(button.name, 0);
            button.onClick.AddListener(() => OnButtonClicked(button.name));
        }    
    }

    private void OnDisable() 
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(() => OnButtonClicked(button.name));
        }    
    }

    private void OnButtonClicked(string name)
    {
        instrumentCount[name]++;
    }

    private void OnStartGame()
    {
        JsonManager.WriteDataToFile<InstrumentIntroductionData>(
            new InstrumentIntroductionData(
                DateTime.Now,
                DateTime.Now - startTime,
                instrumentCount.Select(x => new InstrumentPlayData(x.Key, x.Value)).ToList()
            )
        );
    }
}
