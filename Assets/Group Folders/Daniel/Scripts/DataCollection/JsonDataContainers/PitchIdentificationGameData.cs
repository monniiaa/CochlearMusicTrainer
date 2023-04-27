using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PitchIdentificationGameData : AbstractDataContainer
{
    public override string Path => "\\InstrumentIdentification\\instrument_identification_game_data.json";

    public TimeSpan[] timeTakenToChooseSpeaker;
    public int[] correctChooses;
    public int level;

    public PitchIdentificationGameData(TimeSpan[] timeTakenToChooseSpeakers, int[] correctChooses, int level)
    {
        this.timeTakenToChooseSpeaker = timeTakenToChooseSpeakers;
        this.correctChooses = correctChooses;
        this.level = level;
    }

}
