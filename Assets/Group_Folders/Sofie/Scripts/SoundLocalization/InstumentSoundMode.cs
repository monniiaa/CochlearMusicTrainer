using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.VisualScripting;

public class InstumentSoundMode : MonoBehaviour
{
    private RandomInstrumentPicker randomInstrumentPicker;
    private randomizeSoundLocation randomizeSoundLocation;
    private List<GameObject> pickedInstruments;
    private GameObject instrumentToLocate;
    
    private InstrumentNameUI instrumentNameUI;


    private void Awake()
    {
        randomInstrumentPicker = GetComponent<RandomInstrumentPicker>();
        randomizeSoundLocation = GetComponent<randomizeSoundLocation>();
        instrumentNameUI = GameObject.FindObjectOfType<InstrumentNameUI>();
    }
    
    private void Start()
    {
        StartRound();
    }

    private void StartRound()
    {
        pickedInstruments = randomInstrumentPicker.PickedInstruments(2);
        SetInstrumentToLocate();
        randomizeSoundLocation.SpawnInstruments(3, pickedInstruments.ToArray());
        
    }

    private void SetInstrumentToLocate()
    {
        int rand = Random.Range(0, pickedInstruments.Count);
        instrumentToLocate = pickedInstruments[rand];
        if (instrumentToLocate.GetComponent<MeshRenderer>() != null)
        {
            instrumentToLocate.GetComponent<MeshRenderer>().enabled = false;
        }
        
        /*if(instrumentToLocate.GetComponentInChildren<GameObject>() != null)
        {
            instrumentToLocate.GetComponentInChildren<GameObject>().SetActive(false);
        }*/

        instrumentNameUI.SetText(instrumentToLocate.name);
    }

    private void OnDisable()
    {
        if (instrumentToLocate.GetComponent<MeshRenderer>() != null)
        {
            instrumentToLocate.GetComponent<MeshRenderer>().enabled = true;
        }
        
        /*
        if(instrumentToLocate.GetComponentInChildren<GameObject>() != null)
        {
            instrumentToLocate.GetComponentInChildren<GameObject>().SetActive(true);
        }*/
    }
}
