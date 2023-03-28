
using System;
using System.Collections.Generic;

public interface IMiniGameData
{
    string FileName { get; }
    string Path { get; }
    string CsvColumns { get; }
    int Score { get; }

    string ToCsv();
}

public readonly struct TimbreIdentification : IMiniGameData
{
    public TimbreIdentification(TimeSpan time, int score)
    {
        Time = time;
        Score = score;
        Path = "/TimbreIdentification/";
        CsvColumns = "Time,Score";
        FileName = "instrument_separation_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }
    public TimeSpan Time { get; }

    public string ToCsv()
    {
        return $"{Time},{Score}";
    }
}

public readonly struct InstrumentSeparationData : IMiniGameData
{

    public InstrumentSeparationData(int score, int level, Dictionary<string, float> instrumentSeparation)
    {
        Score = score;
        Level = level;
        InstrumentSeparation = instrumentSeparation;
        Path = "/InstrumentSeparation/";
        CsvColumns = "Score,Level";
        FileName = "instrument_separation_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }
    public int Level { get; }
    
    public string ToCsv()
    {
        return $"{Score},{Level},";
    }

    public readonly Dictionary<string, float> InstrumentSeparation;
    
}

public readonly struct PitchIdentificationData : IMiniGameData
{
    
    public PitchIdentificationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/PitchIdentification/";
        CsvColumns = "Date,Score,Level,Hello";
        FileName = "pitch_identification_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }
    public int Level { get; }
    public string ToCsv()
    {
        return "";
    }
}

public readonly struct MelodyIdentificationData : IMiniGameData
{
    public MelodyIdentificationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/MelodyIdentification/";
        CsvColumns = "Date,Score,Level,Melody";
        FileName = "melody_identification_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }

    public int Level { get; }
    public string ToCsv()
    {
        return $"{Score},{Level}";
    }
}

public readonly struct SoundLocalizationData : IMiniGameData
{
    
    public SoundLocalizationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/SoundLocalization/";
        CsvColumns = "Date,Score,Level";
        FileName = "sound_localization_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }
    public int Level { get; }
    public string ToCsv()
    {
        return $"{DateTime.Now},{Level}";
    }
}

public struct InstrumentIdentificationData : IMiniGameData
{
    
    public InstrumentIdentificationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/SoundLocalization/";
        CsvColumns = "Date,";
        FileName = "instrument_identification_data.csv";
    }

    public string FileName { get; }
    public string Path { get; }
    public string CsvColumns { get; }
    public int Score { get; }
    public int Level { get; }
    public string ToCsv() => $"{Score},{Level}";
}