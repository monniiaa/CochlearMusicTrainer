using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class MelodyInteractable : MonoBehaviour
{
      // public InstrumentFamily instrumentFamily;
    public bool CanBeHeard { get; private set; }
    public bool HasClickedOnce { get; private set; }
    private XRRayInteractor _interactor;
    private XRBaseInteractable _interactable;
    private XRGrabInteractable _grabInteractable;
    private RayLengthControl _rayLengthControl;
    private Outline _outline;

    
    
    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _interactable = _grabInteractable;

        _interactor = FindObjectOfType<XRRayInteractor>();
        _rayLengthControl = FindObjectOfType<RayLengthControl>();
        

        _outline = GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.green;
        _outline.OutlineWidth = 5f;
        _outline.enabled = false;
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(OnInteraction);
        _interactable.selectExited.AddListener(OnStopInteraction);
    }

    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(OnInteraction);
        _interactable.selectExited.RemoveListener(OnStopInteraction);
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

    private void ToggleOutline() => _outline.enabled = !_outline.enabled;
}
