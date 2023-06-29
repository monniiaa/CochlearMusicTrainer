using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundLocalizationDataContainer : AbstractDataContainer
{
    public override string Path => "/SoundLocalization/sound_localization_data.json";
    public bool wasHit;
    public string timeTakenToGuess;
    public string time;
    public float distanceFromPlayer;
    public float angleFromRay;
    public int level;
    public int round;

    public SoundLocalizationDataContainer(DateTime time, TimeSpan timeTakenToGuess, bool wasHit, float distanceFromPlayer, float angleFromRay, int level, int round)
    {
        this.wasHit = wasHit;
        this.timeTakenToGuess = timeTakenToGuess.ToString("h'h 'm'm 's's'");
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.distanceFromPlayer = distanceFromPlayer;
        this.angleFromRay = angleFromRay;
        this.level = level;
        this.round = round;
    }
}


