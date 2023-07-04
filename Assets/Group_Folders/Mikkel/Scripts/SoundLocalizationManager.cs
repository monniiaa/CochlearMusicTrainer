using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using Unity.VisualScripting;
using System;

public class SoundLocalizationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    public GameObject speaker;
    [SerializeField] private GameObject transparentTarget;
    private int currentRound = 0;
    private int maxRounds = 3;

    private int CurrentLevel;
    private int CurrentScore;           
    private string path = "SoundLocalization";
    GameData gameData;
    public XRRayInteractor _xrRayInteractor;
    
    private GameDataManager gameDataManager;


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
    public GameObject[] starAnimation;
    private DateTime startTime;
   
    void Awake()
    {
        gameDataManager = GameDataManager.Instance;
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        CurrentLevel = (gameDataManager.currentLevel == 0) ? 1 : gameDataManager.currentLevel;
        meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
        speakerAnimator = speaker.GetComponentInChildren<Animator>();
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();

    }
    private void OnEnable()
    {
        CurrentScore = 0;
        distanceTracker.distanceEvent += EndRound;
        StartNewRound();//maybe
        
    }

    private void OnDisable()
    {
        gameIsOver = false;
        distanceTracker.distanceEvent -= EndRound;
        currentRound = 0;
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
        JsonManager.WriteDataToFile<SoundLocalizationDataContainer>(
            new SoundLocalizationDataContainer(
            DateTime.Now, 
            DateTime.Now - startTime,
            speakerAnimator.enabled, 
            Vector3.Distance(Camera.main.transform.position, speaker.transform.position), 
            Vector3.Angle( _xrRayInteractor.rayOriginTransform.forward, speaker.transform.position - _xrRayInteractor.rayOriginTransform.position),
            CurrentLevel,
            currentRound));
        waitForTarget = StartCoroutine(WaitForVisibleShootingDisc());
    }

    private void StartNewRound()
    {
        startTime = DateTime.Now;
        currentRound++;
        if (currentRound > maxRounds)
        {
            if (!speaker.IsDestroyed()) speaker.GetComponent<DeletusMaximus>().Destroy();
            gameIsOver = true;
            JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Sound Localization", DateTime.Now, CurrentScore, CurrentLevel));
            InstrumentSeparation.Instance.EndGame();
            if(CurrentLevel == gameData.level)
            {
                gameData.level++;
            }
            if(CurrentScore > gameData.levelScore[CurrentLevel - 1])
            {
                gameData.levelScore[CurrentLevel - 1] = CurrentScore;
            }
            
            DataManager.SaveDataToJson(gameData, path);
            ShowStars(CurrentScore);
            return;
        }
        RespawnSpeaker();
        
    }

    private void ShowStars(int score)
    {
        for (int i = 0; i < starAnimation.Length; i++)
        {
            if(i == score) 
            {
                starAnimation[i].SetActive(true);
                continue;
            }
            starAnimation[i].SetActive(false);
        }
    }

    private void RespawnSpeaker()
    {
        
        if (CurrentLevel == 1)
        {
            if(currentRound == 1)
            {
                speaker = speakerspawner.SpawnSpeaker(prefab);
            }
            else
            {
                speaker = speakerspawner.SpawnSpeaker(transparentTarget);
            }
        }
        else
        {
            speaker = speakerspawner.SpawnSpeaker(prefab);
        }
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

        if (currentRound > 1)
        {
            meshRenderer = speaker.GetComponentInChildren<MeshRenderer>();
            meshRenderer.enabled = false;
        }
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
        if(!speaker.IsDestroyed()) speaker.GetComponent<DeletusMaximus>().Destroy();
        StartNewRound();
    }
}
