using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstrumentsCanvas : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI[] correctInstrumentsText;
    [SerializeField] private GameObject star;

    private void Awake()
    {
        star.SetActive(false);
    }

    public void SetCorrectInstrumentsText(List<InstrumentBehavior> instrumentsPlaying, bool correct, bool numberofinstrumentsmode)
    {
        if (!numberofinstrumentsmode)
        {
            if (instrumentsPlaying.Count > 1)
            {
                titleText.text = "De korrekte instrumenter var";
            }
            else if (instrumentsPlaying.Count == 1)
            {
                titleText.text = "Det korrekte instrument var";
            }
        }
        else
        {
            titleText.text = "Antallet af instrumenter var " + instrumentsPlaying.Count.ToString(); ;
        }
        for (int i = 0; i < instrumentsPlaying.Count; i++)
        {
            correctInstrumentsText[i].text = instrumentsPlaying[i].name;
        }
        if (correct)
        {
            star.SetActive(true);
        }
    }
    
    public void SetTextColor(int index, Color color)
    {
        correctInstrumentsText[index].color = color;
    }

    private void ResetText()
    {
        titleText.text = string.Empty;
        for (int i = 0; i < correctInstrumentsText.Length; i++)
        {
            correctInstrumentsText[i].text = string.Empty;
        }
        star.SetActive(false);
    }

    private void OnDisable()
    {
        ResetText();
    }

    public void SetNextButtonBehavior(bool end)
    {
        if (!end)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => FindObjectOfType<TimbreManager>().CanvasEnd());
            nextButton.onClick.AddListener(() => FindObjectOfType<TimbreManager>().SetRoundFunctionality());
        }
        else
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => FindObjectOfType<TimbreManager>().End());
        }
    }
}
