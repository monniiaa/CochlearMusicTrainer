using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstrumentPicker : MonoBehaviour
{
    [SerializeField] private GameObject[] instrumentFamilies;
    
    public List<InstrumentBehavior> instrumentsPlayingSound = new List<InstrumentBehavior>();
    public HapticsBehavior instrumentPlayingHaptics;
    
    private List<InstrumentBehavior> previousSoundCombination = new List<InstrumentBehavior>();
    private HapticsBehavior previousHaptics;

    private int roundClip;

    private void Start()
    {
        instrumentFamilies = new GameObject[4];
        instrumentFamilies[0] = GameObject.Find("Brass");
        instrumentFamilies[1] = GameObject.Find("Woodwind");
        instrumentFamilies[2] = GameObject.Find("Percussion");
        instrumentFamilies[3] = GameObject.Find("Strings");
    }

    /// <summary>
    /// Chooses random instruments to play.
    /// </summary>
    /// <param name="numberOfInstruments">Should be equal to or smaller than amount of instruments in scene ideally 1-5</param>
    /// <param name="sameFamily">Should instruments from the same family spawn?</param>
    public void PickInstrumentsPlayingSound(int numberOfInstruments)
    {
        ResetInstrumentSoundCombination();
        Debug.Log(previousSoundCombination.Count);
        foreach (InstrumentBehavior i in previousSoundCombination)
        {
            Debug.Log(i.name);
        }

        GameObject fam;
        InstrumentBehavior pickedInstrument1;
        int rand;
        int randI;
        do
        {
            rand = Random.Range(0, instrumentFamilies.Length);
            fam = instrumentFamilies[rand];
            randI = Random.Range(0, fam.GetComponent<Family>().instruments.Count);
            pickedInstrument1 = fam.GetComponent<Family>().instruments[randI].GetComponent<InstrumentBehavior>();
        }while (previousSoundCombination.Contains(pickedInstrument1));
        
        
        List<InstrumentBehavior> pickedInstruments = new List<InstrumentBehavior>();
        List<GameObject> pickedFamily = new List<GameObject>();
        pickedInstruments.Add(pickedInstrument1);
        pickedFamily.Add(fam);

            for (int i = 1; pickedInstruments.Count < numberOfInstruments; i++)
            {
                int randFamily;
                GameObject fam1;

                randFamily = Random.Range(0, instrumentFamilies.Length);
                fam1= instrumentFamilies[randFamily];


                pickedFamily.Add(fam1);
                    
                int randInstrument = Random.Range(0, pickedFamily[pickedFamily.Count -1 ].GetComponent<Family>().instruments.Count);
                if(!pickedInstruments.Contains(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>()))pickedInstruments.Add(pickedFamily[pickedFamily.Count - 1].GetComponent<Family>().instruments[randInstrument].GetComponent<InstrumentBehavior>());
            }
            instrumentsPlayingSound = pickedInstruments;
    }

    public void PlayPickedInstrumentsSound()
    {
        roundClip = 0; //Random.Range(0,6);
        for (int i = 0; i < instrumentsPlayingSound.Count; i++)
        {
            AudioClip clip = instrumentsPlayingSound[i].GetClip(roundClip);
            instrumentsPlayingSound[i].PlayClip(clip);
        }
    }
    
    
    public void PickHapticsInstrument(bool HapticsOnly)
    {
        ResetInstrumentHaptics();
        if (!HapticsOnly)
        {
            int rand = UnityEngine.Random.Range(0, instrumentsPlayingSound.Count);
            instrumentPlayingHaptics = instrumentsPlayingSound[rand].GetComponent<HapticsBehavior>();
        }
        else
        {
            GameObject fam;
            HapticsBehavior pickedInstrument;
            int rand;
            int randI;
            do
            {
                rand = Random.Range(0, instrumentFamilies.Length);
                fam = instrumentFamilies[rand];
                randI = Random.Range(0, fam.GetComponent<Family>().instruments.Count);
                pickedInstrument = fam.GetComponent<Family>().instruments[randI].GetComponent<HapticsBehavior>();
            }while (previousHaptics == pickedInstrument);

            instrumentPlayingHaptics = pickedInstrument;
        }
    }
    
    public void PlayPickedInstrumentHaptics()
    {
        instrumentPlayingHaptics.PlayHapticsClip(roundClip);
    }

    public void PickInstrumentsPlayingSoundAndHaptics(int amount)
    {
        PickInstrumentsPlayingSound(amount);
        PickHapticsInstrument(false);
    }
    
    public void StopInstrumentsPlaying(bool hapticsOn)
    {
        foreach (InstrumentBehavior instrument in instrumentsPlayingSound)
        {
            instrument.StopAudio();
        }

        if (hapticsOn)
        {
            instrumentPlayingHaptics.StopHaptics();
        }
    }

    public void ResetInstruments()
    {
        for (int i = 0; i < instrumentFamilies.Length; i++)
        {
            foreach (GameObject instrument in instrumentFamilies[i].GetComponent<Family>().instruments)
            {
                instrument.GetComponent<InstrumentBehavior>().SetPickedState(false);
                instrument.GetComponent<InstrumentBehavior>().DestroyAnimation(false);
            }
        }
    }

    private void ResetInstrumentSoundCombination()
    {
        previousSoundCombination.Clear();
        foreach (InstrumentBehavior instrument in instrumentsPlayingSound)
        {
            previousSoundCombination.Add(instrument);
        }
        instrumentsPlayingSound.Clear();
    }

    private void ResetInstrumentHaptics()
    {
        previousHaptics = instrumentPlayingHaptics;
        instrumentPlayingHaptics = null;
    }
    
    public void ResetSoundAndHapticsCombination()
    {
        ResetInstrumentSoundCombination();
        ResetInstrumentHaptics();
    }
    
}
