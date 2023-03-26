using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchIdentification : MonoBehaviour
{
    private int maxLevel = 4;
    private GameData data;
    private string path = "PitchIdentification";

    private Difficulty difficulty;
    [SerializeField]
    private GameObject[] modes;

    private Speaker highestSpeaker;
    private bool correctPick;

    private int currentLevel;
    private int currentScore = 0;

    private int tries = 0;

    public GameObject speakerPrefab;

    public Vector3[] initialPositions;

    public AudioSource gameplayAudio;
    public AudioClip sucessAudio;
    public AudioClip failAudio;
    [SerializeField]
    Material sucessMaterial;
    [SerializeField]
    Material failMaterial;
    [SerializeField]
    Canvas endOfRoundCanvas;
    Speaker[] speakers;




    private void Start()
    {
        gameplayAudio = GetComponent<AudioSource>();
        data =  DataManager.ReadJson(path);

        currentLevel = data.level;
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
    
    private void SetMode()
    {
        if(currentLevel <= 3)
        {
            difficulty = Difficulty.Easy; 
            modes[0].SetActive(true);
            modes[1].SetActive(false);
            modes[2].SetActive(false);
        }
        else if(currentLevel >3 && currentLevel <= 10)
        {
            difficulty = Difficulty.Medium;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        } else if(currentLevel > 10)
        {
            difficulty = Difficulty.Hard;
            modes[0].SetActive(false);
            modes[1].SetActive(true);
            modes[2].SetActive(false);
        }
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
        tries++;
        Debug.Log(tries);
        if (speaker== highestSpeaker) {
            gameplayAudio.PlayOneShot(sucessAudio);
            speaker.GetComponent<MeshRenderer>().material = sucessMaterial;
            currentScore += 1;
            data.levelScore[currentLevel-1] = currentScore;

        } else if (speaker!= highestSpeaker)
        {
            gameplayAudio.PlayOneShot(failAudio);
            speaker.GetComponent<MeshRenderer>().material = failMaterial;
        }
        switch (tries)
        {
            case 1:
                StartRound();
                break;
            case 2:

                StartRound();
                break;
            case 3:
                StartRound();
                foreach (Speaker s in speakers)
                {
                    s.DestroySpeaker();
                }
                currentLevel++;
                data.level = currentLevel;
                DataManager.SaveDataToJson(data, path);
                endOfRoundCanvas.gameObject.SetActive(true);
                break;
              
        }
    }



    public void EndRound(Speaker speaker)
    {
        SpeakerPicked(speaker);
        foreach (Speaker s in speakers)
        {
            s.DestroyAnimation();
        }
    }

    public void StartRound()
    {
        foreach(Speaker speaker in speakers)
        {
            speaker.SetPickedState(false);
        }
        switch (difficulty)
        {
            case Difficulty.Easy:
                SetPitchDifference( 700, 4000, 2000 - data.level * 20);
                break;
            case Difficulty.Medium:
                SetPitchDifference( 300, 1500, 1000 - data.level * 20);
                break;
            case Difficulty.Hard:
                SetPitchDifference( 100, 900, 400 - data.level * 20);
                break;
            default:
                break;
        }

    }

}
