using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private Sprite activeLevelImage;
    [SerializeField]
    private Sprite lockedLevelImage;
    [SerializeField]
    private Image activeStar;
    [SerializeField]
    private Image lockedStar;

    [SerializeField]
    private GameObject[] levels;

    public void ActivateLevel(int level)
    {
            levelsText[level - 1].text = level.ToString();
            levelImages[level - 1].sprite = activeLevelImage;
            levels[level - 1].tag = "UnlockedLevel";
    }

    private void LockLevels(int levels)
    {
        for(int i = 0; i < levels; i++)
        {
            levelsText[i].text = "";
            levelImages[i].sprite = lockedLevelImage;
        }
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
