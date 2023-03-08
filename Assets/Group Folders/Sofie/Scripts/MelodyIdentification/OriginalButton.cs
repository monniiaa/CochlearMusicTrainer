using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalButton : MelodyButton
{
    protected override void SetPitch()
    {
        pitch = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Playing");
            PlayAudio(pitch);
        }
    }
}
