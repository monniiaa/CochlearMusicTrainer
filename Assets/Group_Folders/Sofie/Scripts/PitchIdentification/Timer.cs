using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft;

    public bool timerOn = false;
    [SerializeField]
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    public TextMeshProUGUI timetTxt;
    private Color OGColor;
    Animator animator;

    private bool play = false;

    // Start is called before the first frame update
    private void Awake()
    {
        timetTxt = GetComponentInChildren<TextMeshProUGUI>();
        OGColor = timetTxt.color;
        _audioSource = GetComponentInChildren<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        _audioClip = Resources.Load("CountdownSound/beep")  as AudioClip;
        _audioSource.volume = 0.1f;
        timeLeft = 10f;

    }

    void Start()
    {
        //StartCoroutine(Reset());
    }

    public IEnumerator Reset()
    {
        
        play = true;
        timetTxt.color = OGColor;
        timetTxt.enabled = false;
        _audioSource.Stop();
        yield return new WaitForSeconds(1f);
        animator.SetBool("StartPopUp", true);
        timetTxt.enabled = true;
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
            }else
            {
                timeLeft = 0;
                timerOn = false;
                animator.SetBool("StartPopUp", false);
            }

            if (timeLeft <= 3 && play)
            {
                play = false;
                timetTxt.color = Color.red;
                _audioSource.PlayOneShot(_audioClip);
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timetTxt.text =  seconds.ToString();

    }
    

    
    
    
}
