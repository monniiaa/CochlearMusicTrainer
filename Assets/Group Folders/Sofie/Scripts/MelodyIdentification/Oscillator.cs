using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    public AudioClip[] notes;
    public AudioSource source;
    public int startNote;
    public int currentNote;
    public AudioClip currentClip;

    public Material mat;

    public Animator animator;


    private void Awake()
    {
        source = GetComponent<AudioSource>();

    }


    public void CreateStartNote()
    {
        int rand = Random.Range(0, notes.Length);
        startNote = rand;
        
    }

    public void CreateStartNote(int i)
    {
        startNote = i;
    }
    
    public void StopMelody()
    {
        StopAllCoroutines();
        source.Stop();
    }
    public void PlayMelody()
    {
        StartCoroutine(PlaySequence(DiscriminationManager.levelInterval, DiscriminationManager.levelSequenceLength, DiscriminationManager.levelNoteTime));
      //  StartCoroutine(PlaySequence(2,4,0.4f));
    }


    IEnumerator PlaySequence(int interval, int length, float noteTime)
    {
        currentNote = startNote;
        currentClip = notes[currentNote];
        int i = 0;
        do
        {
            currentClip = notes[currentNote];
            Debug.Log(currentClip.name);
            source.PlayOneShot(currentClip);
            currentNote += interval;
            currentNote = currentNote % notes.Length;
            i++;
            yield return new WaitForSeconds(noteTime);

        } while (i < length);

        source.Stop();
    }

    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }

    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");

    }

}
