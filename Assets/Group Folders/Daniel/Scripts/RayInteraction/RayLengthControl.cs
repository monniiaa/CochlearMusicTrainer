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
    public XRRayInteractor XRRayInteractor { get; private set; }

    public XRRayInteractor.LineType rayLineType = XRRayInteractor.LineType.StraightLine;

    [SerializeField] private float speed;
    [Header("Straight Line")]
    [SerializeField] private float maxLength = 50;
    [SerializeField] private float minLength = 1;
    [Header("Bezier Curve")]
    [SerializeField] private float maxDistance = 10;
    [SerializeField] private float minDistance = 1;
    private float _currentLength;
    private float _currentDistance;
    
    public bool IsChangingRay { set; get; }
    public int JoystickDirection { set; get; } // 1 means forward, -1 means backwards
    void Awake()
    {
        XRRayInteractor = GetComponent<XRRayInteractor>();
        _currentDistance = Mathf.Clamp(XRRayInteractor.velocity, minDistance, maxDistance);
        _currentLength = Mathf.Clamp(XRRayInteractor.maxRaycastDistance, minLength, maxLength);
    }
    
    void Update()
    {
        if (!IsChangingRay) return;

        switch (rayLineType)
        {
            case XRRayInteractor.LineType.StraightLine:
            
                _currentLength = XRRayInteractor.maxRaycastDistance;
        
                if (_currentLength > maxLength || _currentLength < minLength)
                {
                    XRRayInteractor.maxRaycastDistance = Mathf.Clamp(XRRayInteractor.maxRaycastDistance, minLength, maxLength);
                    return;
                }

                XRRayInteractor.maxRaycastDistance += JoystickDirection * speed * Time.deltaTime;
                break;
            
            case XRRayInteractor.LineType.BezierCurve:
                _currentDistance = XRRayInteractor.controlPointDistance;
                
                if (_currentDistance > maxDistance || _currentDistance < minDistance)
                {
                    XRRayInteractor.controlPointDistance = Mathf.Clamp(XRRayInteractor.controlPointDistance, minDistance, maxDistance);
                    return;
                }
                XRRayInteractor.controlPointDistance += JoystickDirection * speed * Time.deltaTime;
                break;
            case XRRayInteractor.LineType.ProjectileCurve:
                throw new NotImplementedException("No implementation for bezier curves");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ChangeDirectionFromJoystick(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            JoystickDirection = 0;
            IsChangingRay = false;
            return;
        }
        JoystickDirection = (direction.x > 0) ? (int) direction.x : (int) direction.y;
        IsChangingRay = true;
    }
}
