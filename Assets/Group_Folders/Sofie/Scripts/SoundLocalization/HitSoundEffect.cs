using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HitSoundEffect : MonoBehaviour
{
   [SerializeField] private XRRayInteractor playerRayInteractior;
    [SerializeField] ActionBasedController controller;
    [SerializeField] private SphereCollider _sphereCollider;

    // AudioClip that should be played when the speaker is hit
    [SerializeField]
    public AudioClip speakerHitSound;
    [SerializeField]
    public AudioClip speakerMissSound;

    // Audio source component for playing the audio clip
    private AudioSource audioSource;
    // Boolean to keep track if the audio clip has been played
    private bool hasPlayed = false;


    private void Awake()
    {
        controller = FindObjectOfType<ActionBasedController>();
        playerRayInteractior = FindObjectOfType<XRRayInteractor>();
        _sphereCollider = GetComponent<SphereCollider>();
        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        controller.selectAction.action.performed += OnRay;
    }

    private void OnDisable()
    {
        controller.selectAction.action.performed -= OnRay;
    }

    private void OnRay(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(playerRayInteractior.rayOriginTransform.position, playerRayInteractior.rayOriginTransform.forward);
        if(_sphereCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            audioSource.clip = speakerHitSound;
        }
        else
        {
            audioSource.clip = speakerMissSound;
        }
        audioSource.loop = false;
        audioSource.Play();
    }
}






