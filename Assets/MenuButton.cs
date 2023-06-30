using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    private int level;
    
     private Haptics haptics;
     private UISounds buttonSounds;
     [SerializeField] private GameObject completion;
    [SerializeField] UIMenu uiMenu;
    [SerializeField] private Animator animator;


    

    private void Awake()
    {
        haptics = FindObjectOfType<Haptics>();
        buttonSounds = FindObjectOfType<UISounds>();
        level = int.Parse(gameObject.name);
        completion = GameObject.Find("completion" + level);
        animator = GameObject.Find("animation" + level).GetComponent<Animator>();
        uiMenu = FindObjectOfType<UIMenu>();
        completion.SetActive(true);
    }
    
    private void Start()
    {
        CreateTrigger();
    }

    private void CreateTrigger()
    {

            if(GetComponent<EventTrigger>() == null)
            {
                gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
            
            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.Select;
            entry3.callback.AddListener((data) => { OnSelectDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);
            trigger.triggers.Add(entry2);
            trigger.triggers.Add(entry3);
    }

    private void OnPointerExitDelegate(PointerEventData arg0)
    {
        HoverExitInteraction();
    }

    private void OnSelectDelegate(PointerEventData arg0)
    {
        SelectInteraction();
    }

    private void OnPointerDownDelegate(PointerEventData arg0)
    {
        HoverInteraction();
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

    private void OnDisable()
    {
        animator.SetBool("Hover", false);
        completion.SetActive(false);
    }

    public void SelectInteraction()
    {
        haptics.SendSelectHaptics();
            buttonSounds.PlaySelectSound();
    }
}
