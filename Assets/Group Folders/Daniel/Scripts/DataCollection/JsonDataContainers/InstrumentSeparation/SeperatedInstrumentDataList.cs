using System;
using System.Collections.Generic;

[Serializable]
public struct SeperatedInstrumentDataList
{
    public List<SeparatedInstrumentData> seperatedInstrumentDataList;

    public SeperatedInstrumentDataList(List<SeparatedInstrumentData> seperatedInstrumentDataList)
    {
        this.seperatedInstrumentDataList = seperatedInstrumentDataList;
    }
}
