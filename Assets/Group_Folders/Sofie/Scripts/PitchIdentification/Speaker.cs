using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public Animator animator;
    private Material OGMat;
    public AudioClip[] notes;
    AudioSource audioSource;
    public AudioClip currentClip;
    public int note;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        animator = GetComponent<Animator>();
        SetPickedState(false);
        OGMat = GetComponent<MeshRenderer>().material;

    }
    public void ResetMat()
    {
        GetComponent<MeshRenderer>().material = OGMat;
    }

    public void ResetCurrentNote()
    {
        note = 0;
        currentClip = null;
    }

    public void SetNote(int clip)
    {
        currentClip = notes[clip];
    }
    public void PlayClip()
    {
        audioSource.PlayOneShot(currentClip);
    }


    public void StopAudio()
    {
        audioSource.Stop();
    }



    public void SetPickedState(bool state)
    {
        animator.SetBool("Picked", state);
    }

    public void DestroyAnimation()
    {
        animator.SetTrigger("Destroy");
    }

    public void DestroySpeaker()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }

    public void SwitchMaterial(Material mat1, Material mat2)
    {
        StartCoroutine(MaterialSwitch(mat1, mat2));
    }
    IEnumerator MaterialSwitch(Material mat1, Material mat2)
    {
        GetComponent<MeshRenderer>().material = mat1;
        yield return new WaitForSeconds(0.7f);
        GetComponent<MeshRenderer>().material = mat2;
    }
}
