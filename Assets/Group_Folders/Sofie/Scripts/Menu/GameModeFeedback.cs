using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeFeedback : MonoBehaviour
{
    [SerializeField] private string gameModeName;
    [SerializeField] private Image hoverImage, selectImage;
    
    [SerializeField] private Sprite hoverLevelImage;
    [SerializeField] private Sprite selectLevelImage;
    private void Awake()
    {
        selectLevelImage = Resources.Load<Sprite>("MenuUI/SelectOutline");
        hoverLevelImage = Resources.Load<Sprite>("MenuUI/HoverOutline");
        
      //  feedbackImage = GetComponentInChildren<Image>(true);

      gameModeName = gameObject.name;
        hoverImage = GameObject.Find(gameModeName + "Hover").GetComponent<Image>();
        selectImage = GameObject.Find(gameModeName + "Selected").GetComponent<Image>();
        
        hoverImage.gameObject.SetActive(false);
        hoverImage.sprite = hoverLevelImage;
        
        selectImage.gameObject.SetActive(false);
        selectImage.sprite = selectLevelImage;
    }
    
    private void Start()
    {
        EventTrigger();
    }

    void EventTrigger()
    {
        if(gameObject.GetComponent<EventTrigger>() == null)
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
        hoverImage.gameObject.SetActive(false);
    }

    private void OnSelectDelegate(PointerEventData arg0)
    {
        hoverImage.gameObject.SetActive(false);
        selectImage.gameObject.SetActive(true);
    }

    private void OnPointerDownDelegate(PointerEventData arg0)
    {
        hoverImage.gameObject.SetActive(true);
        
    }
}
