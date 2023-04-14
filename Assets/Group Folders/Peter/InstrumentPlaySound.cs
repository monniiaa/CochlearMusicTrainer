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

    void Update()
    {
        transform.Rotate(0f,.1f,0f);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
