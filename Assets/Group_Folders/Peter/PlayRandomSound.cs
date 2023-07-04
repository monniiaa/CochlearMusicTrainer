using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;

public class PlayRandomSound : MonoBehaviour
{
    public Transform[] parentObjects; // array of parent objects to select from
    public GameData gameData;
    public int difficulty = 1;
    public string path;
    public int currentLevel;

    //Variables in charge of getting the child objects and their audiosources
    public List<GameObject> selectedObjects = new List<GameObject>(); // list of selected child objects
    private List<string> objectsPlayed = new List<string>();
    private AudioSource[] allAudioSources;

    //Round related variables
    private bool round1 = false, round2 = false, round3 = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip incorrectClip;

    //Scoring variables
    public float time;
    private float timer1, timer2, timer3;
    public float points;
    private bool isCorrect = false;
    public GameObject currentInstrument;
    //[SerializeField] private TMP_Text firstInstrument, secondInstrument, thirdInstrument;
    [SerializeField] private GameObject oneStar, twoStar, threeStar, noStar;
    [SerializeField] private GameObject resultCanvas;
    private List<int> errorCount = new List<int>();
    private List<float> timerCount = new List<float>();

    //VR interaction variables
    [SerializeField] private OutlineManager outline;

    RaycastHit hit;
    RaycastHit previousHit;
    public Camera camera;
    private int selectedObjectIndex;
    private GameDataManager gameDataManager;
    private int currentScore;
    private DateTime startTime;
    private DateTime startPlayTime;
    private string chosenInstrument;

    //public InputActionReference XRinput;

    private void Awake()
    {
        gameDataManager = GameDataManager.Instance;
        audioSource = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);
        currentLevel = (gameDataManager.currentLevel == 0) ? 1 : gameDataManager.currentLevel;

        int numParents = difficulty;
        List<int> pickedFamily = new List<int>();
        for (int i = 0; i < numParents; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = UnityEngine.Random.Range(0, parentObjects.Length);
            }
            while (pickedFamily.Contains(randomIndex));
            
            pickedFamily.Add(randomIndex);
            Transform parent = parentObjects[randomIndex];
            // select a random child object from the parent object and add it to the list
            int numChildren = parent.childCount;
            int randomChildIndex = UnityEngine.Random.Range(0, numChildren);
            GameObject child = parent.GetChild(randomChildIndex).gameObject;
            selectedObjects.Add(child);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopRound();
        }
    }

    private void EndFeedback()
    {
        resultCanvas.SetActive(true);
        switch (errorCount.Count)
        {
            case 3:
                noStar.SetActive(true);
                break;
            case 2:
                oneStar.SetActive(true);
                break;
            case 1:
                twoStar.SetActive(true);
                break;
            default:
                threeStar.SetActive(true);
            break;

        }
    }

    public void StopRound()
    {
        outline.ClearAllSelections();
        StopMusic();
            if (round1 == false)
            {
                
                timer1 = time;
                timerCount.Add(timer1);
                round1 = true;
                if (isCorrect)
                {
                    currentScore++;
                }
                else
                {
                    errorCount.Add(1);
                }
                RoundFinishAudio();
                StartCoroutine(WaitForFeedback(audioSource));
                time = 0;
                isCorrect = false;
            }
            else if (round2 == false)
            {

                timer2 = time;
                timerCount.Add(timer2);
                round2 = true;
                if (isCorrect)
                {
                    currentScore++;
                }
                else
                {
                    errorCount.Add(1);
                }
            RoundFinishAudio();
            StartCoroutine(WaitForFeedback(audioSource));
            time = 0;
            }
            else if (round3 == false)
            {
                timer3 = time;
                timerCount.Add(timer3);
                round3 = true;
                if (isCorrect)
                {
                    currentScore++;
                }
                else
                {
                    errorCount.Add(1);
                }
                JsonManager.WriteDataToFile<ScoreData>(new ScoreData("Instrument Identification", DateTime.Now, currentScore, currentLevel));
                if(currentLevel == gameData.level) gameData.level++;
                if(currentScore > gameData.levelScore[currentLevel - 1])
                {
                    gameData.levelScore[currentLevel - 1] = currentScore;
                }
                DataManager.SaveDataToJson(gameData, path);
                RoundFinishAudio();
                selectedObjectIndex = 0;
                EndFeedback();
            }
            else if (round3 == true)
            {
                
                Debug.Log("training over");
            }
    }
    

    public void Round()
    {
        // randomly select one of the selected child objects and play a random sound from its associated sound folder
        //int selectedIndex = Random.Range(0, selectedObjects.Count);
        GameObject selectedObject = selectedObjects[selectedObjectIndex];
        string folderName = selectedObject.name + "Track"; // assume the sound folder is named after the child object
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderName);
        if (clips != null && clips.Length > 0)
        {
            AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
            AudioSource audioSource = selectedObject.GetComponent<AudioSource>();
            selectedObject.tag = "playing";
            currentInstrument = selectedObject;
            objectsPlayed.Add(selectedObject.ToString());
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        selectedObjectIndex++;
        startTime = DateTime.Now;
    }

    private IEnumerator WaitForFeedback(AudioSource audio)
    {
        yield return new WaitWhile(() => audio.isPlaying);
        Round();
    }

    private void StopMusic()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    public void Triggered(GameObject instrument)
    {
        chosenInstrument = instrument.name;
        //if (outline.selected == null || currentInstrument == null) return;
        if (currentInstrument.name == instrument.name)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }

    private void OnEnable()
    {
        startPlayTime = DateTime.Now;
        //XRinput.action.performed += StopRound;
    }

    private IEnumerator WaitForSelection()
    {
        yield return new WaitForEndOfFrame();
        
    }

    private void RoundFinishAudio()
    {
        if (isCorrect)
        {
            audioSource.clip = correctClip;
        }
        else
        {
            audioSource.clip = incorrectClip;
        }
        audioSource.Play();
    }

    private void OnDisable() 
    {
        JsonManager.WriteDataToFile<PlayTimeData>(new PlayTimeData("Instrument Identification", DateTime.Now, DateTime.Now - startPlayTime));
    }
}
