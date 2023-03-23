using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain = 0;
    public float volume = 0.1f;
    public float[] frequencies;
    public int thisFreq;

    private void Start()
    {
        frequencies = new float[7];
        frequencies[0] = 523.25f;
        frequencies[1] = 587.33f;
        frequencies[2] = 659.25f;
        frequencies[3] = 698.46f;
        frequencies[4] = 783.99f;
        frequencies[5] = 880.00f;
        frequencies[6] = 987.77f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetNotePlaying(true);
            CreateStartNote();
            StartCoroutine(PlaySequence(4, 7));
        } 
        
    }

    public void CreateStartNote()
    {
        int thisFreq = Random.Range(0, 7);
        
    }


    IEnumerator PlaySequence(int interval, int length)
    {
        int i = 0;
        do
        {
            frequency = frequencies[thisFreq];
            thisFreq += interval;
            thisFreq = thisFreq % frequencies.Length;
            i++;
            yield return new WaitForSeconds(1f);

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
