using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class InteractableInstrument : MonoBehaviour
{
    public bool CanBeHeard { get; private set; }
    public bool HasClickedOnce { get; private set; }
    private XRRayInteractor _interactor;
    private XRBaseInteractable _interactable;
    private XRGrabInteractable _grabInteractable;
    private RayLengthControl _rayLengthControl;
    private Outline _outline;
    private Button _button;
    private TextMeshProUGUI _buttonText;

    private const string HearingText = "Jeg kan høre instrumentet";
    private const string CannotHearText = "Jeg kan IKKE høre instrumentet";
    private const string DefaultText = "Ikke valgt endnu";
    
    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _interactable = _grabInteractable;

        _interactor = FindObjectOfType<XRRayInteractor>();
        _rayLengthControl = FindObjectOfType<RayLengthControl>();

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
        _buttonText.text = DefaultText;
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
        _interactor.lineType = _rayLengthControl.rayLineType = XRRayInteractor.LineType.BezierCurve;
    }

    private void OnStopInteraction(SelectExitEventArgs args)
    {
        ToggleOutline();
        _interactor.lineType = _rayLengthControl.rayLineType = XRRayInteractor.LineType.StraightLine;
        _rayLengthControl.XRRayInteractor.maxRaycastDistance = 10000;
    }

    private void OnInstrumentHearingButtonClicked()
    {
        HasClickedOnce = true;
        CanBeHeard = !CanBeHeard;
        _buttonText.text = (CanBeHeard) ? HearingText : CannotHearText;
        _button.image.color = (CanBeHeard) ? new Color(0, 100, 0) : new Color(128, 0, 0);
    }

    private void ToggleOutline() => _outline.enabled = !_outline.enabled;

}
