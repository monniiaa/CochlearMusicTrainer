using System;

[Serializable]
public struct OtherInstrumentDistances
{
    public string instrumentName;
    public float distance;
    public OtherInstrumentDistances(string instrumentName, float distance)
    {
        this.instrumentName = instrumentName;
        this.distance = distance;
    }
}
