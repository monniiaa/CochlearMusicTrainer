using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SongData", menuName = "ScriptableObjects/SongData", order = 2)]
public class SongData : ScriptableObject
{
    public string songName;
    public SongStem[] stems;
    public Vector2 startTime;
    public Vector2 endTime;
}
