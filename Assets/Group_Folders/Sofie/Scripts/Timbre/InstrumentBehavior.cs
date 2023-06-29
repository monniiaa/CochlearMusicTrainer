using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstrumentBehavior : MonoBehaviour
{
    public Animator animator;

    private AudioSource audioSource;
    [SerializeField]
    private ParticleSystem particleNotes;

    public AudioClip[] songs;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; 
        audioSource.Stop();
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

    public void PlayClip()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void SetClip(int clip)
    {
        string folderName = gameObject.name + "Track"; // assume the sound folder is named after the child object
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderName);
        if (clips != null && clips.Length > 0)
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
