using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstrumentsCanvas : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI[] correctInstrumentsText;
    public void SetCorrectInstrumentsText(List<InstrumentBehavior> instrumentsPlaying)
    {
        if (instrumentsPlaying.Count > 0)
        {
            titleText.text = "De korrekte instrumenter var";
        }
        else if (instrumentsPlaying.Count == 0)
        {
            titleText.text = "Det korrekte instrument var";
        }
        for (int i = 0; i < instrumentsPlaying.Count; i++)
        {
            correctInstrumentsText[i].text = instrumentsPlaying[i].gameObject.name;
        }
    }

    private void ResetText()
    {
        titleText.text = string.Empty;
        for (int i = 0; i < correctInstrumentsText.Length; i++)
        {
            correctInstrumentsText[i].text = string.Empty;
        }
    }

    private void OnDisable()
    {
        ResetText();
    }
}
