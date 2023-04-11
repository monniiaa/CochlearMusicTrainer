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

    private int currentRound = 0;
    private int maxRounds = 3;

    private int CurrentLevel;
    private int CurrentScore;           
    private string path = "SoundLocalization";
    GameData gameData;
    private XRRayInteractor _xrRayInteractor;

    private Coroutine waitForTarget;

    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 10;
    [SerializeField]
    randomizeSoundLocation speakerspawner;
    private XRInteractorLineVisual _interactorLine;

    private MeshRenderer meshRenderer;

    public Animator speakerAnimator;

    void Start()
    {
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        CurrentLevel = gameData.level;
        speaker = speakerspawner.SpawnSpeaker(prefab);
        meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
        speakerAnimator = speaker.GetComponentInChildren<Animator>();
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
        StartNewRound();//maybe

    }
    private void OnEnable()
    {
        distanceTracker.distanceEvent += EndRound;
    }

    private void EndRound()
    {
        if (waitForTarget != null) return;


        int numCollidersHit = CheckRayHit();

        if (numCollidersHit > 0)
        {
            speakerAnimator.enabled = true;
        }

        meshRenderer.enabled = true;

        Debug.Log(CheckRayHit());

        waitForTarget = StartCoroutine(WaitForVisibleShootingDisc());
       
    }

    private void StartNewRound() //maybe
    {
        currentRound++;
        Debug.Log("Round: " + currentRound);
        if (currentRound > maxRounds)
        {
            Debug.Log("Game over");
            return;
        }

        RespawnSpeaker();
    }

    private void RespawnSpeaker()
    {
        speaker.GetComponent<DeletusMaximus>().Destroy();
        speaker = speakerspawner.SpawnSpeaker(prefab);
        meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
        speakerAnimator = speaker.GetComponentInChildren<Animator>();
        speakerAnimator.enabled = false;
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

    IEnumerator WaitForVisibleShootingDisc()
    {
        
        yield return new WaitForSeconds(2.5f);
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        waitForTarget = null;
        StartNewRound();
    }
}
