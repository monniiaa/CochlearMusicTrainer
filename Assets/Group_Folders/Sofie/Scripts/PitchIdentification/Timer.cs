using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft;

    public bool timerOn = false;

    public TextMeshProUGUI timetTxt;
    // Start is called before the first frame update
    private void Awake()
    {
        timetTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        timerOn = true;

    }
    

    // Update is called once per frame
    void Update()
    {
        if(timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerOn = false;
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timetTxt.text = string.Format("{0:00}", seconds);
    }
}
