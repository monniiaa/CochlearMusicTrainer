using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class MelodyIdentificationGameData : AbstractDataContainer
{
    public override string Path => "\\MelodyIdentification\\melody_identification_data.json";

    public string time;
    public string timeTakenToChooseSpeaker;
    public string melodyReference;
    public string chosenMelody;
    public bool wasCorrect;
    public string[] melodyOptions;
    public int level;
    public int round;

    public MelodyIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseSpeaker, string melodyReference, string chosenMelody, bool wasCorrect, string[] melodyOptions, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseSpeaker = timeTakenToChooseSpeaker.ToString("h'h 'm'm 's's'");;
        this.melodyReference = melodyReference;
        this.chosenMelody = chosenMelody;
        this.wasCorrect = wasCorrect;
        this.melodyOptions = melodyOptions;
        this.level = level;
        this.round = round;
    }

}
