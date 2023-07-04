using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Family : MonoBehaviour
{
    public FamilyType familyType;
    public List<GameObject> instruments;

    [SerializeField] private GameObject[] _instruments;
    
    public void ResetInstrumentsInFamily()
    {
        instruments.Clear();
        for(int i = 0; i < _instruments.Length  ; i++)
        {
            instruments.Add(_instruments[i]);
        }
    }

    private void Awake()
    {
       ResetInstrumentsInFamily();
    }
}

public enum FamilyType
{
    String,
    Woodwind,
    Brass,
    Percussion,
    Keyboard,
    Voice
}
