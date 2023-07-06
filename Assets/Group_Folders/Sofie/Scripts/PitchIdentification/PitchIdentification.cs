using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Mono.CSharp;
using Unity.VisualScripting;

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
    [SerializeField]
    private Timer timer;
    private bool OutOfTime = false;
    [SerializeField] private Material material;
    private float timerStart;


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

        foreach (Speaker speaker in speakers)
        {
            speaker.SetPickedState(false);
        }
        StartRound();
        
    }

    private void Start()
    {
        timer.timeLeft = timerStart;
        timer.StartCoroutine(timer.Reset());
    }

    private void Update()
    {
        if (difficulty == Difficulty.Hard)
        {
            Debug.Log(timer.timeLeft);
            if (timer.timeLeft <= 0)
            {
                foreach (Speaker s in speakers)
                {
                    s.SwitchMaterial(failMaterial, material);
                }

                if (round < 4)
                {
                    gameplayAudio.PlayOneShot(failAudio);
                }
                EndRound();
                OutOfTime = true;
                SetRoundFunctionality();
                
            }
        }
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
        timer.timeLeft = timerStart;
        timer.StartCoroutine(timer.Reset());
        
    }

    public void SetPitchDifference( int interval)
    {
        highestSpeaker = speakers[0];
        if (speakers.Length > 2)
        {
            for (int i = 0; i < speakers.Length; i++)
            {
                int randNote = UnityEngine.Random.Range(0, speakers[0].notes.Length);
                speakers[i].note = randNote;
                speakers[i].SetNote(randNote);
                for (int j = 0; j < speakers.Length; j++)
                {
                    if (j != i)
                    {
                        {
                            while (speakers[i].note == speakers[j].note)
                            {
                                int upperbound = Mathf.Min(randNote + interval, speakers[i].notes.Length- 1);
                                int lowerBound = randNote - interval;
                                int rand;
                                if (lowerBound < 0)
                                {
                                    
                                    lowerBound = speakers[i].notes.Length + lowerBound;
                                    if (lowerBound > upperbound)
                                    {
                                        (upperbound, lowerBound) = (lowerBound, upperbound);
                                    }
                                }
                                rand = UnityEngine.Random.Range(lowerBound, upperbound);
                                speakers[i].note = rand;
                                speakers[i].SetNote(rand);
                            } 
                        }
                    }
                }
                if (speakers[i].note > highestSpeaker.note)
                {
                    highestSpeaker = speakers[i];
                }
            }
        }
        else if (speakers.Length == 2)
        {
            int randnote = UnityEngine.Random.Range(0, speakers[0].notes.Length);
            speakers[0].note = randnote;
            speakers[0].SetNote(randnote);

            int rand = UnityEngine.Random.Range(0, 2);

            if (rand == 1)
            {
                int note = (randnote + interval) % speakers[1].notes.Length;
                speakers[1].note = note;
                speakers[1].SetNote(note);
            }
            else
            {
                int note = randnote - interval;
                if (note < 0)
                {
                    Debug.Log(note);
                    note = speakers[1].notes.Length + note;
                }
                speakers[1].note = note;
                speakers[1].SetNote(note);
            }

            if (speakers[1].note > speakers[0].note)
            {
                highestSpeaker = speakers[1];
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
                    new string[] { speakers[0].currentClip.name, speakers[1].currentClip.name },
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
                
                SetPitchDifference( 10 - currentLevel);
                break;
            case Difficulty.Medium:
                SetPitchDifference( 7-currentLevel);
                break;
            case Difficulty.Hard:
                SetPitchDifference( 1);
                timer = FindObjectOfType<Timer>(true);
                timer.gameObject.SetActive(true);
                timerStart = 15-currentLevel;
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
            OutOfTime = false;
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
        OutOfTime = false;
        startTime = DateTime.Now;
        if (timer != null && round != 1)
        {
            timer.timeLeft = timerStart;
            timer.StartCoroutine(timer.Reset());
        }
        foreach (Speaker speaker in speakers)
        {
            speaker.SetPickedState(false);
        }
        SetDifficultyChanges();
    }
}
