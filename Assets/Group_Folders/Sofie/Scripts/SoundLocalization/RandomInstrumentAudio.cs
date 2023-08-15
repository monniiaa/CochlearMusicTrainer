using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInstrumentAudio : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    
    [SerializeField] AudioSource audioSource;




    public void SetClip(int clipIndex)
    {
        audioSource.clip = audioClips[clipIndex];
    
    }
}
