using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootstepSounds : MonoBehaviour
{

public CheckIfGrounded checkIfGrounded;
public CheckTerrainTexture checkTerrainTexture;
public AudioSource audioSource;

public AudioClip[] pathClips;
public AudioClip[] dirtClips;
public AudioClip[] sandClips;

public AudioClip[] pathClipsCI;
public AudioClip[] dirtClipsCI;
public AudioClip[] sandClipsCI;

AudioClip previousClip;

private GameObject character;
float currentSpeed;
Vector3 lastPosition;
float distanceCovered;
public float modifier=0.5f;

    // Update is called once per frame
    private void Update()
    {
        currentSpeed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;

        if(currentSpeed > 0)
        {
            distanceCovered += (currentSpeed * Time.deltaTime) * modifier;
            if (distanceCovered > 0.005f)
            {
                TriggerNextClip();
                distanceCovered = 0;
            }
        }
    }

    AudioClip GetClipFromArray(AudioClip[] clipArray)
    {
        int attempts=3;
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Length-1)];

        while(selectedClip == previousClip && attempts > 0)
        {
            selectedClip = clipArray[Random.Range(0, clipArray.Length-1)];
            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }

    void TriggerNextClip()
    {
        audioSource.volume = Random.Range(0.1f,0.4f);

        if (GameManager.Instance.currentCondition == 0)
        {
            audioSource.pitch = Random.Range(0.8f,1.5f);
            
            if (checkIfGrounded.hit.collider != null && SceneManager.GetActiveScene().name == "Inside_Scene")
            {
                audioSource.volume = Random.Range(0.05f,0.1f);
                audioSource.PlayOneShot(GetClipFromArray(pathClips), 1);
            }
            
            if (checkIfGrounded.hit.collider != null && checkIfGrounded.hit.collider.tag == "path")
            {
                audioSource.PlayOneShot(GetClipFromArray(pathClips), 1);
            }

            if (checkIfGrounded.hit.collider != null && checkIfGrounded.hit.collider.tag == "sand")
            {
                audioSource.PlayOneShot(GetClipFromArray(sandClips), 1);
            } else
            {
                audioSource.PlayOneShot(GetClipFromArray(dirtClips), 1);
            }
        } else 
        {
            audioSource.pitch = Random.Range(0.8f,1.5f);
            
            if (checkIfGrounded.hit.collider != null && SceneManager.GetActiveScene().name == "Inside_Scene")
            {
                audioSource.volume = Random.Range(0.05f,0.1f);
                audioSource.PlayOneShot(GetClipFromArray(pathClipsCI), 1);
            }
        
            if (checkIfGrounded.hit.collider != null && checkIfGrounded.hit.collider.tag == "path")
            {
                audioSource.PlayOneShot(GetClipFromArray(pathClipsCI), 1);
            }

            if (checkIfGrounded.hit.collider != null && checkIfGrounded.hit.collider.tag == "sand")
            {
                audioSource.PlayOneShot(GetClipFromArray(sandClipsCI), 1);
            } else 
            {
                audioSource.PlayOneShot(GetClipFromArray(dirtClipsCI), 1);
            }
        }
    }
}
