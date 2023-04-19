using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchIdentification : LevelManager
{
    private int maxLevel = 4;

    private Speaker highestSpeaker;
    private bool correctPick;

    public GameObject speakerPrefab;
    public Vector3[] initialPositions;
    [SerializeField]

    Speaker[] speakers;
    public GameObject[] starAnimation = new GameObject[4];
    public InstrumentSeparation ModeManager;


    private void Awake()
    {
        ModeManager = InstrumentSeparation.Instance;
        gameplayAudio = GetComponent<AudioSource>();
        path = "PitchIdentification";
        gameData =  DataManager.ReadJson(path);

        currentLevel = gameData.level;
        Debug.Log("Level: " +currentLevel);
        SetMode();
        speakers = GameObject.FindObjectsOfType<Speaker>();
        initialPositions = new Vector3[speakers.Length];
        for( int i = 0; i < speakers.Length;i++)
        {
            initialPositions[i] = speakers[i].transform.position;
        }
        StartRound();
    }
    
    public void RestartLevel()
    {
        currentLevel--;
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
            int randNote = Random.Range(lowestNote, highestNote);
            speakers[i].note = randNote;
            speakers[i].SetNote(randNote);
            for (int j = 0; j < speakers.Length;j++)
            {
                if (j != i)
                {
                    
                    while (  speakers[i].note > speakers[j].note - interval && speakers[i].note < speakers[j].note + interval)
                    {
                        int rand= Random.Range(lowestNote, highestNote);
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
        EndRound();
        if (speaker== highestSpeaker) {
            gameplayAudio.PlayOneShot(sucessAudio);
            speaker.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore += 1;
            gameData.levelScore[currentLevel-1] = currentScore;

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
        yield return new WaitForSeconds(0.7f);
        foreach (Speaker s in speakers)
        {
            s.gameObject.SetActive(false);
        }
        ModeManager.EndGame();
        //TODO: SHOW STAR RESULT

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
            currentLevel++;
            gameData.level = currentLevel;
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
        foreach (Speaker speaker in speakers)
        {
            speaker.SetPickedState(false);
        }
        SetDifficultyChanges();
    }
}
