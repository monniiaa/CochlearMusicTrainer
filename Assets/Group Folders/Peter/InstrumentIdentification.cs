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
    // Add more entries as needed
};
    [SerializeField] private bool mediumDifficulty;
    private int difficultyAdjust;

    private AudioSource audioSource;

    private void Awake()
    {
        switch (mediumDifficulty)
        {
            case (true):
                difficultyAdjust = 0;
            break;

            default:
                difficultyAdjust = 1;
            break;
        }
        Debug.Log(objectToFolderMap);
    }

    private void ChooseSound(int difficulty)
    {
        Debug.Log(objectsToChooseFrom.Length - difficulty + " test");
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
        Debug.Log(randomIndex + "rand");
        Debug.Log(objectsToChooseFrom.Length + "length");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(difficultyAdjust + "diffadj");
            ChooseSound(difficultyAdjust);
            audioSource.Play();
           /* if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("No audio clip specified in Script");
            }*/
        }
    }
}
