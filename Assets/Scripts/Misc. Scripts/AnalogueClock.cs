using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnalogueClock : MonoBehaviour
{
   
    public GameObject secondsHand, minutesHand, hoursHand;

    private void FixedUpdate()
    {
       DateTime currentTime = DateTime.Now; //Function to gain current system time. 

       float secondsDegree = -(currentTime.Second / 60f) * 360f; 
       secondsHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, secondsDegree)); //Sets rotation

       float minutesDegree = -(currentTime.Minute / 60f) * 360f; 
       minutesHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, minutesDegree)); //Sets rotation

       float hoursDegree = -(currentTime.Hour / 12f) * 360f;
       hoursHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, hoursDegree));
    }
}
