using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(XRRayInteractor))]
public class RayLengthControl : MonoBehaviour
{
    private XRRayInteractor _xrRayInteractor;

    [SerializeField] private float duration;
    private float time = 0;
    private const float MaxLength = 50;
    private const float MinLength = 1;
    private float currentLength;
    public bool isChangingLength;
    public int joystickDirection;
    void Start()
    {
        _xrRayInteractor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangingLength)
        {
            time = 0;
            return;
        }
        currentLength = _xrRayInteractor.maxRaycastDistance;
        if (currentLength is > MaxLength or < MinLength)
        {
            _xrRayInteractor.maxRaycastDistance = Mathf.Clamp(_xrRayInteractor.maxRaycastDistance, MinLength, MaxLength);
            return;
        }
        _xrRayInteractor.maxRaycastDistance += joystickDirection * duration * Time.deltaTime;
    }
}
