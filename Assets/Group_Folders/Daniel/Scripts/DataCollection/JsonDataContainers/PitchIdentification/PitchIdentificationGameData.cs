using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PitchIdentificationGameData : AbstractDataContainer
{
    public override string Path => "/PitchIdentification/pitch_identification_data.json";
    
    public string time;
    public string timeTakenToChooseSpeaker;
    public string chosenClip;
    public string correctClip;
    public bool wasCorrect;
    public int level;
    public int round;

    public PitchIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseSpeaker, string chosenClip, string correctClip, bool wasCorrect, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseSpeaker = timeTakenToChooseSpeaker.ToString("h'h 'm'm 's's'");;
        this.chosenClip = chosenClip;
        this.correctClip = correctClip;
        this.wasCorrect = wasCorrect;
        this.level = level;
        this.round = round;
    }

}
