using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * From https://forum.unity.com/threads/generating-a-simple-sinewave.471529/
 */
public class ToneGenerator : MonoBehaviour
{
      //Creates a slider in the inspector
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
        
    }
   
    void Update()
    {

    }
   
    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i = 0; i < data.Length; i+= channels)
        {          
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);
           
            if(channels == 2)
                data[i+1] = CreateSine(timeIndex, frequency2, sampleRate);
           
            timeIndex++;
           
            //if timeIndex gets too big, reset it to 0
            if(timeIndex >= (sampleRate * waveLengthInSeconds))
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

    public void StopAudio()
    {
        audioSource.Stop();
        playing = false;
    }


    private void OnDisable()
    {
        StopAudio();
    }
}
