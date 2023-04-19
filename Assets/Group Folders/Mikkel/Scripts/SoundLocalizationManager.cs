using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using Unity.VisualScripting;

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


    private bool gameIsOver = false;

    private Coroutine waitForTarget;

    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 10;
    [SerializeField]
    randomizeSoundLocation speakerspawner;
    private XRInteractorLineVisual _interactorLine;

    private MeshRenderer meshRenderer;

    public Animator speakerAnimator;

    public GameObject FeedbackZero;
    public GameObject FeedbackOne;
    public GameObject FeedbackTwo;
    public GameObject FeedbackThree;
   
    void Awake()
    {
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        CurrentLevel = gameData.level;
        meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
        speakerAnimator = speaker.GetComponentInChildren<Animator>();
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();

    }
    private void OnEnable()
    {
        distanceTracker.distanceEvent += EndRound;
        StartNewRound();//maybe
    }

    private void OnDisable()
    {
        gameIsOver = false;
        currentRound = 0;
        distanceTracker.distanceEvent -= EndRound;
    }

    private void EndRound()
    {
        if (gameIsOver) return;
        if (waitForTarget != null) return;

        if(CheckRayHit())
        {
            speakerAnimator.enabled = true;
            CurrentScore++;
        }

        meshRenderer.enabled = true;

        waitForTarget = StartCoroutine(WaitForVisibleShootingDisc());
       
    }

    private void StartNewRound() //maybe
    {
        currentRound++;
        Debug.Log("Round: " + currentRound);
        Debug.Log("Score: " + CurrentScore);
        if (currentRound > maxRounds)
        {
            if (!speaker.IsDestroyed()) speaker.GetComponent<DeletusMaximus>().Destroy();
            gameIsOver = true;
            InstrumentSeparation.Instance.EndGame();
            switch (CurrentScore)
            {
                case 0:
                    FeedbackZero.SetActive(true);
                    FeedbackZero.GetComponent<Animator>().enabled = true;
                    return;
                case 1:
                    FeedbackOne.SetActive(true);
                    FeedbackOne.GetComponent<Animator>().enabled = true;
                    return;
                case 2:
                    FeedbackTwo.SetActive(true);
                    FeedbackTwo.GetComponent<Animator>().enabled = true;
                    return;
                case 3:
                    FeedbackThree.SetActive(true);
                    FeedbackThree.GetComponent<Animator>().enabled = true;
                    return;
                default:
                    return;
            }
        }
        Debug.Log( currentRound > maxRounds);
        Debug.Log("Spawn");
        RespawnSpeaker();
        
    }

    private void RespawnSpeaker()
    {
        if(!speaker.IsDestroyed()) speaker.GetComponent<DeletusMaximus>().Destroy();
        speaker = speakerspawner.SpawnSpeaker(prefab);
        if (CurrentLevel < 3)
        {
            speaker.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }else if (CurrentLevel >=3 && CurrentLevel < 6)
        {
            speaker.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else
        {
            speaker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
        speakerAnimator = speaker.GetComponentInChildren<Animator>();
        speakerAnimator.enabled = false;
    }



    public bool CheckRayHit()
    {
        List<Collider> colliders = new List<Collider>();
        RaycastHit[] results = new RaycastHit[1];
        Ray ray = new Ray(_xrRayInteractor.rayOriginTransform.position, _xrRayInteractor.rayOriginTransform.forward);
        return Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("ShootingDisc"));
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
