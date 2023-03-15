using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRRayInteractor))]
public class RayLengthControl : MonoBehaviour
{
    private XRRayInteractor _xrRayInteractor;

    [SerializeField] private float speed;
    [SerializeField] private float maxLength = 50;
    [SerializeField] private float minLength = 1;
    private float _currentLength;
    
    public bool isChangingLength { set; get; }
    public int joystickDirection { set; get; } // 1 means forward, -1 means backwards
    void Awake()
    {
        _xrRayInteractor = GetComponent<XRRayInteractor>();
        _currentLength = _xrRayInteractor.maxRaycastDistance;
        _currentLength = Mathf.Clamp(_xrRayInteractor.maxRaycastDistance, minLength, maxLength);
    }
    
    void Update()
    {
        if (!isChangingLength) return;

        _currentLength = _xrRayInteractor.maxRaycastDistance;
        
        if (_currentLength > maxLength || _currentLength < minLength)
        {
            _xrRayInteractor.maxRaycastDistance = Mathf.Clamp(_xrRayInteractor.maxRaycastDistance, minLength, maxLength);
            return;
        }

        _xrRayInteractor.maxRaycastDistance += joystickDirection * speed * Time.deltaTime;
    }

    public void ChangeDirectionFromJoystick(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            joystickDirection = 0;
            return;
        }
        joystickDirection = (direction.x > 0) ? (int) direction.x : (int) direction.y;
    }
}
