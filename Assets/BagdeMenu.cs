using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagdeMenu : MonoBehaviour
{
    [SerializeField] BadgeManager badgeManager;

    [SerializeField] private Image goldImage;
    [SerializeField] private Image silverImage;
    [SerializeField] private Image bronzeImage;
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI silverText;
    [SerializeField] private TextMeshProUGUI bronzeText;

    private Sprite noBadge;
    private Sprite bronzeBadge;
    private Sprite silverBadge;
    private Sprite goldBadge;

    private void Start()
    {
        noBadge = Resources.Load<Sprite>("Badges/noBadge");
        bronzeBadge = Resources.Load<Sprite>("Badges/Bronze");
        silverBadge = Resources.Load<Sprite>("Badges/Silver");
        goldBadge = Resources.Load<Sprite>("Badges/Gold");
        reset();
        SetTextAndImage();
    }

    private void SetTextAndImage()
    {
        if (badgeManager.badgeImage.sprite == goldBadge)
        {
            SetGold();
            SetSilver();
            SetBronze();
        } else if (badgeManager.badgeImage.sprite == silverBadge)
        {
            SetSilver();
            SetBronze();
        } else if (badgeManager.badgeImage.sprite == bronzeBadge)
        {
            SetBronze();
        } else if(badgeManager.badgeImage.sprite == noBadge)
        {
            reset();
        }
    }
    
    private void SetGold()
    {
        goldImage.sprite = goldBadge;
        goldText.text = "Lås op for alle levels og få 3 stjerner for at opnå denne badge";
    }

    private void SetSilver()
    {
        silverImage.sprite = silverBadge;
        silverText.text = "Lås op for alle levels for at opnå denne badge";
    }
    
    private void SetBronze()
    {
        bronzeImage.sprite = bronzeBadge;
        bronzeText.text = "Løs en level med 3 stjerner for at opnå denne badge";
    }

    private void reset()
    {
        goldImage.sprite = noBadge;
        silverImage.sprite = noBadge;
        bronzeImage.sprite = noBadge;
        
        goldText.text = "Låst";
        silverText.text = "Låst";
        bronzeText.text = "Låst";
    }
}
