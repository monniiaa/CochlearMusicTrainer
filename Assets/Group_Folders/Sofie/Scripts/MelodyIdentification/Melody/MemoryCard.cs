using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Haptics;

public class MemoryCard : MonoBehaviour
{

    [SerializeField] GameObject cardBack;
    [SerializeField] GameObject imHovered;
    [SerializeField] GameObject iAmPressed;
    [SerializeField] private MelodyContoller controller;
    
    private XRSimpleInteractable interactableHandler;
    private bool imTouched = false;
    public bool alreadyMatched = false;
    
    public AudioSource audioSource;
    private HapticClipPlayer hapticPlayer;
    private HapticClip hapticClip;
    public int numClicks = 0;
    public int _id;
    
    public int Id
    {
        get { return _id; }
    }

    private void Start()
    {
        interactableHandler = GetComponent<XRSimpleInteractable>();
        
        interactableHandler.hoverEntered.AddListener(EnteredTrigger);
        interactableHandler.hoverExited.AddListener(ExitedTrigger);
        interactableHandler.selectEntered.AddListener(Pressed);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetCard(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void SetAudio(int id, AudioClip sound)
    {
        _id = id;
        audioSource.clip = sound;
        //if(hapticsOn) hapticPlayer = new HapticClipPlayer(hapticClip);
    }
    public void StopHaptics()
    {
        if(hapticPlayer != null) hapticPlayer.Stop();
    }
    
    public void SetHaptics(HapticClip haptic)
    {
        hapticClip = haptic;
        hapticPlayer = new HapticClipPlayer(hapticClip);
    }

    private void Pressed(SelectEnterEventArgs args)
    {
        if (!alreadyMatched)
        {
            controller.CardRevealed(this);
            iAmPressed.SetActive(true);
            audioSource.Play();
            if(hapticPlayer != null) hapticPlayer.Play(HapticInstance.Hand.Right);

            numClicks++;
            imTouched = false;
        }    
    }
    public void Unreveal()
    {
        if (!alreadyMatched)
        {
            cardBack.SetActive(true);
            if (iAmPressed.activeSelf)
            {
                iAmPressed.SetActive(false);
            }
        }
    }

    public void Matched()
    {
        SpriteRenderer sprite = iAmPressed.GetComponent<SpriteRenderer>();
        sprite.color = Color.green;
    }
    
    
    public void Reveal()
    {
        cardBack.SetActive(false);

    }
    private void EnteredTrigger(HoverEnterEventArgs arg0)
    {
        if (!alreadyMatched)
        {
            imHovered.SetActive(true);
            imTouched = true;
        }
    }

    private void ExitedTrigger(HoverExitEventArgs args0)
    {
        imHovered.SetActive(false);
        imTouched = false;
        audioSource.Stop();
        StopHaptics();
    }
}
