using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedback : MonoBehaviour
{
    [SerializeField] private UISounds buttonSounds;
    [SerializeField] private Haptics haptics;

    private void Start()
    {
        haptics = FindObjectOfType<Haptics>();
        buttonSounds = FindObjectOfType<UISounds>();
        CreateTrigger();
    }

    private void CreateTrigger()
    {
        if(gameObject.GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        
            
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.Select;
        entry3.callback.AddListener((data) => { OnSelectDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry3);
    }

    private void OnSelectDelegate(PointerEventData arg0)
    {
        buttonSounds.PlaySelectSound();
        haptics.SendSelectHaptics();
    }


    private void OnPointerDownDelegate(PointerEventData arg0)
    {
        buttonSounds.PlayHoverSound();
        haptics.SendHoverHaptics();
    }
}
