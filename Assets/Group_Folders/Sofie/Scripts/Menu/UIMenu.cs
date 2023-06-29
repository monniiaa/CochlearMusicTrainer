using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] levelsText;
    [SerializeField]
    private Image[] scoreInfo;
    [SerializeField]
    private Image[] levelImages;
    [SerializeField]
    private GameObject[] levelInfo;
    [SerializeField]
    private int maxLevel = 10;

    [SerializeField]
    private Sprite activeLevelImage;
    [SerializeField]
    private Sprite lockedLevelImage;
    [SerializeField]
    private Image activeStar;
    [SerializeField]
    private Image lockedStar;

    [SerializeField]
    private GameObject[] levels;
    [SerializeField]
    private Image[] levelDots;
    [SerializeField]
    private Sprite unlockedDot;
    [SerializeField]
    private Sprite lockedDot;
    [SerializeField]
    private Sprite gradientDot;

    public int activeLevels;


    public void ActivateLevels(int level)
    {
        activeLevels = level;
        for (int i = 0; i < level; i++)
        {
            levelsText[i].text = (i+1).ToString();
            levelImages[i].sprite = activeLevelImage;
            levels[i].tag = "UnlockedLevel";
        }
        ActivateDots(level);
    }

    public void ActivateDots(int level)
    {
        for (int i = 0; i < (level - 1) * 3; i++)
        {
            levelDots[i].sprite = unlockedDot;

        }
        levelDots[(level - 1) * 3].sprite = unlockedDot;
        levelDots[((level - 1) * 3) + 1].sprite = gradientDot;
    }

    public void LockLevels(int levels)
    {
        for(int i = 0; i < levels; i++)
        {
            levelsText[i].text = "";
            levelImages[i].sprite = lockedLevelImage;
        }
        for (int i = 0; i < (levels - 1) * 3; i++)
        {
            levelDots[i].sprite = lockedDot;

        }
    }
    
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void ShowLevelInfo(int level, int score)
    { 
        levelInfo[level - 1].SetActive(true);
        SetStars(level, score);
    }

    public void HideLevelInfo(int level)
    {
        levelInfo[level - 1].SetActive(false);
    }

    public void SetStars(int level, int score)
    {
        switch (score)
        {
            case 0:
                for (int i = 0; i < 3; i++)
                {
                    Image star = Instantiate(lockedStar, levelInfo[level - 1].transform.position - new Vector3(0, 0, 0.13f) + new Vector3(0, 0, i * 0.14f), Quaternion.identity, levelInfo[level - 1].transform) ;
                    star.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                break;
            case 1:
                Image active = Instantiate(activeStar, levelInfo[level - 1].transform.position + new Vector3(0, 0, 0.13f), Quaternion.identity, levelInfo[level - 1].transform);
                active.transform.localRotation = new Quaternion(0, 0, 0, 0);
                for (int i = 1; i < 3; i++)
                {
                    Image star = Instantiate(lockedStar, levelInfo[level - 1].transform.position - new Vector3(0, 0, 0.13f  + 0.14f ) + new Vector3(0, 0, i * 0.14f), Quaternion.identity, levelInfo[level - 1].transform);
                    star.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                break;
            case 2:
                for (int i = 1; i < 3; i++)
                {
                    Image star = Instantiate(activeStar, levelInfo[level - 1].transform.position - new Vector3(0, 0, 0.13f) + new Vector3(0, 0, i * 0.14f), Quaternion.identity, levelInfo[level - 1].transform);
                    star.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                Image locked = Instantiate(lockedStar, levelInfo[level - 1].transform.position - new Vector3(0, 0, 0.13f), Quaternion.identity, levelInfo[level - 1].transform);
                locked.transform.localRotation = new Quaternion(0, 0, 0, 0);

                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    Image star = Instantiate(activeStar, levelInfo[level - 1].transform.position - new Vector3(0, 0, 0.13f) + new Vector3(0, 0, i * 0.14f), Quaternion.identity, levelInfo[level - 1].transform);
                    star.transform.localRotation = new Quaternion(0, 0, 0, 0);
                }
                break;
            default:
                break;
        }
    }

}
