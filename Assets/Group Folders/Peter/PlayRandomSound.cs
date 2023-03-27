using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public Transform[] parentObjects; // array of parent objects to select from
    public int difficulty = 1; // the difficulty level (1-3)

    private List<GameObject> selectedObjects = new List<GameObject>(); // list of selected child objects

    private AudioSource[] allAudioSources;

    private bool round1 = false, round2 = false, round3 = false;

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
        // select up to 2 or 3 parent objects, depending on difficulty
        //int numParents = difficulty == 3 ? 3 : Random.Range(2, 4);
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
            Debug.Log("children" + randomChildIndex);
            GameObject child = parent.GetChild(randomChildIndex).gameObject;
            selectedObjects.Add(child);
        }
        //Debug.Log(selectedObjects.Count);
        foreach (GameObject obj in selectedObjects)
        {
            obj.SetActive(true);
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
                {
                    points += 50;
                    Debug.Log("test");
                }
                Round();
                Debug.Log("round 2 complete");
            }
            else if (round3 == false)
            {
                round3 = true;
                {
                    points += 50;
                    Debug.Log("test");
                }
                Debug.Log("round 3 complete");
            }
            else if (round3 == true)
            {
                Debug.Log("training over");
            }
        }
    }

    private void Start()
    {
        Round();
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
