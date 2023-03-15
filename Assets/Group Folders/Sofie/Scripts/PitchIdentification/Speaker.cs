using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip sound;
    public int pitch;
 

    public  void SetPitch()
    {
        audioSource.pitch = pitch;
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(sound);
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
