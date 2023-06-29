using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentIdentification : MonoBehaviour
{
    //Music objects
    public GameObject[] objectsToChooseFrom;
    public Dictionary<string, string> objectToFolderMap = new Dictionary<string, string>()
{
    {"DrumKit", "DrumTrack"},
    {"KeyboardStand", "PianoTrack"},
    {"AcousticGuitar (7)", "GuitarTrack" },
    {"Bass", "BassTrack" },
    {"ElectricGuitar", "ElectricGuitarTrack"},
    // Add more entries as needed
};
    [SerializeField] private GameObject easy, medium, hard;
    private AudioSource audioSource;
    private AudioSource[] allAudioSources;

    //Difficulty and round variables
    [SerializeField] private int difficulty;
    private bool round1 = false, round2 = false, round3 = false;
    private int difficultyAdjust;

    //Scoring variables
    private float time;
    public float points;
    private bool isCorrect = false;
    private GameObject currentInstrument;

    RaycastHit hit;
    RaycastHit previousHit;
    public Camera camera;

    private void Awake()
    {
        switch (difficulty)
        {
            case (2):
                difficultyAdjust = 0;
                break;
            case (1):
                difficultyAdjust = 1;
                hard.SetActive(false);
                break;
            case (0):
                difficultyAdjust = 3;
                hard.SetActive(false);
                medium.SetActive(false);
                break;
            default:
                Debug.Log("Incorrect difficulty, Choose either: 0 = easy, 1 = medium, 2 = hard");
            break;
        }
        Round();
    }

    private void ChooseSound(int difficulty)
    {
        // Choose a random object from the list
        int randomIndex = Random.Range(0, objectsToChooseFrom.Length - difficulty);
        GameObject chosenObject = objectsToChooseFrom[randomIndex];
        currentInstrument = chosenObject;
        currentInstrument.tag = "playing";

        // Get the folder name specific to the chosen game object
        string folderName;
        if (objectToFolderMap.TryGetValue(chosenObject.name, out folderName))
        {
            // Load the audio clip from the specific folder
            AudioClip[] clipsInFolder = Resources.LoadAll<AudioClip>(folderName);

            // Choose a random clip from the specific folder
            int randomClipInFolderIndex = Random.Range(0, clipsInFolder.Length);
            AudioClip clipInFolderToPlay = clipsInFolder[randomClipInFolderIndex];

            // Override the clip to play with the one from the specific folder
            if (clipInFolderToPlay != null)
            {
                // Find the audio source component attached to this game object
                audioSource = chosenObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.clip = clipInFolderToPlay;
                }
            }
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        SelectInstrument();
        //Debug.DrawRay(ray.origin, ray.direction * 10);

        //Picking the objects needs to be integrated here
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopMusic();
            if (round1 == false)
            {
                round1 = true;
                if (isCorrect)
                {
                    points += 50;
                    Debug.Log("test");
                }
                Round();
                Debug.Log(points + "points");
                isCorrect = false;
                Debug.Log("round 1 complete");
            }
            else if (round2 == false)
            {
                round2 = true;
                Round();
                Debug.Log("round 2 complete");
            }
            else if (round3 == false)
            {
                round3 = true;
                Debug.Log("round 3 complete");
            }
            else if (round3 == true)
            {               
                Debug.Log("training over");
            }
        }
    }

    private void Round()
    {/*
        if (Input.GetKeyDown(KeyCode.Space) && difficulty == 2)
        {
            //Debug.Log(difficultyAdjust + "diffadj");
            //The additional difficulty check, the for loop and the else if
            //can be deleted if it's not supposed play multiple instruments
            for (int i = 0; i < 2; i++)
            {
                ChooseSound(difficultyAdjust);
                audioSource.Play();
            }
        }*/
        if (difficulty == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                ChooseSound(difficultyAdjust);
                audioSource.Play();
            }
        }
        else if (difficulty <= 1)
        {
            ChooseSound(difficultyAdjust);
            audioSource.Play();
        }
    }

    private void SelectInstrument()
    {
        previousHit = hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out hit))
        {
            if (hit.collider.gameObject.tag == "playing")
            {
                if (Input.GetMouseButton(0))
                {
                    isCorrect = true;
                }
            }
            else
            {
                points -= 15;
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