//======= Copyright (c) ANDALACA Corporation, All rights reserved. ===============
//
// Purpose: Handles dynamic game events related to user interaction such as update of UI, Camera Effects ect. ect.
//
//=============================================================================

using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    #region Internal Variables
    //Post-Processing
    private VolumeProfile volumeProfile; 
    private UnityEngine.Rendering.Universal.ColorAdjustments imageExposure;

    //Sound variables
    private AudioMixer masterMixer;
    private float normalSoundLevel;
    //UI Settings
    #endregion

    #region Static Variables
    //Interactive elements parrent

    //Static instance of script
    public static UIHandler Instance { get; private set; }   
    #endregion
  
    private void Awake() 
    {
        #region Singleton
        //Singleton pattern
        if (Instance == null) 
        {
            //For Variable Controller
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        #endregion

        //Post processing volume. 
        volumeProfile = gameObject.transform.parent.GetChild(1).GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        masterMixer = Resources.Load("ResonanceAudioMixer") as AudioMixer;

    }
    
    #region Camera / Sound Fade Effects
    //Camera fade, with extra delay parameter for level change. 
    public IEnumerator LevelChangeFade(float waitTimeBeforFade, bool fadedStatus, float timeToFade)
    {
        yield return new WaitForSeconds(waitTimeBeforFade);
        Fade(fadedStatus, timeToFade);
    }


    //Function that fades both sound and camera with same speed. 
    public void Fade(bool fadedStatus, float timeToFade)
    {
        StartCoroutine(CameraFade(fadedStatus, timeToFade));
        StartCoroutine(SoundFade(fadedStatus, timeToFade));
    }

    //Fade or unfades camera by manipulating the post processing effect postexposure depending on current status. 
    public IEnumerator CameraFade(bool fadedStatus, float timeToFade)
    {
        float timeElapsed = 0;
        float exposureLevel;
        
        if(fadedStatus == true)
        {
            while (timeElapsed < timeToFade)
            {
                if(!volumeProfile.TryGet(out imageExposure)) throw new System.NullReferenceException(nameof(imageExposure));
                exposureLevel = Mathf.Lerp(-15f, .45f, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                imageExposure.postExposure.Override(exposureLevel);
                yield return null;
            }

        } else if (fadedStatus == false)
        {
            while (timeElapsed < timeToFade)
            {
                if(!volumeProfile.TryGet(out this.imageExposure)) throw new System.NullReferenceException(nameof(imageExposure));
                exposureLevel = Mathf.Lerp(.45f, -15f, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                imageExposure.postExposure.Override(exposureLevel);
                yield return null;
            }

        }

    }
    
    public IEnumerator SoundFade(bool fadedStatus, float timeToFade)
    {
        float currentTime = 0;
        masterMixer.GetFloat("volume", out normalSoundLevel);
        normalSoundLevel = Mathf.Pow(10, normalSoundLevel / 20);

        if(fadedStatus == true)
        {
            float targetValue = Mathf.Clamp(1f, 0.0001f, 1);
            while (currentTime < timeToFade)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(normalSoundLevel, targetValue, currentTime / timeToFade);
                masterMixer.SetFloat("volume", Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        } else if (fadedStatus == false)
        {
            float targetValue = Mathf.Clamp(0.0001f, 0.0001f, 1);
            while (currentTime < timeToFade)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(normalSoundLevel, targetValue, currentTime / timeToFade);
                masterMixer.SetFloat("volume", Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }
    } 
    
    #endregion

}
