using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmInstrumentSeparation : MonoBehaviour
{
    private InteractableInstrument[] _interactableInstruments;
    private Button _confirmButton;
    void Awake()
    {
        _confirmButton = GetComponent<Button>();
        _interactableInstruments = FindObjectsOfType<InteractableInstrument>();
    }

    private void OnEnable()
    {
        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    private void OnDisable()
    {
        _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
    }

    private void OnConfirmButtonClicked()
    {
        foreach (var interactableInstrument in _interactableInstruments)
        {
            if (interactableInstrument.hasClickedOnce)
            {
                // Send out a message that all the instruments needs to be chosen
                break;
            }
            // Measure the distance between the instruments and the player 
        }
    }
}
