using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

public class SoundLocalization : LevelManager
{
    [SerializeField] private GameObject mediumHardTargetPrefab;
    [SerializeField] private GameObject mediumHardSpeaker;
    private bool gameIsOver = false;
    private Coroutine waitForTarget;
    [SerializeField] XRInteractorLineVisual _interactorLine;
    private MeshRenderer meshRenderer;
    [SerializeField]
    public GameObject easyTargetPrefab;
    public Animator speakerAnimator;
    private AlphaChange _alphaChange;
    [SerializeField] RuntimeAnimatorController animatorcontrollerMediumHard;
    private MeshRenderer targetMesh;

    [SerializeField] private  List<GameObject> easyModeTargets = new List<GameObject>();
    private int easyModeTargetAmount = 5;
    
    private AudioClip targetSound;
    GameData gameData;
    private DateTime startTime;
    private GameDataManager gameDataManager;
    
    [SerializeField]
    private DistanceTracker distanceTracker;
    private static float distanceTreshold = 10;
    
    public XRRayInteractor _xrRayInteractor;

    private randomizeSoundLocation speakerSpawner;
    [SerializeField] RuntimeAnimatorController animatorcontrollerEasy;
    private InstrumentSeparation modeManager;

    private void Awake()
    {
        round = 0;
        path = "SoundLocalization";
        gameDataManager = GameDataManager.Instance;
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        currentLevel = (gameDataManager.currentLevel == 0) ? 1 : gameDataManager.currentLevel;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
        speakerSpawner = GameObject.FindObjectOfType<randomizeSoundLocation>();
        targetSound = Resources.Load<AudioClip>("SoundLocalization/TargetSound");
        
        meshRenderer = mediumHardSpeaker.GetComponentInChildren<MeshRenderer>();
        speakerAnimator = mediumHardSpeaker.GetComponentInChildren<Animator>();
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
        _alphaChange = GetComponent<AlphaChange>();
        
        modeManager = InstrumentSeparation.Instance;
        SetMode();
        
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }
    
    private void OnEnable()
    {
        currentScore = 0;
        if(difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
        {
            distanceTracker.distanceEvent += EndRound;
        }
        SetRoundFunctionality();

    }
    
    private void OnDisable()
    {
        gameIsOver = false;
        if(difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
        {
            distanceTracker.distanceEvent -= EndRound;
        }
        round = 0;
    }
    
    

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                _interactorLine.stopLineAtFirstRaycastHit = true;
                _interactorLine.snapEndpointIfAvailable = true;
                switch (currentLevel)
                {
                    case 1:
                        easyModeTargetAmount = 1 + round * 2;
                        break;
                    case 2:
                        easyModeTargetAmount = 10 + round;
                        break;
                    case 3:
                        easyModeTargetAmount = 13 + round;
                        break;
                }

                SpawnSpeakers();
                break;
            case Difficulty.Medium:
                _interactorLine.stopLineAtFirstRaycastHit = false;
                _interactorLine.snapEndpointIfAvailable = false;
                speakerSpawner.height = 0.2f;
                mediumHardTargetPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
                RespawnSpeaker();
                break;
            case Difficulty.Hard:
                _interactorLine.stopLineAtFirstRaycastHit = false;
                _interactorLine.snapEndpointIfAvailable = false;
                speakerSpawner.height = 1f;
                mediumHardTargetPrefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                RespawnSpeaker();
                break;
        }
    }

