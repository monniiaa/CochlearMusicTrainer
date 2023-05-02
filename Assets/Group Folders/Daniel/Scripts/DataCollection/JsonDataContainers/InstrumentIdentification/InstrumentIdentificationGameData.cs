using System;

[Serializable]
public class InstrumentIdentificationGameData : AbstractDataContainer
{
    public override string Path => "/InstrumentIdentification/instrument_identification_data.json";

    public string time;
    public string timeTakenToChooseInstrument;
    public string chosenInstrument;
    public string correctInstrument;
    public bool wasCorrect;
    public string[] instrumentOptions;
    public int level;
    public int round;

    public InstrumentIdentificationGameData(DateTime time, TimeSpan timeTakenToChooseInstrument, string chosenInstrument, string correctInstrument, bool wasCorrect, string[] instrumentOptions, int level, int round)
    {
        this.time = time.ToString("dd/MM/yyyy HH:mm:ss");
        this.timeTakenToChooseInstrument = timeTakenToChooseInstrument.ToString("h'h 'm'm 's's'");;
        this.chosenInstrument = chosenInstrument;
        this.correctInstrument = correctInstrument;
        this.wasCorrect = wasCorrect;
        this.instrumentOptions = instrumentOptions;
        this.level = level;
        this.round = round;
    }

}
