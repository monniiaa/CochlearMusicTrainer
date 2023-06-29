using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] _audioSources;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        throw new NotImplementedException();
    }
}
