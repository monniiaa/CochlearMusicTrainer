using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroExplanation : MonoBehaviour
{
    public TMP_Text text1, text2, text3;
    public PlayRandomSound soundStart;

    private void Start()
    {
        soundStart = FindObjectOfType(typeof(PlayRandomSound)) as PlayRandomSound;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
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
                text3.gameObject.SetActive(false);
                soundStart.Round();
                soundStart.time = Time.deltaTime;
                Debug.Log(soundStart.time);
            }
        }
    }
}
