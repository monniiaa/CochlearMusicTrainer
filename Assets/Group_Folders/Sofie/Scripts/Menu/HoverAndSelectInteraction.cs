using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class HoverAndSelectInteraction : MonoBehaviour
{
    [SerializeField] private Button[] gameModes;
    [SerializeField] private UISounds _uiSounds;
    [SerializeField] private Haptics _controllerHaptics;

    [SerializeField] private Sprite hoverLevelImage;
    [SerializeField] private Sprite selectLevelImage;

    private void Awake()
    {
        gameModes = GetComponentsInChildren<Button>();
        _uiSounds = FindObjectOfType<UISounds>();
        _controllerHaptics = GameObject.FindWithTag("GameController").gameObject.GetComponent<Haptics>();


        selectLevelImage = Resources.Load<Sprite>("MenuUI/SelectOutline");
        hoverLevelImage = Resources.Load<Sprite>("MenuUI/HoverOutline");
        
    }

    private void Start()
    {
        CreateEventTrigger();
    }

    private void CreateEventTrigger()
    {
        for(int i = 0; i< gameModes.Length; i++)
        {
            if(gameModes[i].GetComponent<EventTrigger>() == null)
            {
                gameModes[i].AddComponent<EventTrigger>();
            }
            EventTrigger trigger = gameModes[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            

            
            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.Select;
            entry3.callback.AddListener((data) => { OnSelectDelegate((PointerEventData)data); });
            trigger.triggers.Add(entry);
            trigger.triggers.Add(entry3);
        }
    }



    private void OnSelectDelegate(PointerEventData arg0)
    {
        _uiSounds.PlaySelectSound();
        _controllerHaptics.SendSelectHaptics();

    }
    

    private void OnPointerDownDelegate(PointerEventData arg0)
    {
        _uiSounds.PlayHoverSound();
        _controllerHaptics.SendHoverHaptics();
    }
}
