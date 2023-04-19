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
    private GameObject finishButton;
    private InteractableInstrument[] _interactableInstruments;
    private InstrumentSeparation _instrumentSeparation;
    private XRSimpleInteractable _buttonPokeInteractable;

    private void Awake()
    {
        _instrumentSeparation = InstrumentSeparation.Instance;
        _buttonPokeInteractable = finishButton.GetComponentInChildren<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForInstruments());
    }

    private void OnDisable()
    {
        foreach (var interactableInstrument in _interactableInstruments)
        {
            interactableInstrument.Button.onClick.RemoveListener(OnConfirmHearing);
        }
        _buttonPokeInteractable.selectEntered.RemoveListener(OnPokableButtonPressed);
        finishButton.SetActive(false);
    }

    private void OnConfirmHearing()
    {
        if (IsMissingSelections())
        {
            finishButton.SetActive(false);
            return;
        }
        
        finishButton.SetActive(true);
    }

    private void OnPokableButtonPressed(SelectEnterEventArgs args)
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

        _buttonPokeInteractable.selectEntered.AddListener(OnPokableButtonPressed);
    }
}
