using System;

[Serializable]
public class InstrumentIdentificationGameData : AbstractDataContainer
{
    public override string Path => "/InstrumentIdentification/instrument_identification_data.json";

    public string time;
    public string timeTakenToChooseInstruments;
    public string chosenInstruments;
    public bool sameFamily;
    public bool wasCorrect;
    public int level;
    public int round;

    public InstrumentIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseInstrument, string chosenInstrument,bool sameFamily, bool wasCorrect, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseInstruments = timeTakenToChooseInstrument.ToString("h'h 'm'm 's's'");;
        this.chosenInstruments = chosenInstrument;
        this.sameFamily = sameFamily;
        this.wasCorrect = wasCorrect;
        this.level = level;
        this.round = round;
    }

}
