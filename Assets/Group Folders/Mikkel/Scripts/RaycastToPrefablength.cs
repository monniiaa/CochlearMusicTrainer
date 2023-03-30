using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class RaycastToPrefablength : MonoBehaviour
{
    public GameObject prefab; // the prefab to measure
    public Transform raycastOrigin; // the origin of the raycast

    private XRInputSubsystem xrInputSubsystem; // the XR input subsystem

  

public class ExampleScript : MonoBehaviour
{
    public GameObject prefab; // the prefab to measure
    public Transform raycastOrigin; // the origin of the raycast

    private InputDevice characterController; // the character controller device

    private void Start()
    {
        // get the character controller device
        //characterController = XRInputSubsystemHelpers.CharacterControllerDevice;

            //muligvis check vinklen ift hvor personen kigger, se om personen kigger lige på objektet, om de er orienteret ordenligt

        if (characterController == null)
        {
            Debug.LogError("Failed to get character controller device.");
        }
    }

    private void Update()
    {
        // cast a ray from the origin in the forward direction
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit))
        {
            // measure the distance to the hit point
            float distance = Vector3.Distance(prefab.transform.position, hit.point);

            // do something with the distance
            Debug.Log("Distance to hit point: " + distance);
        }
    }
}
}
