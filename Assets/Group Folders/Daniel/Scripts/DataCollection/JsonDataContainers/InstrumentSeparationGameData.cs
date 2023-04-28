using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InstrumentSeparationGameData : AbstractDataContainer
{
     public override string Path => "\\InstrumentSeparation\\instrument_separation_game_data.json";

    public float[] distances;

    public InstrumentSeparationGameData(float[] distances)
    {
        this.distances = distances;
    }

    public InstrumentSeparationGameData()
    {
    }
}
