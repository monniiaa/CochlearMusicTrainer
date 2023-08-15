using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PitchInstruments 
{
    public static AudioClip[] previousPicks = new AudioClip[2];
    public static  AudioClip[] LoadRandomInstrumentPair()
    {
        AudioClip[] returnClips = new AudioClip[2];
        int rand;
        do
        {
            rand = UnityEngine.Random.Range(1, 9);
            returnClips[0] = Resources.Load<AudioClip>("Voice_Countervoice/Instrument" + rand + "/version1");
            returnClips[1] = Resources.Load<AudioClip>("Voice_Countervoice/Instrument" + rand + "/version2");
        } while(previousPicks.Contains(returnClips[0]));

        return returnClips;
    }

    public static AudioClip[] LoadRandomSongPair()
    {
        AudioClip[] returnClips = new AudioClip[2];
        int rand;
        do
        {
            rand = UnityEngine.Random.Range(1, 5);
            returnClips[0] = Resources.Load<AudioClip>("SongsPitch/Song" + rand + "/version1");
            returnClips[1] = Resources.Load<AudioClip>("SongsPitch/Song" + rand + "/version2");
        } while(previousPicks.Contains(returnClips[0]));
        
        return returnClips;
    }
}
