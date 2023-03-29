using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class InteractableInstrument : MonoBehaviour
{
    public bool canBeHeard { get; private set; }
    public bool hasClickedOnce { get; private set; }
    private XRBaseInteractable _interactable;
    private Outline _outline;
    private Button _button;
    private TextMeshProUGUI _buttonText;

    private const string HearingText = "Jeg kan høre instrumentet";
    private const string CannotHearText = "Jeg kan IKKE høre instrumentet";
    
    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();

        _button = GetComponentInChildren<Button>();
        _buttonText = _button.GetComponentInChildren<TextMeshProUGUI>();

        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.green;
        _outline.OutlineWidth = 5f;
        _outline.enabled = false;
    }

    private void Start()
    {
        _button.image.color = Color.gray;
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnInteraction);
        _interactable.selectExited.AddListener(OnStopInteraction);
        
        _button.onClick.AddListener(OnInstrumentHearingButtonClicked);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnInteraction);
        _interactable.selectExited.RemoveListener(OnStopInteraction);
        _button.onClick.RemoveListener(OnInstrumentHearingButtonClicked);
    }

    private void OnInteraction(SelectEnterEventArgs args)
    {
        ToggleOutline();
    }

    private void OnStopInteraction(SelectExitEventArgs args)
    {
        ToggleOutline();
    }

    private void OnInstrumentHearingButtonClicked()
    {
        hasClickedOnce = true;
        canBeHeard = !canBeHeard;
        _buttonText.text = (canBeHeard) ? HearingText : CannotHearText;
        _button.image.color = (canBeHeard) ? new Color(0, 100, 0) : new Color(128, 0, 0);
    }

    private void ToggleOutline() => _outline.enabled = !_outline.enabled;

}
