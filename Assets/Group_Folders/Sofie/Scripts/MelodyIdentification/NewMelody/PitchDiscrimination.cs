using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PitchDiscrimination : LevelManager
{
    private MelodySpeaker[] melodies;
    public static int sequenceLength { get; private set; }
    private MelodySpeaker originalMelody;

    private MelodySpeaker pickedMelody;
    private MelodySounds melodySounds;
    public InstrumentSeparation ModeManager;
    private GameDataManager _gameDataManager;
    private DateTime startTime;

    private List<AudioClip> melodiesForRound;
    
    private void Awake() 
    {
        _gameDataManager = GameDataManager.Instance;
        melodySounds = MelodySounds.Instance;
        SetMode();
    }

    private void Start()
    {
        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        ModeManager = InstrumentSeparation.Instance;
        melodies = GameObject.FindObjectsOfType<MelodySpeaker>();
        path = "PitchDiscrimination";
        Debug.Log(difficulty);
        originalMelody = GameObject.FindWithTag("Original").GetComponent<MelodySpeaker>();
        
        StartRound();
        gameData = DataManager.ReadJson(path);

        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }

    protected override void RestartLevel()
    {
        round = 1;
        currentScore = 0;
        foreach (MelodySpeaker speaker in melodies)
        {
            speaker.gameObject.SetActive(true);
        }
        StartRound();
    }
    
    public void SetOriginalMelody()
    {
        int rand = UnityEngine.Random.Range(0, melodiesForRound.Count);
        originalMelody.currentClip = melodiesForRound[rand];
        originalMelody.SetClip(originalMelody.currentClip);
        melodiesForRound.RemoveAt(rand);
            
        
    }

    public void SetEquivalentMelody()
    {
        int rand;
        do
        {
            rand = UnityEngine.Random.Range(0, melodies.Length);
        } while (melodies[rand] == originalMelody);
        melodies[rand].currentClip = originalMelody.currentClip;
        melodies[rand].SetClip(originalMelody.currentClip);
        melodies[rand].gameObject.tag = "Equivalent";
    }

    public void SetDissimilarMelody()
    {
        Debug.Log(melodiesForRound.Count);
        for (int i = 0; i < melodies.Length; i++)
        {
            if (!melodies[i].CompareTag("Equivalent") && !melodies[i].CompareTag("Original"))
            {
                melodies[i].currentClip = melodiesForRound[0];
                melodies[i].SetClip(melodiesForRound[0]);
            }
                
        }
    }
    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                sequenceLength = UnityEngine.Random.Range(2, 4);
                melodiesForRound = melodySounds.LoadRandomEasyMelodyPair(sequenceLength);
                break;
            case Difficulty.Medium:
                sequenceLength = UnityEngine.Random.Range(3, 5);
                melodiesForRound = melodySounds.LoadRandomMediumMelodyPair(sequenceLength);
                break;
            case Difficulty.Hard:
                sequenceLength = UnityEngine.Random.Range(4, 6);
                melodiesForRound = melodySounds.LoadRandomHardMelodyPair(sequenceLength);
                break;
        }
    }

    public void PickMelody(MelodySpeaker melody)
    {
        pickedMelody = melody;
    }
    protected override void EndRound()
    {
        JsonManager.WriteDataToFile<MelodyIdentificationGameData>(
            new MelodyIdentificationGameData(
                startTime,
                DateTime.Now - startTime,
                originalMelody.currentClip.name,
                pickedMelody.currentClip.name,
                originalMelody.currentClip.name == pickedMelody.currentClip.name,
                new string[] { melodies[0].currentClip.name, melodies[1].currentClip.name, melodies[2].currentClip.name },
                currentLevel,
                round
            ));
        melodiesForRound.Clear();
        foreach (MelodySpeaker speaker in melodies)
        {
            speaker.DestroyAnimation();
            
        }
        if (pickedMelody.currentClip.name == originalMelody.currentClip.name)
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
        SetDissimilarMelody();
        startTime = DateTime.Now;
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.7f);
        foreach (MelodySpeaker speaker in melodies)
        {
            speaker.GetComponent<MeshRenderer>().material = speaker.mat;
        }
    }

    IEnumerator End()
    {
        JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Melody Identification", DateTime.Now, currentScore, currentLevel));
        yield return new WaitForSeconds(0.7f);
        foreach (MelodySpeaker speaker in melodies)
        {
            speaker.gameObject.SetActive(false);
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
