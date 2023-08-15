using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InstrumentNameUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetText(string txt)
    {
        text.text = txt;
    }
}
