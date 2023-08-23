using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;
using Oculus.Platform.Samples.VrHoops;

public class HapticsBehavior : MonoBehaviour
{
    [SerializeField] HapticClip[] instrumentClips;
    private HapticClipPlayer player;
    
    public string folderName;

    private void Awake()
    {
        folderName = "HapticsXRNew/" + gameObject.name + "_Haptics"; // assume the sound folder is named after the child object
        instrumentClips = Resources.LoadAll<HapticClip>(folderName);
    }

    public void PlayHapticsClip(int clipIndex)
    {
        player = new HapticClipPlayer(instrumentClips[clipIndex]);
        player.Play(HapticInstance.Hand.Right);
    }
    
    public void StopHaptics()
    {
        if (player != null)
        {
            player.Stop();
        }
    }

}
