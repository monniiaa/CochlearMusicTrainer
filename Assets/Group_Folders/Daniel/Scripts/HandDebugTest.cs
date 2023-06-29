using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandDebugTest : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_inputActionAsset;
    private InputAction _handPosition;
    private InputAction _handRotation;
    private InputAction _handTracked;

    private void OnEnable()
    {
        _handPosition = m_inputActionAsset.FindActionMap("XRI LeftHand Tracking").FindAction("Position");
        _handRotation = m_inputActionAsset.FindActionMap("XRI LeftHand Tracking").FindAction("Rotation");
        _handTracked = m_inputActionAsset.FindActionMap("XRI LeftHand Tracking").FindAction("Tracking");
        
        _handPosition.Enable();
        _handRotation.Enable();
        _handTracked.Enable();

        _handPosition.performed += OnHandMove;
        _handRotation.performed += OnHandRotate;
        _handTracked.performed += OnHandTracked;

    }

    private void OnDisable()
    {
        _handPosition.Disable();
        _handRotation.Disable();
        _handTracked.Disable();

        
        _handPosition.performed -= OnHandMove;
        _handRotation.performed -= OnHandRotate;
        _handTracked.performed -= OnHandTracked;
    }

    private void OnHandMove(InputAction.CallbackContext ctx)
    {
        Debug.Log( "Position: " + ctx.ReadValue<Vector3>());
    }
    
    private void OnHandRotate(InputAction.CallbackContext ctx)
    {
        Debug.Log("Rotation: " + ctx.ReadValue<Quaternion>().eulerAngles);
    }
    
    private void OnHandTracked(InputAction.CallbackContext ctx)
    {
        Debug.Log("Tracked: " + ctx.ReadValue<int>());
    }
}
