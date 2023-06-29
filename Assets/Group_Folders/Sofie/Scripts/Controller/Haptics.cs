using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Haptics : MonoBehaviour
{
    public XRBaseController rightController;
    
    private float hoverAmplitude = 0.1f;
    private float hoverDuration = 0.1f;
    
    private float selectAmplitude = 0.2f;
    private float selectDuration = 0.2f;

    public void SendHoverHaptics()
    {
        rightController.SendHapticImpulse(hoverAmplitude, hoverDuration);
    }
    
    public void SendSelectHaptics()
    {
        rightController.SendHapticImpulse(selectAmplitude, selectDuration);
    }
}
