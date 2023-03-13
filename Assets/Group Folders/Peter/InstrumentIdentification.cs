using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentIdentification : MonoBehaviour
{
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
    [SerializeField] private int difficulty;
    private int difficultyAdjust;

    private AudioSource audioSource;

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
        //Debug.Log(objectToFolderMap);
    }

    private void ChooseSound(int difficulty)
    {
        //Debug.Log(objectsToChooseFrom.Length - difficulty + " test");
        // Choose a random object from the list
        int randomIndex = Random.Range(0, objectsToChooseFrom.Length - difficulty);
        GameObject chosenObject = objectsToChooseFrom[randomIndex];

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
                else
                {
                    Debug.LogError("AudioSource component not found on game object: " + chosenObject.name);
                }
            }
            else
            {
                Debug.LogWarning("No audio clips found in folder: " + folderName);
            }
        }
        else
        {
            Debug.LogError("No folder name found for game object: " + chosenObject.name);
        }
        //Debug.Log(randomIndex + "rand");
        //Debug.Log(objectsToChooseFrom.Length + "length");
    }

    private void Update()
    {
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
           /* if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No audio clip specified in Script");
            }*/
        }
        else if (Input.GetKeyDown(KeyCode.Space) && difficulty >= 1)
        {
            ChooseSound(difficultyAdjust);
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StopMusic();
        }
    }

    private void StopMusic()
    {
        //Incorporate picking method here, so we can stop the current music and get ready for new or switch level?
        audioSource.Stop();
    }
}