    public override void SetRoundFunctionality()
    {
        round++;
        if (round < 4 && difficulty == Difficulty.Easy)
        {
            EndRound();
            StartCoroutine(WaitBeforStaring());
        }
        else if (difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
        {

            StartRound();
            
                startTime = DateTime.Now;
                if (round >= 4)
                {
                    if (!mediumHardSpeaker.IsDestroyed()) mediumHardSpeaker.GetComponent<DeletusMaximus>().Destroy();
                    gameIsOver = true;
                    JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Sound Localization", DateTime.Now,
                        currentScore, currentLevel));
                    modeManager.EndGame();
                    ShowStar(currentScore);
                    
                    if (currentLevel == gameData.level)
                    {
                        gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
                    }

                    if (gameData.levelScore[currentLevel - 1] < currentScore)
                    {
                        gameData.levelScore[currentLevel - 1] = currentScore;
                    }
                    DataManager.SaveDataToJson(gameData, path);
                }
        }
        if (round >= 4)
        {
            if (difficulty == Difficulty.Easy)
            {
                JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Sound Localization", DateTime.Now,
                    currentScore, currentLevel));
                StartCoroutine(End());
            }
            if (currentLevel == gameData.level)
                {
                    gameData.level = (currentLevel == maxLevel) ? gameData.level : currentLevel + 1;
                }

                if (gameData.levelScore[currentLevel - 1] < currentScore)
                {
                    gameData.levelScore[currentLevel - 1] = currentScore;
                }
                DataManager.SaveDataToJson(gameData, path);
        }
    }
    
    private void RespawnSpeaker()
    {
        mediumHardSpeaker = speakerSpawner.SpawnSpeaker(mediumHardTargetPrefab);
        
        meshRenderer = mediumHardSpeaker.GetComponentInChildren<MeshRenderer>();
        switch (currentLevel)
        {
            case 4:
                meshRenderer.enabled = true;
                if (round == 2)
                {
                    _alphaChange.MakeTransparent(mediumHardSpeaker.GetComponentInChildren<Renderer>(), 0.5f);
                }
                else if (round == 3)
                {
                    meshRenderer.enabled = false;
                }

                break;
            default:
                meshRenderer.enabled = false;
                break;
        }

        speakerAnimator = mediumHardSpeaker.GetComponentInChildren<Animator>();
        speakerAnimator.runtimeAnimatorController = animatorcontrollerMediumHard;
        speakerAnimator.enabled = false;
    }

    public bool CheckRayHit()
    {
        List<Collider> colliders = new List<Collider>();
        RaycastHit[] results = new RaycastHit[1];
        Ray ray = new Ray(_xrRayInteractor.rayOriginTransform.position, _xrRayInteractor.rayOriginTransform.forward);
        
        return Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("ShootingDisc"));
    }


    IEnumerator End()
    {
        if (difficulty == Difficulty.Easy)
        {
            StartCoroutine(ResetEasyModeTargets());
        }
        yield return new WaitForSeconds(2f);
        modeManager.EndGame();
        ShowStar(currentScore);


    }
    private void SpawnSpeakers()
    {
        easyModeTargets = speakerSpawner.SpawnSpeakers(easyModeTargetAmount,1f, easyTargetPrefab);
        foreach (GameObject target in easyModeTargets)
        {
            target.GetComponent<Animator>().runtimeAnimatorController = animatorcontrollerEasy;
        }
        int randomIndex = UnityEngine.Random.Range(0, easyModeTargets.Count);
        easyModeTargets[randomIndex].GetComponent<AudioSource>().clip = targetSound;
        easyModeTargets[randomIndex].GetComponent<AudioSource>().Play();
    }

    public void PickTarget(GameObject target)
    {
        if (target.GetComponent<AudioSource>().clip != null)
        {
            target.GetComponent<AudioSource>().Stop();
            currentScore++;
            gameplayAudio.PlayOneShot(sucessAudio);
            target.GetComponent<TargetLookAt>().enabled = false;
            target.GetComponent<TargetAnimationHandler>().SetCorrectState(true);
        }
        else
        {
            gameplayAudio.PlayOneShot(failAudio);
            target.GetComponent<TargetAnimationHandler>().TriggerDestroyState();
        }
        SetRoundFunctionality();
    }
    

    protected override void EndRound()
    {
        if (difficulty == Difficulty.Easy)
        {
            StartCoroutine(ResetEasyModeTargets());
        } if (difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
        {
            if (gameIsOver) return;
            if (waitForTarget != null) return;

            if (CheckRayHit())
            {
                speakerAnimator.GetComponentInChildren<TargetLookAt>().enabled = false;
                speakerAnimator.enabled = true;
                currentScore++;
            }
            JsonManager.WriteDataToFile<SoundLocalizationDataContainer>(
                new SoundLocalizationDataContainer(
                    DateTime.Now, 
                    DateTime.Now - startTime,
                    speakerAnimator.enabled, 
                    Vector3.Distance(Camera.main.transform.position, mediumHardSpeaker.transform.position), 
                    Vector3.Angle( _xrRayInteractor.rayOriginTransform.forward, mediumHardSpeaker.transform.position - _xrRayInteractor.rayOriginTransform.position),
                    currentLevel,
                    round));
            meshRenderer.enabled = true;
            waitForTarget = StartCoroutine(WaitForVisibleShootingDisc());
        }
    }
    
    IEnumerator WaitForVisibleShootingDisc()
    {
        yield return new WaitForSeconds(2.5f);
        meshRenderer.enabled = false;
        speakerAnimator.enabled = false;
        waitForTarget = null;
        if(!mediumHardSpeaker.IsDestroyed()) mediumHardSpeaker.GetComponent<DeletusMaximus>().Destroy();
        SetRoundFunctionality();
        
    }
    
    IEnumerator ResetEasyModeTargets()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < easyModeTargets.Count; i++)
        {
            easyModeTargets[i].GetComponent<AudioSource>().Stop();
            easyModeTargets[i].GetComponent<TargetAnimationHandler>().TriggerDestroyState();
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < easyModeTargets.Count; i++)
        {
            easyModeTargets[i].GetComponent<DeletusMaximus>().Destroy();
        }
        
        easyModeTargets.Clear();
    }

    IEnumerator WaitBeforStaring()
    {
        yield return new WaitForSeconds(2f);
        StartRound();
    }
    protected override void StartRound()
    {
        SetDifficultyChanges();
    }

    protected override void RestartLevel()
    {
        round = 1;
        currentScore = 0;
    }
}