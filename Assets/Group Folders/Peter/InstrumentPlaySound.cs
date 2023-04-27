using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentPlaySound : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

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
