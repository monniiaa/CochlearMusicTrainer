using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class InteractableInstrument : MonoBehaviour
{
    public InstrumentFamily instrumentFamily;
    public bool CanBeHeard { get; private set; }
    public bool HasClickedOnce { get; private set; }
    private XRRayInteractor _interactor;
    private XRBaseInteractable _interactable;
    private XRGrabInteractable _grabInteractable;
    private RayLengthControl _rayLengthControl;
    private Outline _outline;
    public Button Button { get; private set; }
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

        Button = GetComponentInChildren<Button>(true);
        _buttonText = Button.GetComponentInChildren<TextMeshProUGUI>(true);

        _outline = GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.green;
        _outline.OutlineWidth = 5f;
        _outline.enabled = false;
    }

    private void Start()
    {
        // Button.image.color = Color.gray;
        //i need to change the sprite of the button to a checkmark or something
        Button.image.sprite = Resources.Load<Sprite>("ButtonSprites/KnapBaggrund");
        _buttonText.text = DefaultText;
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnInteraction);
        _interactable.selectExited.AddListener(OnStopInteraction);
        
        Button.onClick.AddListener(OnInstrumentHearingButtonClicked);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnInteraction);
        _interactable.selectExited.RemoveListener(OnStopInteraction);
        Button.onClick.RemoveListener(OnInstrumentHearingButtonClicked);
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
        // Button.image.color = (CanBeHeard) ? new Color(0, 100, 0) : new Color(128, 0, 0);
        Button.image.sprite = Resources.Load<Sprite>(CanBeHeard ? "ButtonSprites/GrønKnap" : "ButtonSprites/ForkertKnap");
        _buttonText.color = new Color(254, 250, 224);

    }

    private void ToggleOutline() => _outline.enabled = !_outline.enabled;

}
