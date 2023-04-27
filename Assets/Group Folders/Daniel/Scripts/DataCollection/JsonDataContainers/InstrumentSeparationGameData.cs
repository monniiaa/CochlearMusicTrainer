using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentSeparationGameData : AbstractDataContainer
{
    public override string Path => "\\InstrumentSeparation\\instrument_identification_game_data.json";

    public float[] distances;

    public InstrumentSeparationGameData(float[] distances)
    {
        this.distances = distances;
    }
}
