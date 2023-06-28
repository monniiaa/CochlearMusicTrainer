using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentBehavior : MonoBehaviour
{
    public Animator animator;

    private AudioSource audioSource;

    public AudioClip[] songs;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; 
        audioSource.Stop();
        animator = GetComponent<Animator>();
        
    }
    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }
    
    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");
    }

    public void PlaySong(int song)
    {
        audioSource.PlayOneShot(songs[song]);
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
