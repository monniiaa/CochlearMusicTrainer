using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongStem", menuName = "ScriptableObjects/SongStem", order = 3)]
public class SongStem : ScriptableObject
{
    public InstrumentData instrumentData;
    public AudioClip audioClip;
}
