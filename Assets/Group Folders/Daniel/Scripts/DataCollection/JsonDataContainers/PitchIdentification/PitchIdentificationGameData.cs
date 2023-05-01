using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PitchIdentificationGameData : AbstractDataContainer
{
    public override string Path => "\\InstrumentIdentification\\instrument_identification_game_data.json";
    
    public string time;
    public string timeTakenToChooseSpeaker;
    public int chosenNote;
    public int correctNote;
    public string[] noteOptions;
    public int level;
    public int round;

    public PitchIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseSpeaker, int chosenNote, int correctNote, string[] noteOptions, int level)
    {
        this.time = time.ToString("dd/MM/yy H:mm:ss");
        this.timeTakenToChooseSpeaker = timeTakenToChooseSpeaker.ToString("H:mm:ss");
        this.chosenNote = chosenNote;
        this.correctNote = correctNote;
        this.noteOptions = noteOptions;
        this.level = level;
    }

}
