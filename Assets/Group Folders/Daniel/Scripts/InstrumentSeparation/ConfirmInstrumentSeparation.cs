using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class ConfirmInstrumentSeparation : MonoBehaviour
{
    [SerializeField]
    private GameObject finishButtonHolder;
    private InteractableInstrument[] _interactableInstruments;
    private InstrumentSeparation _instrumentSeparation;
    private Button _finishButton;

    private void Awake()
    {
        _instrumentSeparation = InstrumentSeparation.Instance;
        _finishButton = finishButtonHolder.GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForInstruments());
        finishButtonHolder.SetActive(false);
    }

    private void OnDisable()
    {
        if(_interactableInstruments != null)
        {
            foreach (var interactableInstrument in _interactableInstruments)
            {
                interactableInstrument.Button.onClick.RemoveListener(OnConfirmHearing);
            }
        }
        
        _finishButton.onClick.RemoveListener(OnFinishButtonPressed);
        finishButtonHolder.SetActive(false);
    }

    private void OnConfirmHearing()
    {
        if (IsMissingSelections())
        {
            finishButtonHolder.SetActive(false);
            return;
        }
        
        finishButtonHolder.SetActive(true);
    }

    private void OnFinishButtonPressed()
    {
        _instrumentSeparation.EndGame();
    }

    private bool IsMissingSelections() => _interactableInstruments.Any(i => !i.HasClickedOnce);

    private IEnumerator WaitForInstruments()
    {
        yield return new WaitForEndOfFrame();
        _interactableInstruments = FindObjectsOfType<InteractableInstrument>();
        
        foreach (var interactableInstrument in _interactableInstruments)
        {
            interactableInstrument.Button.onClick.AddListener(OnConfirmHearing);
        }

        _finishButton.onClick.AddListener(OnFinishButtonPressed);
    }
}
