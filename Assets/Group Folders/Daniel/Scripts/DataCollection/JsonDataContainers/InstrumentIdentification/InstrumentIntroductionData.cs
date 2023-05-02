using System;
using System.Collections.Generic;

[Serializable]
public class InstrumentIntroductionData : AbstractDataContainer
{
    public override string Path => "\\InstrumentIdentification\\instrument_introduction_data.json";
    public string time;
    public string timeUsed;
    public List<InstrumentPlayData> playedInstruments;

    public InstrumentIntroductionData(DateTime time, TimeSpan timeUsed, List<InstrumentPlayData> playedInstruments)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeUsed = timeUsed.ToString("h'h 'm'm 's's'");
        this.playedInstruments = playedInstruments;
    }

    
}
