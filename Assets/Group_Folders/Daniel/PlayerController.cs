using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_movementSpeed;
    private OVRCameraRig _cameraRig;
    private Rigidbody _rb;
    
    private void Start()
    {
        _cameraRig = FindObjectOfType<OVRCameraRig>();
        _rb = _cameraRig.GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        //var direction = new Vector3(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x,0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y);
        /*
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x != 0)
        {
            _rb.velocity = -transform.forward * m_movementSpeed;   
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y != 0)
        {
            _rb.velocity = transform.forward * m_movementSpeed;   
        }
        */
        
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < 0)
        {
            _rb.position += -transform.right * m_movementSpeed * Time.deltaTime;   
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > 0)
        {
            _rb.position += transform.right * m_movementSpeed * Time.deltaTime;   
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y < 0)
        {
            _rb.position += -transform.forward * m_movementSpeed * Time.deltaTime;   
        }
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y > 0)
        {
            _rb.position += transform.forward * m_movementSpeed * Time.deltaTime;   
        }
        
        //_rb.rotation *= Quaternion.Euler(Vector3.up * (Input.GetAxis("Mouse X") * m_movementSpeed));

    }
}
