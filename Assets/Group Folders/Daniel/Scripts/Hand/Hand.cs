using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    [SerializeField]
    private float speed;
    
    private Animator _animator;
    
    private float _gripTarget;
    private float _triggerTarget;
    private float _thumbTarget;
    
    private float _gripCurrent;
    private float _triggerCurrent;
    private float _thumbCurrent;
    
    private static readonly int Grip = Animator.StringToHash("Grip");
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    private static readonly int Thumb = Animator.StringToHash("Thumb");

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    public void SetGrip(float value)
    {
        _gripTarget = value;
    }
    
    public void SetTrigger(float value)
    {
        _triggerTarget = value;
    }
    
    public void SetThumb(float value)
    {
        _thumbTarget = value;
    }

    void AnimateHand()
    {
        if (_thumbCurrent != _thumbTarget)
        {
            _thumbCurrent = Mathf.MoveTowards(_thumbCurrent, _thumbTarget, Time.deltaTime * speed);
            _animator.SetFloat(Thumb, _thumbCurrent);
        }
        if (_gripCurrent != _gripTarget)
        {
            _gripCurrent = Mathf.MoveTowards(_gripCurrent, _gripTarget, Time.deltaTime * speed);
            _animator.SetFloat(Grip, _gripCurrent);
        }
        if (_triggerCurrent != _triggerTarget)
        {
            _triggerCurrent = Mathf.MoveTowards(_triggerCurrent, _triggerTarget, Time.deltaTime * speed);
            _animator.SetFloat(Trigger, _triggerCurrent);
        }
    }
}
