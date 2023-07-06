using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollisionChecker : MonoBehaviour
{
    
    LocalizationManager localizationManager;
    private void Awake()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            /*    Debug.Log("Hit another target");
            localizationManager.RemoveTarget(other.gameObject);
            other.gameObject.GetComponent<DeletusMaximus>().Destroy();*/
        }
    }
}
