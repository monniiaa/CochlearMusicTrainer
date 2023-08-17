using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckMelodyDirection : MonoBehaviour
{

    public delegate void ContourRoundCorrect(Oscillator speaker);
    public ContourRoundCorrect contourRoundCorrectEvent;
    
    public delegate void ContourRoundIncorrect(Oscillator speaker);
    public ContourRoundIncorrect contourRoundIncorrectEvent;
    
    public string direction;

    
    public Material successMat;
    public Material failMat;

    private void Awake()
    {
        direction = gameObject.name;
    }
    

    private void OnFinished(Oscillator speaker)
    {
        if(speaker!= null)
            if (speaker.direction == direction)
            {
                if(contourRoundCorrectEvent != null) contourRoundCorrectEvent(speaker);
            }
            else
            {
                if(contourRoundIncorrectEvent != null) contourRoundIncorrectEvent(speaker);
            }
    }

    
    
    
}
