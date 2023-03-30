using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

public class SoundLocalizationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    public GameObject speaker;
    private int CurrentLevel;
    private int CurrentScore;           
    private string path = "SoundLocalization";
    GameData gameData;
    private XRRayInteractor _xrRayInteractor; 

    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 10;
    [SerializeField]
    randomizeSoundLocation speakerspawner;
    private XRInteractorLineVisual _interactorLine;

    void Start()
    {
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        CurrentLevel = gameData.level;
        speaker = speakerspawner.SpawnSpeaker(prefab);
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
    }
    private void OnEnable()
    {
        distanceTracker.distanceEvent += EndRound;
    }

    private void EndRound()
    {
        
        Debug.Log(CheckRayHit());
        speaker.GetComponent<DeletusMaximus>().Destroy();
        speaker = speakerspawner.SpawnSpeaker(prefab);
        

    }



    public int CheckRayHit()
    {
        List<Collider> colliders = new List<Collider>();
        RaycastHit[] results = new RaycastHit[3];
        Ray ray = new Ray(_xrRayInteractor.rayOriginTransform.position, _xrRayInteractor.rayOriginTransform.forward);
        Physics.RaycastNonAlloc(ray, results, Mathf.Infinity, LayerMask.GetMask("ShootingDisc"));
        foreach (RaycastHit hit in results)
        {
            if (hit.collider == null) continue;
            colliders.Add(hit.collider);
        }
        return colliders.Count;
    }
}
