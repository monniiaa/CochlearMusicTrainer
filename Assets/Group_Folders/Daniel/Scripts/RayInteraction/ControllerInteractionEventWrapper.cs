using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

[RequireComponent(typeof(ActionBasedController))]
public class ControllerInteractionEventWrapper : MonoBehaviour
{
    private ActionBasedController _actionBasedController;

    public UnityEvent triggerPressed;
    public UnityEvent triggerReleased;

    public UnityEvent<float> gripPressed;
    public UnityEvent joystickReleased;
    public UnityEvent<Vector2> joystickNorthPressed;
    public UnityEvent<Vector2> joystickSouthPressed;

    private void Awake()
    {
        _actionBasedController = GetComponent<ActionBasedController>();
    }

    private void OnEnable()
    {
        _actionBasedController.activateAction.action.performed += _ => triggerPressed.Invoke();
        _actionBasedController.activateAction.action.canceled += _ => triggerReleased.Invoke();
        _actionBasedController.selectActionValue.action.performed += context => gripPressed.Invoke(context.ReadValue<float>());
        _actionBasedController.translateAnchorAction.action.performed += context => joystickNorthPressed.Invoke(context.ReadValue<Vector2>());
        _actionBasedController.rotateAnchorAction.action.performed += context => joystickSouthPressed.Invoke(context.ReadValue<Vector2>());
        
        // Joystick Released
        _actionBasedController.translateAnchorAction.action.canceled += _ => joystickReleased.Invoke();
        _actionBasedController.rotateAnchorAction.action.canceled += _ => joystickReleased.Invoke();
        
    }

    private void OnDisable()
    {
        
    }
}
