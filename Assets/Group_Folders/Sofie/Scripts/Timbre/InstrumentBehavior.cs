using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentBehavior : MonoBehaviour
{
    public Animator animator;
    public string name;
    public AudioSource audioSource;
    [SerializeField]
    private ParticleSystem particleNotes;

    [SerializeField] private AudioClip[] clips;
    
    // Start is called before the first frame update
    private void Awake()
    {
        string folderName = gameObject.name + "Track"; // assume the sound folder is named after the child object
        clips = Resources.LoadAll<AudioClip>(folderName);   
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; 
        animator = GetComponent<Animator>();
        if (particleNotes != null)
        {
            particleNotes.Stop();
        }
            
        
    }
    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
        if (particleNotes != null)
        {
            if (state == true)
            {
                particleNotes.Play();
                
            }
            else
            {
                particleNotes.Stop();
            }
        }
    }
    
    public void DestroyAnimation(bool destroy)
    {
        animator.SetBool("Destroyed", destroy);
    }
    
    public void CorrectAnimation(bool correct)
    {
        animator.SetBool("Correct", correct);
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void Play()
    {
        Debug.Log("Playing");
        audioSource.Play();
    }
    public void SetClip(int clip = 0)
    {
        if ( clips.Length > 0)
        {
            audioSource.clip = clips[clip];
        }
    }

    public AudioClip GetClip(int clip)
    {
        return clips[clip];
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
    
}
