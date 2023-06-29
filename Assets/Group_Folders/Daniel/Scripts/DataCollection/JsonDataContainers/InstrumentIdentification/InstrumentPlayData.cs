using System;

[Serializable]
public struct InstrumentPlayData
{
    public string instrumentName;
    public int timesPlayed;

    public InstrumentPlayData(string instrumentName, int timesPlayed)
    {
        this.instrumentName = instrumentName;
        this.timesPlayed = timesPlayed;
    }
}
