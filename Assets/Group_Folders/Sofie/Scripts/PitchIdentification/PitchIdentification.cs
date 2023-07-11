using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using Mono.CSharp;
using Unity.VisualScripting;

public class PitchIdentification : LevelManager
{
    private int maxLevel = 10;

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

    private bool SongsMode = false;
    
    private MelodySpeaker[] melodySpeakers;
    [SerializeField] private List<GameObject> pitchModes;

    private void Awake()
    {
        pitchModes = new List<GameObject>();
        _gameDataManager = GameDataManager.Instance;
        ModeManager = InstrumentSeparation.Instance;

        gameplayAudio = GetComponent<AudioSource>();
        path = "PitchIdentification";
        gameData =  DataManager.ReadJson(path);

        currentLevel = (_gameDataManager.currentLevel == 0) ? 1 : _gameDataManager.currentLevel;
        SetMode();
        initialPositions = new Vector3[speakers.Length];
        for( int i = 0; i < speakers.Length;i++)
        {
            initialPositions[i] = speakers[i].transform.position;
        }

        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }

    }

    private void Start()
    {       
        
        pitchModes.Add(GameObject.Find("ToneMode"));
        pitchModes.Add( GameObject.Find("SongsMode"));
        if (currentLevel <= 2 || currentLevel > 4 && currentLevel < 10)
        {
            SongsMode = false;
            pitchModes[0].gameObject.SetActive(true);
            pitchModes[1].gameObject.SetActive(false);
        } else if (currentLevel == 3 || currentLevel == 4 || currentLevel == 10)
        {
            SongsMode = true;
            pitchModes[0].gameObject.SetActive(false);
            pitchModes[1].gameObject.SetActive(true);
        }
        
        StartRound();
        timer.timeLeft = timerStart;
        if(currentLevel > 6 && currentLevel < 10) timer.StartCoroutine(timer.Reset());
    }

    private void Update()
    {
        if (difficulty == Difficulty.Hard)
        {
            if (currentLevel < 10)
            {
                if (timer.timeLeft <= 0 && OutOfTime == false)
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
    }
    


    protected override void RestartLevel()
    {
        currentScore = 0;
        round = 1;
        if (!SongsMode)
        {
            foreach (Speaker s in speakers)
            {
                s.gameObject.SetActive(true);
                s.ResetMat();
            }
        }
        else
        {
            foreach (MelodySpeaker speaker in melodySpeakers)
            {
                speaker.gameObject.SetActive(true);
            }
        }
        StartRound();
        timer.timeLeft = timerStart;
        if(currentLevel > 6 && currentLevel < 10) timer.StartCoroutine(timer.Reset());
        
    }

    public void SetPitchDifference( int upperInterval, int lowerinterval)
    {
        highestSpeaker = speakers[0];
        int randnote = UnityEngine.Random.Range(0, speakers[0].notes.Length);
        speakers[0].note = randnote;
        speakers[0].SetNote(randnote);
        
        List<int> randomNotes = new List<int>();
        randomNotes.Add(randnote);
        for (int i = 1; i < speakers.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, 2);

                if (rand == 1)
                {
                    int randInterval;
                    int note;
                    do
                    {
                        randInterval = UnityEngine.Random.Range(lowerinterval, upperInterval);
                        note = (randnote + randInterval) % speakers[1].notes.Length;
                        speakers[i].note = note;
                        speakers[i].SetNote(note);
                    } while (randomNotes.Contains(note));
                    randomNotes.Add(note);
                }
                else
                {
                    int randInterval;
                    int note;

                    do
                    {
                        randInterval = UnityEngine.Random.Range(lowerinterval, upperInterval);
                        note = randnote - randInterval;
                        if (note < 0)
                        {
                            note = speakers[i].notes.Length + note;
                        }
                        speakers[i].note = note;
                        speakers[i].SetNote(note);
                    }while (randomNotes.Contains(note));
                    randomNotes.Add(note);
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
                if (currentLevel <= 2)
                {
                    speakers = GameObject.FindObjectsOfType<Speaker>();
                    SetPitchDifference( 10 , 6- currentLevel);
                }
                else if (currentLevel == 3)
                {
                    melodySpeakers = GameObject.FindObjectsOfType<MelodySpeaker>();
                    SetRoundInstrumentVersions();
                }
                break;
            case Difficulty.Medium:
                if (currentLevel == 4)
                {
                    melodySpeakers = GameObject.FindObjectsOfType<MelodySpeaker>();
                    SetRoundInstrumentVersions();
                }
                else
                {
                    speakers = GameObject.FindObjectsOfType<Speaker>();
                    SetPitchDifference( 11-currentLevel, 1);
                }
                break;
            case Difficulty.Hard:
                if (currentLevel <= 9)
                {
                    speakers = GameObject.FindObjectsOfType<Speaker>();
                    SetPitchDifference( 12-currentLevel, 1);
                    timer = FindObjectOfType<Timer>(true);
                    timer.gameObject.SetActive(true);
                    timerStart = 15 - currentLevel;
                }
                else
                {
                    melodySpeakers = GameObject.FindObjectsOfType<MelodySpeaker>();
                    SetRoundInstrumentVersions();
                }

                break;
        }

    }

    IEnumerator End()
    {
       JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Pitch Identification", DateTime.Now, currentScore, currentLevel));
        yield return new WaitForSeconds(0.7f);

        if (!SongsMode)
        {
            foreach (Speaker s in speakers)
            {
                s.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (MelodySpeaker speaker in melodySpeakers)
            {
                speaker.gameObject.SetActive(false);
            }
        }
        ModeManager.EndGame();
        ShowStar(currentScore);
    }

    public override void SetRoundFunctionality()
    {
        EndRound();
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
        if (!SongsMode)
        {
            foreach (Speaker s in speakers)
            {
                s.ResetCurrentNote();
                s.DestroyAnimation();
            }
        }
        else
        {
            foreach (MelodySpeaker speaker in melodySpeakers)
            {
                speaker.currentClip = null;
                speaker.DestroyAnimation();
            }
        }
    }

    protected override void StartRound()
    {
        OutOfTime = false;
        startTime = DateTime.Now;
        if (timer != null && round != 1 && currentLevel > 6 && currentLevel < 10)
        {
            timer.timeLeft = timerStart;
            timer.StartCoroutine(timer.Reset());
        }

        SetDifficultyChanges();
        if (!SongsMode)
        {
            foreach (Speaker speaker in speakers)
            {
                speaker.SetPickedState(false);
            }
        }
        else
        {
            foreach (MelodySpeaker s in melodySpeakers)
            {
                s.SetPickedState(false);
            }
        }
    }

    private void SetRoundInstrumentVersions()
    {
        AudioClip[] versions;
        if (currentLevel < 10)
        {
            versions = PitchInstruments.LoadRandomInstrumentPair();
            PitchInstruments.previousPicks = versions;
        }
        else
        {
            versions = PitchInstruments.LoadRandomSongPair();
            PitchInstruments.previousPicks = versions;
        }
        int rand = UnityEngine.Random.Range(0, 2);
        melodySpeakers[0].SetClip(versions[rand]);
        melodySpeakers[1].SetClip(versions[(rand + 1) % 2]);

        if (rand == 0)
        {
            melodySpeakers[0].tag = "HighestSpeaker";
        }
        else
        {
            melodySpeakers[1].tag = "HighestSpeaker";
        }
    }
    
    public void PickInstrumentSpeaker(MelodySpeaker melodyspeaker)
    {
        if (melodyspeaker.CompareTag("HighestSpeaker"))
        {
            gameplayAudio.PlayOneShot(sucessAudio);
            melodyspeaker.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore += 1;

        } else
        {
            gameplayAudio.PlayOneShot(failAudio);
            melodyspeaker.GetComponent<MeshRenderer>().material = failMaterial;
        }

        foreach (MelodySpeaker speaker in melodySpeakers)
        {
            speaker.tag = "Untagged";
        }
    }
}
