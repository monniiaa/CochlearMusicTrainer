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
    private Coroutine _coroutine;

    private float startVolume = 1;

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
        _coroutine = StartCoroutine(FadeOut(2f));
        
        
    }
    public void PlayMelody()
    {
        StartCoroutine(PlaySequence(DiscriminationManager.levelInterval, DiscriminationManager.levelSequenceLength, DiscriminationManager.levelNoteTime));
    }


    private IEnumerator PlaySequence(int interval, int length, float noteTime)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            source.volume = startVolume;
        }
        currentNote = startNote;
        currentClip = notes[currentNote];
        int i = 0;
        do
        {
            currentClip = notes[currentNote];
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
    public IEnumerator FadeOut(float FadeTime)
    {

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }
}
