using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_actionAsset;
    [SerializeField] private XRRayInteractor m_xrRayInteractor;
    [SerializeField] private TeleportationProvider m_teleportationProvider;
    
    private InputAction _activate;
    private InputAction _cancel;
    private InputAction _thumbstick;
    private bool _isActive;

    void Start()
    {
        m_xrRayInteractor.enabled = false;
    }

    private void OnEnable()
    {
        _activate = m_actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        _cancel = m_actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        _thumbstick = m_actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        
        _activate.Enable();
        _cancel.Enable();
        _thumbstick.Enable();

        _activate.performed += OnTeleportActivated;
        _cancel.performed += OnTeleportCancelled;
    }

    private void OnDisable()
    {
        _activate.Disable();
        _cancel.Disable();
        _thumbstick.Disable();
        
        _activate.performed -= OnTeleportActivated;
        _cancel.performed -= OnTeleportCancelled;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive) return;
        if(_thumbstick.triggered) return;
        
        
        
        if (!m_xrRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            m_xrRayInteractor.enabled = false;
            _isActive = false;
            return;
        }

        TeleportRequest teleportRequest = new TeleportRequest()
        {
            destinationPosition = hit.point
        };

        m_teleportationProvider.QueueTeleportRequest(teleportRequest);
        m_xrRayInteractor.enabled = false;
        _isActive = false;
    }

    private void OnTeleportActivated(InputAction.CallbackContext ctx)
    {
        m_xrRayInteractor.enabled = true;
        _isActive = true;
    }
    
    private void OnTeleportCancelled(InputAction.CallbackContext ctx)
    {
        m_xrRayInteractor.enabled = false;
        _isActive = false;
    }
}
