using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundLocalizationDataContainer : AbstractDataContainer
{
    public override string Path => "\\SoundLocalization\\sound_localization_data.json";
    public bool wasHit;
    public string time;
    public float distanceFromPlayer;
    public float angleFromPlayer;
    public int level;
    public int round;

    public SoundLocalizationDataContainer(DateTime time, bool wasHit, float distanceFromPlayer, float angleFromPlayer, int level)
    {
        this.wasHit = wasHit;
        this.time = time.ToString("dd/MM/yy H:mm:ss");
        this.distanceFromPlayer = distanceFromPlayer;
        this.angleFromPlayer = angleFromPlayer;
        this.level = level;
    }
}


