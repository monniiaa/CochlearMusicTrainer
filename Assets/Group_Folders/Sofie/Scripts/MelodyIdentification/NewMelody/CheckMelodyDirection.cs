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
    public FinishButton finishButton;
    public Oscillator speaker;
    
    public Material successMat;
    public Material failMat;

    private void Awake()
    {
        direction = gameObject.name;
    }

    private void OnEnable()
    {
        finishButton.finishedEvent += OnFinished;
    }

    private void OnDisable()
    {
        finishButton.finishedEvent -= OnFinished;
    }

    private void OnFinished()
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

    private void OnTriggerEnter(Collider other)
    {
        finishButton.gameObject.SetActive(true);
        if(other.gameObject.GetComponent<Oscillator>() != null)
            speaker = other.gameObject.GetComponent<Oscillator>();
    }

    private void OnTriggerExit(Collider other)
    {
        finishButton.gameObject.SetActive(false);
        speaker = null;
    }
    
    
}
