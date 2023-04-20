using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroExplanation : MonoBehaviour
{
    public TMP_Text text1, text2, text3;
    public GameObject canvas;
    //In order to call the method from PlayRandomSound, there needs to be a reference to the script, which is declared here. 
    public PlayRandomSound soundStart;

    private void Start()
    {
        //It then finds the object with the PlayRandomSound, in order to establish the link, I guess
        soundStart = FindObjectOfType(typeof(PlayRandomSound)) as PlayRandomSound;
    }

    public void IntroText()
    {
            if(text1.IsActive())
            {
                text1.gameObject.SetActive(false);
                text2.gameObject.SetActive(true);
            }
            else if (text2.IsActive())
            {
                text2.gameObject.SetActive(false);
                text3.gameObject.SetActive(true);
            }
            else if (text3.IsActive())
            {
                canvas.SetActive(false);
                text3.gameObject.SetActive(false);
                soundStart.Round();
                soundStart.time = Time.deltaTime;
                Debug.Log(soundStart.time);
            }
    }
}