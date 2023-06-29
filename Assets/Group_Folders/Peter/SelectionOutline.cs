using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectionOutline : MonoBehaviour
{
    public Outline outline;
    public GameObject selected;
    private OutlineManager manager;
    private XRSimpleInteractable interactable;
    public delegate void outlineChanged(GameObject gameObject);
    public event outlineChanged OnOutlineChange;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<OutlineManager>();
        outline = GetComponent<Outline>();
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(SetOutline);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(SetOutline);
    }

    public void SetOutline(SelectEnterEventArgs args)
    {
        Debug.Log("It is working");
        outline.enabled = !outline.enabled;
        OnOutlineChange?.Invoke(gameObject);
    }
}
