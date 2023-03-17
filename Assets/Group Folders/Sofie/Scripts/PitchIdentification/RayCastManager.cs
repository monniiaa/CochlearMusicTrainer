using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayCastManager : MonoBehaviour
{
    public Camera camera;
    public PitchIdentification pitchIdentification;
    RaycastHit hit;
    RaycastHit PreviousHit;

    private Speaker[] speakers;

    private Speaker pickedSpeaker;




    // Start is called before the first frame update
    void Start()
    {
        speakers = GameObject.FindObjectsOfType<Speaker>();
        pitchIdentification.StartRound(speakers);
    }

    // Update is called once per frame
    void Update()
    {
        DetectObjectWithRaycast();
       if(Input.GetKeyDown(KeyCode.Space)) {
            pitchIdentification.EndRound(pickedSpeaker, speakers);
            pickedSpeaker.StopAudio();
            pickedSpeaker = null;
        }
    }

    public void DetectObjectWithRaycast()
    {
        PreviousHit = hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
         //  && PreviousHit.collider.gameObject != hit.collider.gameObject
            if (hit.collider.gameObject.tag == "Speaker" )
            {
                if (Input.GetMouseButtonDown(0))
                {
                   
                    Debug.Log("HIT");
                    Speaker speaker = hit.collider.gameObject.GetComponent<Speaker>();
                    pickedSpeaker = speaker;
                    if (speaker.playing == false)
                    {
                        foreach (Speaker s in speakers)
                        {
                           s.StopAudio();
                           s.SetPickedState(false);
                        }
                        speaker.PlayAudio();
                        speaker.SetPickedState(true);
                    }
                    else if (speaker.playing == true)
                    {
                        speaker.StopAudio();
                        speaker.SetPickedState(false);
                    }
                }
            }

            }
            
            
        
    }
}
