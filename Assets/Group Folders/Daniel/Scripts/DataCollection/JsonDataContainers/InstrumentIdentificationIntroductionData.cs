using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InstrumentIdentificationIntroductionData : AbstractDataContainer
{
    public override string Path => "\\InstrumentIdentification\\instrument_identification_introduction_data.json";
    public TimeSpan timeTakenToComplete;
    public string[] playedInstruments;

    public InstrumentIdentificationIntroductionData(TimeSpan timeTakenToComplete, string[] playedInstruments)
    {
        this.timeTakenToComplete = timeTakenToComplete;
        this.playedInstruments = playedInstruments;
    }
}
