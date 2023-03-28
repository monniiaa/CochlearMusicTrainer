using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayRandomSound : MonoBehaviour
{
    public Transform[] parentObjects; // array of parent objects to select from
    public int difficulty = 1; // the difficulty level (1-3)

    private List<GameObject> selectedObjects = new List<GameObject>(); // list of selected child objects
    private List<string> objectsPlayed = new List<string>();

    private AudioSource[] allAudioSources;

    private bool round1 = false, round2 = false, round3 = false;

    //Scoring variables
    private float time;
    private float timer1, timer2, timer3;
    public float points;
    private bool isCorrect = false;
    private GameObject currentInstrument;
    [SerializeField] private TMP_Text firstInstrument, secondInstrument, thirdInstrument;

    RaycastHit hit;
    RaycastHit previousHit;
    public Camera camera;

    private void Awake()
    {
        int numParents = difficulty;
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
        foreach (GameObject obj in selectedObjects)
        {
            obj.SetActive(true);
        }
    }

    private void Start()
    {
        Round();
    }

    private void Update()
    {
        time += Time.deltaTime;
        SelectInstrument();

        //Finish round button instead of keycode here
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopMusic();
            if (round1 == false)
            {
                timer1 = time;
                round1 = true;
                if (isCorrect)
                {
                    points += 50;
                }
                Round();
                time = 0;
                isCorrect = false;
            }
            else if (round2 == false)
            {
                timer2 = time;
                round2 = true;
                {
                    points += 50;
                }
                Round();
                time = 0;
            }
            else if (round3 == false)
            {
                timer3 = time;
                round3 = true;
                {
                    points += 50;
                }
                EndFeedback();
            }
            else if (round3 == true)
            {
                Debug.Log("training over");
            }
        }
    }

    private void EndFeedback()
    {
        //UI implementation, with text files showing which instruments were played
        firstInstrument.text = objectsPlayed[0];
        firstInstrument.gameObject.SetActive(true);
        secondInstrument.text = objectsPlayed[1];
        secondInstrument.gameObject.SetActive(true);
        thirdInstrument.text = objectsPlayed[2];
        thirdInstrument.gameObject.SetActive(true);

    }

    private void Round()
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

    private void SelectInstrument()
    {
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
            currentInstrument.tag = "default";
        }
    }
}
