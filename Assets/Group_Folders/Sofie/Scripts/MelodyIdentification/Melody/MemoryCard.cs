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
    public int _id;
    private Animator animator;
    private MelodyContoller _controller;
    public int numClicks { get; private set; }
    private MemoryCard _lastMatched;
    
    
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
        animator = GetComponent<Animator>();
        controller = FindObjectOfType<MelodyContoller>();
        numClicks = 0;
    }

    private void OnEnable()
    {
        _controller = FindObjectOfType<MelodyContoller>();
        _controller.OnLastTouchedEvent += LastCardMatched;
    }
    
    private void OnDisable()
    {
        _controller.OnLastTouchedEvent -= LastCardMatched;
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
        animator.SetTrigger("Destroy");
        instrument.SetActive(true);

    }
    private void EnteredTrigger(HoverEnterEventArgs arg0)
    {
        if (!alreadyMatched)
        {
            if (controller.firstRevealed != this)
            {
                if(_lastMatched != null && _lastMatched.audioSource.isPlaying) _lastMatched.audioSource.Stop();
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
        if (!alreadyMatched) audioSource.Stop();
        StopHaptics();
    }

    private void LastCardMatched(MemoryCard card)
    {
        _lastMatched = card;
    }
    
}
