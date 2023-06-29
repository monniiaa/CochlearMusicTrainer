using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumPlaySounds : MonoBehaviour
{
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrumStickHead"))
        {
            source.Play();
        }
    }
}
