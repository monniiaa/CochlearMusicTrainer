using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiscriminationManager : LevelManager
{
    private Oscillator[] melodies;
    public static int levelInterval { get; private set; }
    public static int levelSequenceLength { get; private set; }
    public static float levelNoteTime { get; private set; }
    private int intervalDistance = 4;
    private Oscillator originalMelody;

    private Oscillator pickedMelody;
    public InstrumentSeparation ModeManager;
    private GameDataManager _gameDataManager;
    private DateTime startTime;
    
    private void Awake() 
    {
        _gameDataManager = GameDataManager.Instance;
    }

    private void Start()
    {
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        ModeManager = InstrumentSeparation.Instance;
        melodies = GameObject.FindObjectsOfType<Oscillator>();
        path = "PitchDiscrimination";
        StartRound();
        gameData = DataManager.ReadJson(path);
        SetDifficultyChanges();
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }

    protected override void RestartLevel()
    {
        round = 1;
        currentScore = 0;
        foreach (Oscillator osc in melodies)
        {
            osc.gameObject.SetActive(true);
        }
        StartRound();
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
            rand = UnityEngine.Random.Range(0, melodies.Length);
        } while (melodies[rand].CompareTag("Original"));

        melodies[rand].CreateStartNote(originalMelody.startNote);
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
                } while (melody.startNote < originalMelody.startNote + intervalDistance && melody.startNote > originalMelody.startNote - intervalDistance
                && melody.startNote <= (originalMelody.startNote + intervalDistance) % originalMelody.notes.Length);
            }
        }
    }

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                levelInterval = UnityEngine.Random.Range(3, 5);
                levelSequenceLength = UnityEngine.Random.Range(2, 4);
                break;
            case Difficulty.Medium:
                levelInterval = UnityEngine.Random.Range(1, 2);
                levelSequenceLength = UnityEngine.Random.Range(3, 5);
                break;
            case Difficulty.Hard:
                levelInterval = UnityEngine.Random.Range(1, 2);
                levelSequenceLength = UnityEngine.Random.Range(3, 6);
                break;
        }
        intervalDistance = UnityEngine.Random.Range(0, 2);
        levelNoteTime = 0.4f;
    }

    public void PickMelody(Oscillator pickedMelody)
    {
        this.pickedMelody = pickedMelody;
    }
    protected override void EndRound()
    {
       /* JsonManager.WriteDataToFile<MelodyIdentificationGameData>(
            new MelodyIdentificationGameData(
                startTime,
                DateTime.Now - startTime,
                originalMelody.currentClip.name,
                pickedMelody.currentClip.name,
                originalMelody.startNote == pickedMelody.startNote,
                new string[] { melodies[0].currentClip.name, melodies[1].currentClip.name, melodies[2].currentClip.name },
                currentLevel,
                round
            ));*/
        
        foreach (Oscillator osc in melodies)
        {
            osc.DestroyAnimation();
            
        }
        if (pickedMelody.startNote == originalMelody.startNote)
        {
            gameplayAudio.PlayOneShot(sucessAudio);
            pickedMelody.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore++;
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
        StartCoroutine(test());
        SetDifficultyChanges();
        SetOriginalMelody();
        SetEquivalentMelody();
        SetDissimilarMelodies();
        startTime = DateTime.Now;
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.7f);
        foreach (Oscillator osc in melodies)
        {
            osc.GetComponent<MeshRenderer>().material = osc.mat;
        }
    }

    IEnumerator End()
    {
        JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Melody Identification", DateTime.Now, currentScore, currentLevel));
        yield return new WaitForSeconds(0.7f);
        foreach (Oscillator osc in melodies)
        {
            osc.gameObject.SetActive(false);
        }
        ModeManager.EndGame();
        ShowStar(currentScore);
    }

    private void ShowStar(int score)
    {
        for (int i = 0; i < starAnimation.Length; i++)
        {
            if(i == score) 
            {
                starAnimation[i].SetActive(true);
                continue;
            }
            starAnimation[i].SetActive(false);
        }
    }

    public override void SetRoundFunctionality()
    {
        EndRound();
        round++;
       if(round < 4)
        {
            StartRound();
        }
        else
        {
           StartCoroutine(End());
           if(currentLevel == gameData.level)
           {
               gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
           }           

           if(gameData.levelScore[currentLevel - 1] < currentScore)
           {
                gameData.levelScore[currentLevel - 1] = currentScore;
           }

           DataManager.SaveDataToJson(gameData, path);
        }
    }
}
