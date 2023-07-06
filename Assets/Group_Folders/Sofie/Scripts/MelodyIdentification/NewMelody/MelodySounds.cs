using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodySounds 
{
    private static MelodySounds _instance;
    
    public static MelodySounds Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new MelodySounds();
            }

            return _instance;
        }
    }

    
    
    public List<AudioClip> LoadRandomEasyMelodyPair(int sequenceLength)
    {
        List<AudioClip> returnClips = new List<AudioClip>();
        switch (sequenceLength)
        {
            case 2:
                int rand2 = UnityEngine.Random.Range(1, 10);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Easy/pair" + rand2 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Easy/pair" + rand2 + "/note2"));
                break;
            case 3:
                int rand3 = UnityEngine.Random.Range(1, 9);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length3/Easy/pair" + rand3 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length3/Easy/pair" + rand3 + "/note2"));
                break;
            case 4:
                int rand4 = UnityEngine.Random.Range(1, 6);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Easy/pair" + rand4 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Easy/pair" + rand4 + "/note2"));
                break;
            case 5:
                int rand5 = UnityEngine.Random.Range(1, 7);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length5/Easy/pair" + rand5 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length5/Easy/pair" + rand5 + "/note2"));
                break;
        }

        return returnClips;
    }

    public List<AudioClip> LoadRandomMediumMelodyPair(int sequenceLength)
    {
        List<AudioClip> returnClips = new List<AudioClip>();
        switch (sequenceLength)
        {
            case 2:
                int rand2 = UnityEngine.Random.Range(1, 4);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Medium/pair" + rand2 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Medium/pair" + rand2 + "/note2"));
                break;
            case 3:
                int rand3 = UnityEngine.Random.Range(1, 4);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length3/Medium/pair" + rand3 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length3/Medium/pair" + rand3 + "/note2"));
                break;
            case 4:
                int rand4 = UnityEngine.Random.Range(1, 6);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Medium/pair" + rand4 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Medium/pair" + rand4 + "/note2"));
                break;
            case 5:
                int rand5 = UnityEngine.Random.Range(1, 7);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length5/Medium/pair" + rand5 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length5/Medium/pair" + rand5 + "/note2"));
                break;
        }

        return returnClips;
    }
    
    public List<AudioClip> LoadRandomHardMelodyPair(int sequenceLength)
    {
        List<AudioClip> returnClips = new List<AudioClip>();
        switch (sequenceLength)
        {
            case 2:
                int rand2 = UnityEngine.Random.Range(1, 3);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Hard/pair" + rand2 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length2/Hard/pair" + rand2 + "/note2"));
                break;
            case 3:
                int rand3 = UnityEngine.Random.Range(1, 3);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length3/Hard/pair" + rand3 + "/note1"));
                returnClips.Add( Resources.Load<AudioClip>("MelodySounds/Length3/Hard/pair" + rand3 + "/note2"));
                break;
            case 4:
                int rand4 = UnityEngine.Random.Range(1,3);
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Hard/pair" + rand4 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length4/Hard/pair" + rand4 + "/note2"));
                break;
            case 5:
                int rand5 = UnityEngine.Random.Range(1, 6);
                returnClips.Add( Resources.Load<AudioClip>("MelodySounds/Length5/Hard/pair" + rand5 + "/note1"));
                returnClips.Add(Resources.Load<AudioClip>("MelodySounds/Length5/Hard/pair" + rand5 + "/note2"));
                break;
        }

        return returnClips;
    }
}
