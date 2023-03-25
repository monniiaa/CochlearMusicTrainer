using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class InteractableInstrument : MonoBehaviour
{

    private XRBaseInteractable _interactable;
    private Outline _outline;
    
    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
        _outline = gameObject.AddComponent<Outline>();
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
    }

    private void OnStopInteraction(SelectExitEventArgs args)
    {
        ToggleOutline();
    }

    private void ToggleOutline() => _outline.enabled = !_outline.enabled;

}
