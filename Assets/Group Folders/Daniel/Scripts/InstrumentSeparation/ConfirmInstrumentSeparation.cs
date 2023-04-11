using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmInstrumentSeparation : MonoBehaviour
{
    [SerializeField]
    private GameObject missingInstrumentsUi;
    private InteractableInstrument[] _interactableInstruments;
    private Button _confirmButton;
    void Awake()
    {
        _confirmButton = GetComponent<Button>();
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
        _interactableInstruments = FindObjectsOfType<InteractableInstrument>();
        foreach (var interactableInstrument in _interactableInstruments)
        {
            if (!interactableInstrument.HasClickedOnce)
            {
                ShowMissingInstrumentsMessage();
                // Send out a message that all the instruments needs to be chosen
                break;
            }
            
            // Measure the distance between the instruments and the player 
        }
    }

    private void ShowMissingInstrumentsMessage()
    {
        missingInstrumentsUi.SetActive(true);
    }
}
