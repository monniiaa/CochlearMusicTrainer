using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundLocalizationHardDataContainer : AbstractDataContainer
{
    public override string Path => "/SoundLocalization/sound_localization_Hard_data.json";
    public string timeTakenToGuess;
    public string time;
    public int level;
    public int round;
    public float distanceFromPlayer;
    public float angleFromRay;
    public bool wasHit;
    public string instrument;
    
    
    
    public SoundLocalizationHardDataContainer(bool wasHit, DateTime time, TimeSpan timeTakenToGuess, string instrument, float distanceFromPlayer, float angleFromRay,  int level, int round)
    {
        this.wasHit = wasHit;
        this.timeTakenToGuess = timeTakenToGuess.ToString("h'h 'm'm 's's'");
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.instrument = instrument;
        this.distanceFromPlayer = distanceFromPlayer;
        this.angleFromRay = angleFromRay;
        this.level = level;
        this.round = round;
        
    }

}
