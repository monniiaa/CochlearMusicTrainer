using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using UnityEngine;

public class InstrumentHaptics : MonoBehaviour
{
    // Start is called before the first frame update
    public HapticClip instrumentClip;
    private HapticClipPlayer player;
    
    private VibrationAndSoundHandler vibrationAndSoundHandler;
    
    bool shouldPlay = true;

    private void Awake()
    {
        player = new HapticClipPlayer(instrumentClip);
        vibrationAndSoundHandler = FindObjectOfType<VibrationAndSoundHandler>();
    }
    


    private void OnEnable()
    {
        if(vibrationAndSoundHandler != null)
        vibrationAndSoundHandler.VibrationEvent += ShouldPlay;
    }

    private void OnDisable()
    {
        if(vibrationAndSoundHandler != null)
        vibrationAndSoundHandler.VibrationEvent -= ShouldPlay;
    }


    public void ShouldPlay(bool shouldPlay)
    {
        this.shouldPlay = shouldPlay;
    }
    
    public void PlayHapticClip()
    {
        if (shouldPlay)
        {
            player.Play(HapticInstance.Hand.Right);
        }
    }

    public void StopHaptics()
    {
        player.Stop();
    }
}
