using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MelodyButton : MonoBehaviour
{

    [SerializeField]
    private LevelManager levelManager;
    protected int pitch;

    protected AudioSource source;
    public KeyCode key;
    private AudioSource[] allAudioSources;
    private void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        source = GetComponent<AudioSource>();
        SetPitch();
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
    protected abstract void SetPitch();

    public void PlayAudio(int pitch)
    {
        StopAllAudio();
        source.pitch = pitch;
        source.PlayOneShot(levelManager.levelMelody);
    }

}
