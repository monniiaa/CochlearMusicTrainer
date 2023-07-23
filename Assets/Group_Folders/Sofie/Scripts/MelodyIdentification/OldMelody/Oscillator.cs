using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public string direction;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

    }

    private void Start()
    {
        ChooseRandomMelodyDirection();
    }


    public void ChooseRandomMelodyDirection()
    {
        CreateStartNote();
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            StartCoroutine(PlayUpgoingMelody(4,1f,3));
        }
        else
        {
            StartCoroutine(PlayDowngoingMelody(4,1f,3));
        }
    }
    IEnumerator PlayUpgoingMelody(int interval, float noteTime, int length)
    {
        direction = "Upgoing";
        while(startNote + interval * length > notes.Length)
        {
            CreateStartNote();
        }
        currentNote = startNote;
        currentClip = notes[currentNote];
        int i = 0;
        do
        {
            currentClip = notes[currentNote];
            source.PlayOneShot(currentClip);
            currentNote += interval;
            i++;
            yield return new WaitForSeconds(noteTime);

        } while (i < length && currentNote < notes.Length);

        source.Stop();
    }

    IEnumerator PlayDowngoingMelody(int interval, float noteTime, int length)
    {
        direction = "Downgoing";
        while(startNote + interval * length > notes.Length)
        {
            CreateStartNote();
        }
        currentNote = startNote;
        currentClip = notes[currentNote];
        int i = 0;
        do
        {
            currentClip = notes[currentNote];
            source.PlayOneShot(currentClip);
            currentNote -= interval;
            i++;
            Debug.Log(currentClip.name);
            yield return new WaitForSeconds(noteTime);

        } while (i < length && currentNote < notes.Length);

        source.Stop();
    }

    public void CreateStartNote()
    {
        int rand = Random.Range(0, notes.Length);
        startNote = rand;
        currentClip = notes[startNote];
    }

    public void CreateStartNote(int i)
    {
        startNote = i;
        currentClip = notes[startNote];
    }
    
    public void StopMelody()
    {
        source.Stop();
        StopAllCoroutines();
    }
    public void PlayMelody()
    {
        StartCoroutine(PlaySequence(DiscriminationManager.levelInterval, DiscriminationManager.levelSequenceLength, DiscriminationManager.levelNoteTime));
    }


    private IEnumerator PlaySequence(int interval, int length, float noteTime)
    {
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
