using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Family : MonoBehaviour
{
    public FamilyType familyType;
    public List<GameObject> instruments;

    [SerializeField] private List<GameObject> _instruments;

    public void ResetInstrumentsInFamily()
    {
        instruments = _instruments;
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
