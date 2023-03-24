using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscriminationManager : MonoBehaviour
{
    private Oscillator[] melodies;
    private int levelInterval;
    private int levelSequenceLength;
    private int levelNoteTime;
    private int intervalDistance = 2;
    private Oscillator originalMelody;

    private void Start()
    {
        melodies = GameObject.FindObjectsOfType<Oscillator>();
        SetOriginalMelody();
        SetEquivalentMelody();
        SetDissimilarMelodies();
    }

    
    public void SetOriginalMelody()
    {
        foreach(Oscillator melody in melodies)
        {
            if (melody.gameObject.CompareTag("Original"))
            {
                originalMelody = melody;
                melody.CreateStartNote();
            }
        }
    }

    public void SetEquivalentMelody()
    {
        int rand;
        do
        {
            rand = Random.Range(0, melodies.Length);
        } while (melodies[rand].GetComponent<Melody>().isOriginal);

        melodies[rand].CreateStartNote(originalMelody.startFreq);
        melodies[rand].gameObject.tag = "Equivalent";
    }

    public void SetDissimilarMelodies()
    {
        foreach(Oscillator melody in melodies)
        {
            if (!melody.gameObject.CompareTag("Original") && !melody.CompareTag("Equivalent"))
            {
                do
                {
                    melody.CreateStartNote();
                } while (melody.startFreq < originalMelody.startFreq + intervalDistance && melody.startFreq > originalMelody.startFreq - intervalDistance
                && melody.startFreq <= (originalMelody.startFreq + intervalDistance) % originalMelody.frequencies.Length);
            }
        }
    }
}
