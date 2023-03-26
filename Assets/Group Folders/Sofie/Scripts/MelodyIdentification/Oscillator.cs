using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    private float gain = 0;
    private float volume = 0.05f;
    public float[] frequencies { get; private set; }
    private int thisFreq;
    public int startFreq { get; private set; }


    private void Awake()
    {
        frequencies = new float[14];
        frequencies[0] = 523.25f;
        frequencies[1] = 587.33f;
        frequencies[2] = 659.25f;
        frequencies[3] = 698.46f;
        frequencies[4] = 783.99f;
        frequencies[5] = 880.00f;
        frequencies[6] = 987.77f;

        frequencies[7] = 1046.50f;
        frequencies[8] = 1174.66f;
        frequencies[9] = 1318.51f;
        frequencies[10] = 1396.91f;
        frequencies[11] = 1567.98f;
        frequencies[12] = 1760.00f;
        frequencies[13] = 1975.53f;

    }


    public void CreateStartNote()
    {
        int rand = Random.Range(0, frequencies.Length);
        startFreq = rand;
        
    }

    public void CreateStartNote(int i)
    {
        startFreq = i;
    }
    public void PlayMelody()
    {
        StartCoroutine(PlaySequence(DiscriminationManager.levelInterval, DiscriminationManager.levelSequenceLength, DiscriminationManager.levelNoteTime));
      //  StartCoroutine(PlaySequence(2,4,0.4f));
    }


    IEnumerator PlaySequence(int interval, int length, float noteTime)
    {
        thisFreq = startFreq;
        int i = 0;
        do
        {
            if(thisFreq >= 7)
            {
                volume = 0.025f;
            }
            frequency = frequencies[thisFreq];
            thisFreq += interval;
            thisFreq = thisFreq % frequencies.Length;
            i++;
            yield return new WaitForSeconds(noteTime);

        } while (i < length);

        SetNotePlaying(false);
    }

    public void SetNotePlaying(bool on)
    {
        if (on)
        {
            gain = volume;
        } 
        else
        {
            gain = 0;
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

        for (int i =0;i<data.Length;i+= channels)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));

            if(channels == 2)
            {
                data[i + 1] = data[i];
            }
            if(phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }
}
