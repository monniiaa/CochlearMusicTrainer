using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentContainer
{
    public string path { get; set; }
    public string instrument { get; set; }
    public List<string> melodies { get; set; }
    public int group { get; set; }

    public InstrumentContainer(string path, string instrument, List<string> melodies, int group)
    {
        this.path = path;
        this.instrument = instrument;
        this.melodies = melodies;
        this.group = group;
    }
}
