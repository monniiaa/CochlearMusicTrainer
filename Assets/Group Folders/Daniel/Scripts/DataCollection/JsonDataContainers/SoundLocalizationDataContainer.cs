using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundLocalizationDataContainer : AbstractDataContainer
{
    public override string Path => "\\SoundLocalization\\sound_localization_data.json";

    public float distance;
    public float hello;

    public SoundLocalizationDataContainer(float distance, float hello)
    {
        this.distance = distance;
        this.hello = hello;
    }
}


