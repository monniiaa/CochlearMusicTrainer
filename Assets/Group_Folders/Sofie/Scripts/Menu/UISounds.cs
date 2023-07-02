using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UISounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] hoverSounds;
    [SerializeField] private AudioClip selectSound;

    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        hoverSounds = Resources.LoadAll<AudioClip>("PopSounds");
        selectSound = Resources.Load<AudioClip>("SelectSound/pop2");
        
    }

    public void PlayHoverSound()
    {
        int index = Random.Range(0, hoverSounds.Length);
        audioSource.volume = 0.1f;
        audioSource.PlayOneShot(hoverSounds[index]);
    }

    public void PlaySelectSound()
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(selectSound);
    }
}
