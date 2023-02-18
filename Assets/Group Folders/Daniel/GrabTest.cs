using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabTest : MonoBehaviour
{
    private XRSimpleInteractable _simpleInteractable;
    private Transform _followedTransform;
    private bool _isSelected;
    void OnEnable()
    {
        _simpleInteractable.selectEntered.AddListener(OnSelect);
        _simpleInteractable.selectExited.AddListener(OnDeselect);
    }

    private void Awake()
    {
        _simpleInteractable = GetComponent<XRSimpleInteractable>();
    }

    private void OnSelect(SelectEnterEventArgs action)
    {
        _followedTransform = action.interactorObject.GetAttachTransform(_simpleInteractable);
        _isSelected = true;
    }
    
    private void OnDeselect(SelectExitEventArgs action)
    {
        _isSelected = false;
        _followedTransform = null;
    }

    private void LateUpdate()
    {
        if (_isSelected)
        {
            transform.position = _followedTransform.position;
        }
    }
}
