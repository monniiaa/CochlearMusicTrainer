using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundLocalizationDataContainer : AbstractDataContainer
{
    public override string Path => "/SoundLocalization/sound_localization_data.json";
    public string timeTakenToGuess;
    public string time;
    public bool correctTarget;
    public int amountOfTargets;
    public int level;
    public int round;
    
    
    
    public SoundLocalizationDataContainer(DateTime time, TimeSpan timeTakenToGuess, bool correctTarget, int amountOfTargets, int level, int round)
    {
        this.correctTarget = correctTarget;
        this.timeTakenToGuess = timeTakenToGuess.ToString("h'h 'm'm 's's'");
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.amountOfTargets = amountOfTargets;
        this.level = level;
        this.round = round;
        
    }
    
}


