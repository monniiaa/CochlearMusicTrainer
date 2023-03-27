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
    Canvas endOfRoundCanvas;
    Speaker[] speakers;

    private void Start()
    {
        gameplayAudio = GetComponent<AudioSource>();
        path = "PitchIdentification";
        gameData =  DataManager.ReadJson(path);

        currentLevel = gameData.level;
        Debug.Log("Level: " +currentLevel);
        SetMode();
        speakers = GameObject.FindObjectsOfType<Speaker>();
        initialPositions = new Vector3[speakers.Length];
        endOfRoundCanvas.gameObject.SetActive(false);
        for( int i = 0; i < speakers.Length;i++)
        {
            initialPositions[i] = speakers[i].transform.position;
        }
        StartRound();
    }
    

    public void SetPitchDifference(int minfrequency, int maxfrequency, int intervalFrequency)
    {
        highestSpeaker = speakers[0];
        for (int i = 0; i < speakers.Length; i++)
        {
            int randFrequency = Random.Range(minfrequency, maxfrequency);
            speakers[i].frequency1 = randFrequency;
            speakers[i].frequency2 = randFrequency;
            for (int j = 0; j < speakers.Length;j++)
            {
                if (j != i)
                {
                    while ( speakers[i].frequency1 + (intervalFrequency/2) > speakers[j].frequency1 &&
                        speakers[i].frequency1 - (intervalFrequency /2) < speakers[j].frequency1 )
                    {
                        int rand= Random.Range(minfrequency, maxfrequency);
                        speakers[i].frequency1 = rand;
                        speakers[i].frequency2 = rand;
                    }
                }
            }
            if (speakers[i].frequency1 > highestSpeaker.frequency1)
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
                SetPitchDifference(700, 4000, 2000 - gameData.level * 20);
                break;
            case Difficulty.Medium:
                SetPitchDifference(300, 1500, 1000 - gameData.level * 20);
                break;
            case Difficulty.Hard:
                SetPitchDifference(100, 900, 400 - gameData.level * 20);
                break;
            default:
                break;
        }
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
            foreach (Speaker s in speakers)
            {
                s.DestroySpeaker();
            }
            currentLevel++;
            gameData.level = currentLevel;
            DataManager.SaveDataToJson(gameData, path);
            endOfRoundCanvas.gameObject.SetActive(true);
        }
    }

    protected override void EndRound()
    {

        foreach (Speaker s in speakers)
        {
            s.ResetFrequency();
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
