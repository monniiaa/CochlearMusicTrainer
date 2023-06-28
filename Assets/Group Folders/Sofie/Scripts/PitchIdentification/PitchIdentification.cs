using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PitchIdentification : LevelManager
{
    private int maxLevel = 4;

    private Speaker highestSpeaker;
    private bool correctPick;
    private GameDataManager _gameDataManager;

    public GameObject speakerPrefab;
    public Vector3[] initialPositions;
    [SerializeField]

    Speaker[] speakers;
    public InstrumentSeparation ModeManager;
    private DateTime startTime;


    private void Awake()
    {
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;

        gameplayAudio = GetComponent<AudioSource>();
        path = "PitchIdentification";
        gameData =  DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        SetMode();
        speakers = GameObject.FindObjectsOfType<Speaker>();
        initialPositions = new Vector3[speakers.Length];
        for( int i = 0; i < speakers.Length;i++)
        {
            initialPositions[i] = speakers[i].transform.position;
        }

        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
        StartRound();
    }
    
    protected override void RestartLevel()
    {
        currentScore = 0;
        round = 1;
        foreach (Speaker s in speakers)
        {
            s.gameObject.SetActive(true);
        }
        StartRound();
    }

    public void SetPitchDifference(int lowestNote, int highestNote , int interval)
    {
        highestSpeaker = speakers[0];
        for (int i = 0; i < speakers.Length; i++)
        {
            int randNote = UnityEngine.Random.Range(lowestNote, highestNote);
            speakers[i].note = randNote;
            speakers[i].SetNote(randNote);
            for (int j = 0; j < speakers.Length;j++)
            {
                if (j != i)
                {
                    
                    while (  speakers[i].note > speakers[j].note - interval 
                    && speakers[i].note < speakers[j].note + interval)
                    {
                        int rand= UnityEngine.Random.Range(lowestNote, highestNote);
                        speakers[i].note = rand;
                        speakers[i].SetNote(rand);
                    }
                    
                }
            }
            if (speakers[i].note > highestSpeaker.note)
            {
                highestSpeaker = speakers[i];
            }
        }
    }
    public void SpeakerPicked(Speaker speaker)
    {
        JsonManager.WriteDataToFile<PitchIdentificationGameData>(
            new PitchIdentificationGameData(
                DateTime.Now,
                DateTime.Now - startTime,
                speaker.currentClip.name,
                highestSpeaker.currentClip.name,
                speaker == highestSpeaker,
                new string[] { speakers[0].currentClip.name, speakers[1].currentClip.name},
                currentLevel,
                round
            )
        );
        EndRound();
        if (speaker == highestSpeaker) {
            gameplayAudio.PlayOneShot(sucessAudio);
            speaker.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore += 1;

        } else if (speaker!= highestSpeaker)
        {
            gameplayAudio.PlayOneShot(failAudio);
            speaker.GetComponent<MeshRenderer>().material = failMaterial;
        }
       
    }

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                SetPitchDifference(5, 25, 6);
                break;
            case Difficulty.Medium:
                SetPitchDifference(5, 25, 3);
                break;
            case Difficulty.Hard:
                SetPitchDifference(0, 30, 1);
                break;
            default:
                break;
        }
    }

    IEnumerator End()
    {
        JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Pitch Identification", DateTime.Now, currentScore, currentLevel));
        yield return new WaitForSeconds(0.7f);
        foreach (Speaker s in speakers)
        {
            s.gameObject.SetActive(false);
        }
        ModeManager.EndGame();
        ShowStar(currentScore);
    }

    public override void SetRoundFunctionality()
    {
        round++;
        if (round < 4)
        {
            StartRound();
        }
        else
        {
            StartCoroutine(End());
            if(currentLevel == gameData.level)
            {
                gameData.level += 1;
            }          
            if(gameData.levelScore[currentLevel - 1] < currentScore)
            {
                    gameData.levelScore[currentLevel - 1] = currentScore;
            }
            DataManager.SaveDataToJson(gameData, path);  
        }
    }

    protected override void EndRound()
    {
        foreach (Speaker s in speakers)
        {
            s.ResetCurrentNote();
            s.DestroyAnimation();
        }
    }

    protected override void StartRound()
    {
        startTime = DateTime.Now;
        foreach (Speaker speaker in speakers)
        {
            speaker.SetPickedState(false);
        }
        SetDifficultyChanges();
    }
}
