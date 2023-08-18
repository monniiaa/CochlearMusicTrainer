using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Haptics;

public class MemoryCard : MonoBehaviour
{
    public Outline hoverOutline;
    public GameObject spawnpoint;
    [SerializeField] public GameObject instrument;
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
        controller = FindObjectOfType<MelodyContoller>();

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
    
    public void SetAndSpawnInstrument(GameObject inst)
    {
        instrument = Instantiate(inst, spawnpoint.transform.position, spawnpoint.transform.rotation, spawnpoint.transform);
        instrument.transform.localScale = new Vector3(0.5f,0.5f, 0.5f);
        instrument.SetActive(false);
    }

    private void Pressed(SelectEnterEventArgs args)
    {
        if (!alreadyMatched)
        {
            controller.CardRevealed(this);
            hoverOutline.enabled = true;
            hoverOutline.OutlineColor = Color.yellow;
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
            GetComponent<MeshRenderer>().enabled = true;
            hoverOutline.enabled = false;
        }
    }

    public void Matched()
    {
        hoverOutline.OutlineColor = Color.green;
    }
    
    
    public void Reveal()
    {
        GetComponent<MeshRenderer>().enabled = false;
        instrument.SetActive(true);

    }
    private void EnteredTrigger(HoverEnterEventArgs arg0)
    {
        if (!alreadyMatched)
        {
            if (controller.firstRevealed != this)
            {
                hoverOutline.enabled = true;
                Color color = Color.yellow;
                color.a = 0.4f;
                hoverOutline.OutlineColor = color;
                imTouched = true;
            }
        }
    }

    private void ExitedTrigger(HoverExitEventArgs args0)
    {
        Reset();
    }

    public void Reset(){
        if(controller.firstRevealed != this)hoverOutline.enabled = false;
        imTouched = false;
        audioSource.Stop();
        StopHaptics();
}
}
