using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class MelodyIdentificationGameData : AbstractDataContainer
{
    public override string Path => "/MelodyIdentification/melody_identification_data.json";
    
    public string name;
    public float clicks;
    public float level;
    public string melodyName;
    public bool instr;
    public bool mel;
    public bool haptics;

    public MelodyIdentificationGameData(string name, float clicks, int level, string melodyName, bool similarInstruments, bool sameMelody, bool haptics)
    {
        this.name = name;
        this.clicks = clicks;
        this.level = level;
        this.melodyName = melodyName;
        this.instr = similarInstruments;
        this.mel = sameMelody;
        this.haptics = haptics;
    }

}
