using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public Animator animator;

    public float frequency1 = 0;

    //Creates a slider in the inspector
    public float frequency2 = 0;

    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;

    public float amplitude;

    AudioSource audioSource;
    int timeIndex = 0;
    public bool playing = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        animator = GetComponent<Animator>();
        SetPickedState(false);

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);

            if (channels == 2)
                data[i + 1] = CreateSine(timeIndex, frequency2, sampleRate);

            timeIndex++;

            //if timeIndex gets too big, reset it to 0
            if (timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
    }

    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return amplitude * (Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate));
    }

    public void PlayAudio()
    {
  
        audioSource.Play();
        playing = true;
    }
    public void ResetFrequency()
    {
        frequency1 = 0;
        frequency2 = 0;
    }

    public void StopAudio()
    {
        audioSource.Stop();
        playing = false;
    }


    private void OnDisable()
    {
        StopAudio();
    }


    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }

    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");

    }

    public void DestroySpeaker()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }
}
