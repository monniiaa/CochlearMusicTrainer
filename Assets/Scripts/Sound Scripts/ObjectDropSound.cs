using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectDropSound : MonoBehaviour
{
    public AudioClip[] audioClips; //For now assign in inspector
    public AudioClip audioClipToPlay;

    private void Awake(){
        audioClips = Resources.LoadAll<AudioClip>("Resources/Sounds/Drop");
        GetComponent<AudioSource>().spatialBlend = 1;
    }

    private void OnCollisionEnter(Collision other){

        if(other.gameObject.tag == "Surface") //Remember to set tag on floor mesh.
        {
          ItemSound(other);
        }
    }


    private void ItemSound(Collision other) //Needs to be adjusted so that the speed of the object determines between 3 different sounds.
    {

        if(other.relativeVelocity.magnitude >= 0.1 && other.relativeVelocity.magnitude < 0.5)
        {
            PlayAudio(0);
        }
        if(other.relativeVelocity.magnitude >= 0.5 && other.relativeVelocity.magnitude < 2.0)
        {
            PlayAudio(1);
        }
        if(other.relativeVelocity.magnitude >= 2.0)
        {
            PlayAudio(2);
        }
    }

    private void PlayAudio (int clipNumber) //Just for calling the selected audiosource from the array.
    {
        if(clipNumber == 0){
            GetComponent<AudioSource>().volume = 0.2f;
        } else if(clipNumber == 1){
            GetComponent<AudioSource>().volume = 0.6f;
        } else{
            GetComponent<AudioSource>().volume = 1.0f;
        }

        int index = Random.Range(0, audioClips.Length);
        audioClipToPlay = audioClips[index];
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClipToPlay;
        audio.Play();
    }
}
