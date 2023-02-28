using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteChannel : MonoBehaviour
{
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (GameManager.Instance.channelToMute == 2)
        {
            return;
        }
        
        for (int i = GameManager.Instance.channelToMute; i < data.Length; i += channels)
        {
            data[i] = 0f;
        }
    }
}
