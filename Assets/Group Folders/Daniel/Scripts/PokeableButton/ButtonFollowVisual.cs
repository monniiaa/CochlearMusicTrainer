using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Credit: https://www.youtube.com/watch?v=bts8VkDP_vU
/// </summary>
public class ButtonFollowVisual : MonoBehaviour
{
    public Transform m_visualTarget;
    public Vector3 m_localAxis;
    public float m_resetSpeed = 5;
    public float m_followAngleThreshold = 45;

    private bool _freeze;

    private Vector3 _initialLocalPos;

    private Vector3 _offset;
    private Transform _pokeAttachTransform;
    
    private XRBaseInteractable _interactable;
    private bool _isFollowing;

    void Start()
    {
        _initialLocalPos = m_visualTarget.localPosition;
        
        _interactable = GetComponent<XRBaseInteractable>();
        _interactable.hoverEntered.AddListener(Follow);
        _interactable.hoverExited.AddListener(Reset);
        _interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(HoverEnterEventArgs args)
    {
        if (args.interactorObject is not XRPokeInteractor interactor) return;
        XRPokeInteractor pokeInteractor = interactor;

        _isFollowing = true;
        _freeze = false;
        
        _pokeAttachTransform = pokeInteractor.attachTransform;
        _offset = m_visualTarget.position - _pokeAttachTransform.position;

        float pokeAngle = Vector3.Angle(_offset, m_visualTarget.TransformPoint(m_localAxis));
        Debug.Log(pokeAngle);
        if (pokeAngle < m_followAngleThreshold)
        {
            _isFollowing = true;
            _freeze = false;
        }
    }

    public void Reset(HoverExitEventArgs args)
    {
        if (args.interactorObject is not XRPokeInteractor) return;
        _isFollowing = false;
        _freeze = false;
    }

    public void Freeze(SelectEnterEventArgs args)
    {
        if (args.interactorObject is not XRPokeInteractor) return;
        _freeze = true;
    }

    void Update()
    {
        if (_freeze) return;
        if (_isFollowing)
        {
            Vector3 localTargetPosition = m_visualTarget.InverseTransformPoint(_pokeAttachTransform.position + _offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, m_localAxis);
            
            m_visualTarget.position = m_visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            m_visualTarget.localPosition = Vector3.Lerp(m_visualTarget.localPosition, _initialLocalPos, Time.deltaTime * m_resetSpeed);
        }
    }
}
