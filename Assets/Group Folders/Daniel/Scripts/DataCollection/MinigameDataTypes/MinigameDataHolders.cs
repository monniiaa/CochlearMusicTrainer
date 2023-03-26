
using System.Collections.Generic;

internal interface IMiniGameData
{
    string Path { get; }
    int Score { get; }
    int Level { get; }

    string ToCsv();
}

public readonly struct InstrumentSeparationData : IMiniGameData
{

    public InstrumentSeparationData(int score, int level, Dictionary<string, float> instrumentSeparation)
    {
        Score = score;
        Level = level;
        InstrumentSeparation = instrumentSeparation;
        Path = "/InstrumentSeparation/instrument_separation_data.csv";
    }

    public string Path { get; }
    public int Score { get; }
    public int Level { get; }
    
    public string ToCsv()
    {
        
    }

    public readonly Dictionary<string, float> InstrumentSeparation;
    
}

public readonly struct PitchIdentificationData : IMiniGameData
{
    
    public PitchIdentificationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/PitchIdentification/pitch_identification_data.csv";
    }

    public string Path { get; }
    public int Score { get; }
    public int Level { get; }
    public string ToCsv()
    {
        
    }
}

public readonly struct MelodyIdentificationData : IMiniGameData
{
    public MelodyIdentificationData(int score, int level)
    {
        Score = score;
        Level = level;
        Path = "/MelodyIdentification/melody_identification_data.csv";
    }

    public string Path { get; }
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
        Path = "/SoundLocalization/sound_localization_data.csv";
    }

    public const string FileName = "SoundLocalization";
    public string Path { get; }
    public int Score { get; }
    public int Level { get; }
    public string ToCsv()
    {
        return $"{Score},{Level}";
    }
}

public struct InstrumentIdentificationData
{
    public int Score { get; }
    
}