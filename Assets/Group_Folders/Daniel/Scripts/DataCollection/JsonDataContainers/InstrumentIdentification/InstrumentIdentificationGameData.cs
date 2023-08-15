using System;

[Serializable]
public class InstrumentIdentificationGameData : AbstractDataContainer
{
    public override string Path => "/InstrumentIdentification/instrument_identification_data.json";

    public string time;
    public string timeTakenToChooseInstruments;
    public string[] chosenInstruments;
    public string[] correctInstruments;
    public bool wasCorrect;
    public int level;
    public int round;

    public InstrumentIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseInstrument, string[] chosenInstruments, string[] correctInstruments, bool wasCorrect, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseInstruments = timeTakenToChooseInstrument.ToString("h'h 'm'm 's's'");;
        this.chosenInstruments = chosenInstruments;
        this.correctInstruments = correctInstruments;
        this.wasCorrect = wasCorrect;
        this.level = level;
        this.round = round;
    }

}
