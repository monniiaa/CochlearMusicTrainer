using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class ShootInteraction : MonoBehaviour
{
    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 4;
    public static int points { get; private set; }

    private void OnEnable()
    {
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
        distanceTracker.distanceEvent += IncrementPoints;
    }

    private void IncrementPoints(float distance)
    {
        if (distance <= distanceTreshold)
        {
            points+= 1;
        }
        else
        {
            points+= 0;
        }
    }




}
