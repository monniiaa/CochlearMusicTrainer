using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentPlaySound : MonoBehaviour
{
    AudioSource audioSource;
    private AudioSource[] audioSources;

    private void Awake() 
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        audioSource.Play();
    }
}
