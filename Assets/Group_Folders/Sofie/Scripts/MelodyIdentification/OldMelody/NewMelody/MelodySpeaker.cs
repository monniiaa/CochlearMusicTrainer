using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodySpeaker : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    public Material mat;
    
    public AudioClip currentClip;


    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    
    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }

    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");
    }
    
    public void SetClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void PlayClip()
    {
        if(audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);
    }
    
    public void StopAudio()
    {
        audioSource.Stop();
    }
}
