using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InstrumentIntroduction : MonoBehaviour
{
    public GameObject[] AllInstruments;
    public TMP_Text[] texts;
    private int currentIndex = 0;
    private int currentTextIndex = 0;
    [SerializeField] GameObject startGameButton;

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

        if (currentIndex >= AllInstruments.Length-1)
        {
            startGameButton.SetActive(true);
        }
    }

    public void Instrument(GameObject item)
    {
        item.SetActive(true);        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TrainingEnvironmentTest");
    }
}
