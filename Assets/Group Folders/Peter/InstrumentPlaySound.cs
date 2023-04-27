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
        audioSource.Play();
    }
}
