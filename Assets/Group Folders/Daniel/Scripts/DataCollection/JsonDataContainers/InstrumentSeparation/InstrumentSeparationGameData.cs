using System;
using System.Collections.Generic;

[Serializable]
public class InstrumentSeparationGameData : AbstractDataContainer
{
    public override string Path => "\\InstrumentSeparation\\instrument_separation_game_data.json";


    public string musicPlayed;
    public int level;
    public List<SeparatedInstrumentData> separatedInstrumentDataList;
    
    public InstrumentSeparationGameData(string musicPlayed, int level, List<SeparatedInstrumentData> separatedInstrumentDataList)
    {
        this.musicPlayed = musicPlayed;
        this.level = level;
        this.separatedInstrumentDataList = separatedInstrumentDataList;
    }

    public InstrumentSeparationGameData()
    {
    }
}
