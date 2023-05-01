using System;
using System.Collections.Generic;

[Serializable]
public struct SeparatedInstrumentData
{
    public string instrumentName;
    public bool canHear;
    public float distanceToPlayer;
    public List<OtherInstrumentDistances> distances;

    public SeparatedInstrumentData(string instrumentName, bool canHear, float distanceToPlayer, List<OtherInstrumentDistances> distances)
    {
        this.instrumentName = instrumentName;
        this.canHear = canHear;
        this.distanceToPlayer = distanceToPlayer;
        this.distances = distances;
    }
}
