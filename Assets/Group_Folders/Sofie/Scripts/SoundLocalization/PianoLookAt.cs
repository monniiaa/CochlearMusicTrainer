using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoLookAt : MonoBehaviour
{
    [SerializeField]
    private Transform _toRotate;

    [SerializeField]
    private Transform _target;

    private void Awake()
    {
        _toRotate = transform;
        _target = FindObjectOfType<Camera>().transform;
    }
    
    void Update()
    {
        _toRotate.LookAt(_target);
        Vector3 newRot = _toRotate.rotation.eulerAngles;
        newRot.x = -90;
        newRot.y = newRot.y - 80;
        transform.rotation = Quaternion.Euler(newRot);
    }
}
