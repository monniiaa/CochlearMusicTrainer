//======= Copyright (c) ANDALACA Corporation, All rights reserved. ===============
//
// Purpose: Manages game aspects such as spawning, scene loading, levels and more. 
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
  
    #region Internal Variables
    //Global player ref.
    public GameObject player{ get; private set; }
    //Camera fade effect. 
    private const float waitTimeBeforeDelay = 0.5f;
    private const float timeToFade = 2f;
    private float loadDelay;  
    public int currentCondition = 0;
    #endregion
   

    #region Static Variables
    private static bool hasSpawned;
    public static GameManager Instance { get; private set; }   
    #endregion

    #region Set Condition
    public List<GameObject> SoundObjects = new List<GameObject>();
    public int channelToMute { get; private set; }
    #endregion
    
    private void Awake()
    {
        #region Singleton
        //Singleton pattern
        if (Instance == null) //Creates single ton instance and adds it to dont destory
        {
            //For Variable Controller
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        #endregion
        
        // Load spawn Position and spawn player.
        SpawnPlayer();

        // Calculations.
        loadDelay = waitTimeBeforeDelay + timeToFade;   

    }

    private void Start()
    {
        channelToMute = 2;
    }

    #region Awake functions
    //Spawn the player prefab if it has not been spawned before. 
    private void SpawnPlayer()
    {
        if(hasSpawned == false)
        {
            //Find components with level spawn.
            LevelSpawnIndicator levelSpawnTransform = Object.FindObjectOfType<LevelSpawnIndicator>();   
            //Cast gameobject to GameObjectPosition object. 
            Transform levelspawnPosition = levelSpawnTransform.gameObject.transform;

            player = Instantiate(Resources.Load<GameObject>("Player") as GameObject, new Vector3(levelspawnPosition.position.x, levelspawnPosition.position.y-2f, levelspawnPosition.position.z), levelspawnPosition.rotation);
            DontDestroyOnLoad(player);
            //Move to when you save first time!
            hasSpawned = true;
        }
    }

    #region Scene Changing
    //Function for changing scene.
    public IEnumerator LoadScene(string toScene)
    { 
        StartCoroutine(UIHandler.Instance.LevelChangeFade(waitTimeBeforeDelay, false, timeToFade));
        yield return new WaitForSeconds(timeToFade);
        //Begin Loading.
        StartCoroutine(ChangeScene(toScene));
    }

    //Scene changing logic, add code to be loaded on loading screen here. 
    private IEnumerator ChangeScene(string toScene)
    {
        yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(toScene);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            //Updates loading text with progress. 
            //UIHandler.Instance.UpdateLoadingText(asyncOperation.progress);
                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Wait for fade. 
                    yield return new WaitForSeconds(loadDelay);
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
                }
            
            yield return null;
        }
        //IF YOU ARE TO LOAD ANYTHING JUST AS YOU ENTER THE SCENE IT IS HERE ------------------------------------------------------------------------------.
        //Sets player position depending on context.
        SetPlayerPosition(toScene);
        //Camera effect from faded to normal. 
        StartCoroutine(UIHandler.Instance.LevelChangeFade(waitTimeBeforeDelay, true, timeToFade));
    }
    #endregion
   
    //Set player position after level change. Consider rewriting this for more clarity.
    public void SetPlayerPosition(string toScene)
    {   
        //Find components with level spawn.
        LevelSpawnIndicator levelSpawnTransform = Object.FindObjectOfType<LevelSpawnIndicator>();   
        //Cast gameobject to GameObjectPosition object. 
        Transform levelspawnPosition = levelSpawnTransform.gameObject.transform;
        //Assign position to player position
        player.transform.SetPositionAndRotation(new Vector3(levelspawnPosition.position.x, levelspawnPosition.position.y-2f, levelspawnPosition.position.z), levelspawnPosition.rotation);
    }
    #endregion


    #region Set Condition
    public void SetCondition(int conditionIndex)
    {
        currentCondition = conditionIndex;
        StartCoroutine(LoadSounds(conditionIndex));
    }

    IEnumerator LoadSounds(int conditionIndex)
    {
        yield return new WaitForSeconds(1);

        SoundObjects = new List<GameObject>();
        foreach(GameObject soundobject in GameObject.FindGameObjectsWithTag("SoundObject"))
        {
            SoundObjects.Add(soundobject);
        }
        
        switch(conditionIndex)
        {
            //Normal Hearing
            case 0:
                channelToMute = 2;

                for(int i = 0; i< SoundObjects.Count; i++)
                {
                    SoundObjects[i].GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/" + SoundObjects[i].name);
                    if(SoundObjects[i].GetComponent<AudioSource>().loop){
                    SoundObjects[i].GetComponent<AudioSource>().Play();
                    }
                }
            break;

            //Unilateral CI (left)
            case 1:
                channelToMute = 1; //Mute right channel

                for(int i = 0; i< SoundObjects.Count; i++)
                {
                    SoundObjects[i].GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/" + SoundObjects[i].name + "_CI");
                    if(SoundObjects[i].GetComponent<AudioSource>().loop){
                    SoundObjects[i].GetComponent<AudioSource>().Play();
                    }
                }
            break;

            //Unilateral CI (right)
            case 2:
                channelToMute = 0; //Mute left channel

                for(int i = 0; i< SoundObjects.Count; i++)
                {
                    SoundObjects[i].GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/" + SoundObjects[i].name + "_CI");
                    if(SoundObjects[i].GetComponent<AudioSource>().loop){
                    SoundObjects[i].GetComponent<AudioSource>().Play();
                    }
                }
            break;

            //Bilateral CI
            case 3:
                channelToMute = 2;

                for(int i = 0; i< SoundObjects.Count; i++)
                {
                    SoundObjects[i].GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/" + SoundObjects[i].name + "_CI");
                    if(SoundObjects[i].GetComponent<AudioSource>().loop){
                    SoundObjects[i].GetComponent<AudioSource>().Play();
                    }
                }
            break;
        }
    }
    #endregion

}
