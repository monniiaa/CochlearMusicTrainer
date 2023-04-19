using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class DistanceTracker : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    
    public delegate void Distance();
    public event Distance distanceEvent;
 
   [SerializeField]
    private ActionBasedController controller;


    private void OnEnable()
    {
        controller.selectAction.action.performed += Grippressed;
    }

    private void OnDisable()
    {
        
    }

    private void Grippressed(InputAction.CallbackContext ctx)
    {
        GameObject speaker = GameObject.FindGameObjectWithTag("InteractablePrefabTag");
         // Log distance to console with 2 decimal places
        if(distanceEvent != null) distanceEvent();
    }
}
