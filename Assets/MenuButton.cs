using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private int level;
    
     private Haptics haptics;
     private UISounds buttonSounds;
     [SerializeField] private GameObject completion;
    [SerializeField] UIMenu uiMenu;
    [SerializeField] private Animator animator;


    

    private void Start()
    {
        haptics = FindObjectOfType<Haptics>();
        buttonSounds = FindObjectOfType<UISounds>();
        level = int.Parse(gameObject.name);
        completion = GameObject.Find("completion" + level);
        animator = GameObject.Find("animation" + level).GetComponent<Animator>();

        completion.SetActive(true);
                
    }

    public void HoverInteraction()
    {
        if(level <= uiMenu.activeLevels)
        {
            completion.SetActive(true);
            animator.SetBool("Hover", true);
            haptics.SendHoverHaptics();
            buttonSounds.PlayHoverSound();
            
        }
    }
    
    public void HoverExitInteraction()
    {
        animator.SetBool("Hover", false);
    }



    public void SelectInteraction()
    {
        haptics.SendSelectHaptics();
            buttonSounds.PlaySelectSound();
    }
}
