using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonVariation : MelodyButton
{
    public bool isIdentical = false;

    protected override void SetPitch()
    {
        if (isIdentical)
        {
            pitch = 1;
        }
        else pitch = Random.Range(2, 5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            PlayAudio(pitch);
        }
    }
}
