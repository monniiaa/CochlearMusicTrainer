using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstrumentIntroduction : MonoBehaviour
{
    public GameObject[] AllInstruments;
    public TMP_Text[] texts;
    private int currentIndex = 0;
    private int currentTextIndex = 0;

    private void Awake()
    {
        
    }

    // Activate the next game object in the array
    public void ActivateNext()
    {
        int nextIndex = (currentIndex + 1) % AllInstruments.Length;  // Calculate next index
        AllInstruments[nextIndex].SetActive(true);  // Activate next game object
        AllInstruments[currentIndex].SetActive(false);  // Deactivate current game object
        currentIndex = nextIndex;  // Set current index to next index

        int nextTextIndex = (currentTextIndex + 1) % texts.Length;  // Calculate next index
        texts[nextTextIndex].gameObject.SetActive(true);  // Activate next game object
        texts[currentTextIndex].gameObject.SetActive(false);  // Deactivate current game object
        currentTextIndex = nextTextIndex;  // Set current index to next index
        Debug.Log(currentIndex);
    }

    public void Instrument(GameObject item)
    {
        item.SetActive(true);        
    }
}
