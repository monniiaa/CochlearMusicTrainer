using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstrumentData", menuName = "ScriptableObjects/InstrumentData", order = 1)]
public class InstrumentData : ScriptableObject
{
    public Instrument instrument;
    //public InstrumentFamily instrumentFamily;
    public GameObject instrumentPrefab;
}
