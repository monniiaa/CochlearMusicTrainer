using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentBehavior : MonoBehaviour
{
    public Animator animator;

    public AudioSource audioSource;
    [SerializeField]
    private ParticleSystem particleNotes;
    
    // Start is called before the first frame update
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
    
    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");
    }
    
    public void CorrectAnimation(bool correct)
    {
        animator.SetBool("Correct", correct);
    }

    public void Play()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Debug.Log("Playing " + audioSource.clip.name);
    }
    public void SetClip(int clip = 0)
    {
        string folderName = gameObject.name + "Track"; // assume the sound folder is named after the child object
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderName);   
        if ( clips.Length > 0)
        {
            if (clip < 0 || clip > clips.Length)
            {
                audioSource.clip = clips[0];
            }
            else
            {
                audioSource.clip = clips[clip];
            }
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
