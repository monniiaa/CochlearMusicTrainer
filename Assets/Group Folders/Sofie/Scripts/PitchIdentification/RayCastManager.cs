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

    private ToneGenerator[] tones;


    // Start is called before the first frame update
    void Start()
    {
        tones = GameObject.FindObjectsOfType<ToneGenerator>();
        pitchIdentification.SetPitchDifference(tones);
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
         //  && PreviousHit.collider.gameObject != hit.collider.gameObject
            if (hit.collider.gameObject.tag == "Speaker" )
            {
                if (Input.GetMouseButtonDown(0))
                {
                   
                    Debug.Log("HIT");
                    ToneGenerator tone = hit.collider.gameObject.GetComponent<ToneGenerator>();
                    if (tone.playing == false)
                    {
                        foreach (ToneGenerator t in tones)
                        {
                            t.StopAudio();
                           t.gameObject.GetComponent<Speaker>().SetPickedState(false);



                        }
                        tone.PlayAudio();
                        hit.collider.gameObject.GetComponent<Speaker>().SetPickedState(true);


                    }
                    else if (tone.playing == true)
                    {
                        tone.StopAudio();
                        hit.collider.gameObject.GetComponent<Speaker>().SetPickedState(false);
                    }
                }
            }

            }
            
            
        
    }
}
