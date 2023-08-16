using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeManager : MonoBehaviour
{
    private Sprite noBadge;
    private Sprite bronzeBadge;
    private Sprite silverBadge;
    private Sprite goldBadge;
    
    private GameData gameData;

    [SerializeField] public Image badgeImage;
    [SerializeField] private string path;
    private void Start()
    {
        noBadge = Resources.Load<Sprite>("Badges/noBadge");
        bronzeBadge = Resources.Load<Sprite>("Badges/Bronze");
        silverBadge = Resources.Load<Sprite>("Badges/Silver");
        goldBadge = Resources.Load<Sprite>("Badges/Gold");
        gameData = DataManager.ReadJson(path);
        
        SetBadge();
    }

    private void SetBadge()
    {
        Debug.Log(path + ": " + gameData.level);
        if (gameData.level == 10)
        {
            int count = 0;
            for (int i = 0; i < gameData.levelScore.Length; i++)
            {
                if (gameData.levelScore[i] == 3)
                {
                    count++;
                }
            }

            if (count == 10)
            {
                badgeImage.sprite = goldBadge;
            }
            else if (count >= 1  && gameData.levelScore[9] > 0)
            {
                badgeImage.sprite = silverBadge;
            }
            else
            {
                badgeImage.sprite = noBadge;
            }
        }
        else
        {
            int count = 0;
            for (int i = 0; i < gameData.levelScore.Length; i++)
            {
                if (gameData.levelScore[i] == 3)
                {
                    count++;
                }
            }

            if (count >= 1)
            {
                badgeImage.sprite = bronzeBadge;
            }
            else
            {
                badgeImage.sprite = noBadge;
            }
        }
    }
}
