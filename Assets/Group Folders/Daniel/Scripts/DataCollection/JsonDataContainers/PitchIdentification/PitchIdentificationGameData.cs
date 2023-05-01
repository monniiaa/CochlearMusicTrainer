using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PitchIdentificationGameData : AbstractDataContainer
{
    public override string Path => "\\PitchIdentification\\pitch_identification_data.json";
    
    public string time;
    public string timeTakenToChooseSpeaker;
    public string chosenNote;
    public string correctNote;
    public bool wasCorrect;
    public string[] noteOptions;
    public int level;
    public int round;

    public PitchIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseSpeaker, string chosenNote, string correctNote, bool wasCorrect, string[] noteOptions, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseSpeaker = timeTakenToChooseSpeaker.ToString("h'h 'm'm 's's'");;
        this.chosenNote = chosenNote;
        this.correctNote = correctNote;
        this.wasCorrect = wasCorrect;
        this.noteOptions = noteOptions;
        this.level = level;
        this.round = round;
    }

}
