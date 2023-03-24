using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLocalizationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    public GameObject speaker;
    private int CurrentLevel;
    private int CurrentScore;
    private string path = "SoundLocalization";
    GameData gameData;

    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 1;
    [SerializeField]
    randomizeSoundLocation speakerspawner;

    void Start()
    {
        gameData = DataManager.ReadJson(path);
        CurrentLevel = gameData.level;
        speaker = speakerspawner.SpawnSpeaker(prefab);
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
    }
    private void OnEnable()
    {
        distanceTracker.distanceEvent += EndRound;
    }

    private void EndRound(float distance)
    {
        speaker.GetComponent<DeletusMaximus>().Destroy();
        speaker = speakerspawner.SpawnSpeaker(prefab);
        IncrementPoint(distance);

    }

    private void IncrementPoint(float distance)
    {

        if (distance <= distanceTreshold)
        {
            CurrentScore += 1;
            Debug.Log(CurrentScore);
        }
        else
        {
            CurrentLevel += 0;
            Debug.Log(CurrentScore);
        }
    }
}
