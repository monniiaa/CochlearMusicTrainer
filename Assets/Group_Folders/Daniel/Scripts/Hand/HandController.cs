using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset m_inputActionAsset;
    private InputActionMap _inputActionMap;
    private InputAction _thumbRest;
    private InputAction _trigger;
    private InputAction _grip;
    private ActionBasedController _actionBasedController;
    
    [SerializeField] private Hand m_hand;
    
    void Awake()
    {
        
        _inputActionMap = m_inputActionAsset.FindActionMap("XRI RightHand Interaction");
        _thumbRest = _inputActionMap.FindAction("Thumbrest");
        _trigger = _inputActionMap.FindAction("Activate");
        _grip = _inputActionMap.FindAction("Select");
    }
    
    private void OnEnable()
    {
        _inputActionMap.Enable();
    }

    private void OnDisable()
    {
        _inputActionMap.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        m_hand.SetGrip(_grip.ReadValue<float>());
        m_hand.SetTrigger(_trigger.ReadValue<float>());
        if(_thumbRest != null) m_hand.SetThumb(_thumbRest.ReadValue<float>());
    }
}
