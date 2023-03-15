using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayCastManager : MonoBehaviour
{
    private Camera camera;
    public PitchIdentification pitchIdentification;
    RaycastHit hit;
    RaycastHit PreviousHit;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        DetectObjectWithRaycast();
    }

    public void DetectObjectWithRaycast()
    {
        PreviousHit = hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
           
            if (hit.collider.gameObject.tag == "Speaker" && PreviousHit.collider.gameObject != hit.collider.gameObject)
            {
                Debug.Log("HIT");
                Speaker speaker = hit.collider.gameObject.GetComponent<Speaker>();
                if (speaker.audioSource.isPlaying == false)
                {
                    speaker.PlaySound();
                }
                
            }
            if (hit.collider.gameObject.tag == "Speaker")
            {
                Speaker speaker = hit.collider.gameObject.GetComponent<Speaker>();
                if (Input.GetMouseButtonDown(0))
                {
                    pitchIdentification.SpeakerPicked(speaker);
                }
            }
            
        }
    }
}
