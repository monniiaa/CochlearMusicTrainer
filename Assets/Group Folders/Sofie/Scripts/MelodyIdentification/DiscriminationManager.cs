using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscriminationManager : LevelManager
{
    private Oscillator[] melodies;
    public static int levelInterval { get; private set; }
    public static int levelSequenceLength { get; private set; }
    public static float levelNoteTime { get; private set; }
    private int intervalDistance = 4;
    private Oscillator originalMelody;

    public AudioSource gameplayAudio;
    public AudioClip sucessAudio;
    public AudioClip failAudio;
    [SerializeField]
    Material sucessMaterial;
    [SerializeField]
    Material failMaterial;

    private Oscillator pickedMelody;

    private void Start()
    {
        melodies = GameObject.FindObjectsOfType<Oscillator>();
        path = "PitchDiscrimination";
        //   SetDifficultyChanges();
        // SetRoundFunctionality();
        StartRound();
        gameData = DataManager.ReadJson(path);
        currentLevel = gameData.level;
        SetDifficultyChanges();

    }

    
    public void SetOriginalMelody()
    {
        foreach(Oscillator melody in melodies)
        {
            if (melody.gameObject.CompareTag("Original"))
            {
                originalMelody = melody;
                melody.CreateStartNote();
            }
        }
    }

    public void SetEquivalentMelody()
    {
        int rand;
        do
        {
            rand = Random.Range(0, melodies.Length);
        } while (melodies[rand].CompareTag("Original"));

        melodies[rand].CreateStartNote(originalMelody.startFreq);
        melodies[rand].gameObject.tag = "Equivalent";
    }

    public void SetDissimilarMelodies()
    {
        foreach(Oscillator melody in melodies)
        {
            if (!melody.gameObject.CompareTag("Original") && !melody.CompareTag("Equivalent"))
            {
                do
                {
                    melody.CreateStartNote();
                } while (melody.startFreq < originalMelody.startFreq + intervalDistance && melody.startFreq > originalMelody.startFreq - intervalDistance
                && melody.startFreq <= (originalMelody.startFreq + intervalDistance) % originalMelody.frequencies.Length);
            }
        }
    }

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                levelInterval = Random.Range(3, 5);
                levelSequenceLength = Random.Range(1, 3);
                break;
            case Difficulty.Medium:
                levelInterval = Random.Range(1, 2);
                levelSequenceLength = Random.Range(2, 4);
                break;
            case Difficulty.Hard:
                levelInterval = Random.Range(1, 2);
                levelSequenceLength = Random.Range(2, 4);
                break;
        }
        intervalDistance = Random.Range(0, 2);
        levelNoteTime = 0.4f;
    }

    public void PickMelody(Oscillator pickedMelody)
    {
        this.pickedMelody = pickedMelody;
    }
    protected override void EndRound()
    {
        if (pickedMelody.startFreq == originalMelody.startFreq)
        {
            Debug.Log(pickedMelody.name + pickedMelody.startFreq);
            Debug.Log(originalMelody.name + originalMelody.startFreq);
            gameplayAudio.PlayOneShot(sucessAudio);
            pickedMelody.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore++;
            gameData.levelScore[currentLevel - 1] = currentScore;
            pickedMelody.tag = "Untagged";
            pickedMelody = null;
        }
        else 
        {
            gameplayAudio.PlayOneShot(failAudio);
            pickedMelody.GetComponent<MeshRenderer>().material = failMaterial;
            pickedMelody = null;
        }
    }

    protected override void StartRound()
    {
        SetDifficultyChanges();
        SetOriginalMelody();
        SetEquivalentMelody();
        SetDissimilarMelodies();

    }

    public override void SetRoundFunctionality()
    {
        round++;
        EndRound();
       if(round < 4)
        {
            
            StartRound();
        }
        else
        {
           // currentLevel++;
          //  gameData.level = currentLevel;
          //  DataManager.SaveDataToJson(gameData, path);
        }
    }
}
