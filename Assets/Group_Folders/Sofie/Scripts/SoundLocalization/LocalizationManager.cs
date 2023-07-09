using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit;

public class LocalizationManager : LevelManager
{
    [SerializeField]
    public GameObject easyTargetPrefab;
    
    private MeshRenderer targetMesh;
    private bool transparent = false;

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
    [SerializeField] RuntimeAnimatorController animatorcontroller;
    private InstrumentSeparation modeManager;

    private void Awake()
    {
        path = "SoundLocalization";
        gameDataManager = GameDataManager.Instance;
        _xrRayInteractor = FindObjectOfType<XRRayInteractor>();
        gameData = DataManager.ReadJson(path);
        currentLevel = (gameDataManager.currentLevel == 0) ? 1 : gameDataManager.currentLevel;
        distanceTracker = GameObject.FindObjectOfType<DistanceTracker>();
        speakerSpawner = GameObject.FindObjectOfType<randomizeSoundLocation>();
        targetSound = Resources.Load<AudioClip>("SoundLocalization/TargetSound");
        
        
        modeManager = InstrumentSeparation.Instance;
        SetMode();
        
        foreach (GameObject star in starAnimation)
        {
            star.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartRound();
        Debug.Log(difficulty);
    }
    private void OnEnable()
    {
        currentScore = 0;
        distanceTracker.distanceEvent += EndRound;

    }
    
    private void OnDisable()
    {
        distanceTracker.distanceEvent -= EndRound;
    }
    

    protected override void SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                switch (currentLevel)
                {
                    case 1:
                        easyModeTargetAmount = 1 + round;
                        break;
                    case 2:
                        easyModeTargetAmount = 8 + round;
                        break;
                    case 3:
                        easyModeTargetAmount = 10+ round;
                        break;
                }
                SpawnSpeakers();
                break;
        }
    }

    public override void SetRoundFunctionality()
    {
        EndRound();
        round++;
        if (round < 4)
        {
            StartCoroutine(WaitBeforStaring());
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
    public bool CheckRayHit()
    {
        List<Collider> colliders = new List<Collider>();
        RaycastHit[] results = new RaycastHit[1];
        Ray ray = new Ray(_xrRayInteractor.rayOriginTransform.position, _xrRayInteractor.rayOriginTransform.forward);
        return Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("ShootingDisc"));
    }


    IEnumerator End()
    {
        yield return new WaitForSeconds(2f);
        modeManager.EndGame();
        ShowStar(currentScore);
    }
    private void SpawnSpeakers()
    {
        easyModeTargets = speakerSpawner.SpawnSpeakers(easyModeTargetAmount,1f, easyTargetPrefab);
        foreach (GameObject target in easyModeTargets)
        {
            target.GetComponent<Animator>().runtimeAnimatorController = animatorcontroller;
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
        }

        
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
        
    }
}
