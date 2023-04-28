using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayRandomSound : MonoBehaviour
{
    public Transform[] parentObjects; // array of parent objects to select from
    public GameData gameData;
    public Difficulty difficulty; // the difficulty level (1-3)
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
    private GameObject currentInstrument;
    //[SerializeField] private TMP_Text firstInstrument, secondInstrument, thirdInstrument;
    [SerializeField] private GameObject oneStar, twoStar, threeStar, noStar;
    [SerializeField] private GameObject resultCanvas;
    private List<int> errorCount = new List<int>();
    private List<float> timerCount = new List<float>();

    //VR interaction variables
    [SerializeField]
    private OutlineManager outline;

    RaycastHit hit;
    RaycastHit previousHit;
    public Camera camera;

    public InputActionReference XRinput;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        path = "InstrumentIdentification";
        gameData = DataManager.ReadJson(path);
        currentLevel = gameData.level;
        SetDifficulty();
        int numParents = SetDifficultyChanges();
        List<int> pickedFamily = new List<int>();
        for (int i = 0; i < numParents; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, parentObjects.Length);
            }
            while (pickedFamily.Contains(randomIndex));
            
            pickedFamily.Add(randomIndex);
            Transform parent = parentObjects[randomIndex];
            // select a random child object from the parent object and add it to the list
            int numChildren = parent.childCount;
            int randomChildIndex = Random.Range(0, numChildren);
            //Debug.Log("children" + randomChildIndex);
            GameObject child = parent.GetChild(randomChildIndex).gameObject;
            selectedObjects.Add(child);
        }
        //Debug.Log(selectedObjects.Count);
        
        
    }

    private void Update()
    {
        time += Time.deltaTime;
        //SelectInstrument();
    }

    private int SetDifficultyChanges()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return 1;
                break;
            case Difficulty.Medium:
                return 2;
                break;
            case Difficulty.Hard:
                return 3;
                break;
            default:
                return 1;
        }
    }
    
    protected void SetDifficulty()
    {
        if (currentLevel <= 3)
        {
            difficulty = Difficulty.Easy;
        }
        else if (currentLevel > 3 && currentLevel <= 6)
        {
            difficulty = Difficulty.Medium;
        }
        else if (currentLevel > 6)
        {
            difficulty = Difficulty.Hard;
        }
    }
    
    
    private void EndFeedback()
    {
        //UI implementation, with text files showing which instruments were played
        /*firstInstrument.text = objectsPlayed[0];
        firstInstrument.gameObject.SetActive(true);
        secondInstrument.text = objectsPlayed[1];
        secondInstrument.gameObject.SetActive(true);
        thirdInstrument.text = objectsPlayed[2];
        //testing below
        //thirdInstrument.text = points.ToString();
        thirdInstrument.gameObject.SetActive(true);*/
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

    public void StopRound(InputAction.CallbackContext ctx)
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
                    points += 50;
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
                    points += 50;
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
                    points += 50;
                }
                else
                {
                    errorCount.Add(1);
                }

                currentLevel++;
                gameData.level = currentLevel;
                DataManager.SaveDataToJson(gameData, path);
                RoundFinishAudio();
            EndFeedback();
                Debug.Log(errorCount.Count);
            }
            else if (round3 == true)
            {
                Debug.Log("training over");
            }
    }

    public void Round()
    {
        // randomly select one of the selected child objects and play a random sound from its associated sound folder
        int selectedIndex = Random.Range(0, selectedObjects.Count);
        GameObject selectedObject = selectedObjects[selectedIndex];
        string folderName = selectedObject.name + "Track"; // assume the sound folder is named after the child object
        Debug.Log(folderName);
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderName);
        if (clips != null && clips.Length > 0)
        {
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];
            AudioSource audioSource = selectedObject.GetComponent<AudioSource>();
            selectedObject.tag = "playing";
            currentInstrument = selectedObject;
            objectsPlayed.Add(selectedObject.ToString());
            audioSource.clip = randomClip;
            audioSource.Play();
        }
    }

    private IEnumerator WaitForFeedback(AudioSource audio)
    {
        yield return new WaitUntil(()=>audio.isPlaying);
        Round();
    }

    private void SelectInstrument()
    {
        //Possibly outdated method here, from testing stage
        previousHit = hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "playing")
            {
                if (Input.GetMouseButton(0))
                {
                    isCorrect = true;
                }
            }
        }
    }

    private void StopMusic()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    public void Triggered()
    {
        if (outline.selected == null || currentInstrument == null) return;
        if (currentInstrument.name.Equals(outline.selected.name))
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
        XRinput.action.performed += StopRound;
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
}
