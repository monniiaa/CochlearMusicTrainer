using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteInstrument : MonoBehaviour
{
     VibrationAndSoundHandler vibrationAndSoundHandler;
     AudioSource audioSource;
    
    
    private void Awake()
    {
        vibrationAndSoundHandler = FindObjectOfType<VibrationAndSoundHandler>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.8f;
    }
    
    public void ShouldPlay(bool shouldPlay)
    {
        if (shouldPlay)
        {
            audioSource.volume = 0.8f;
        } else {
            audioSource.volume = 0f;
        }
        
        Debug.Log(shouldPlay + "Audiosource volume: " + audioSource.volume);
    }
    
    private void OnEnable()
    {
        vibrationAndSoundHandler.SoundEvent += ShouldPlay;
    }

    private void OnDisable()
    {
        vibrationAndSoundHandler.SoundEvent -= ShouldPlay;
    }
}
