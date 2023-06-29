using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Level : MonoBehaviour
{
    private Sprite activeStar;
    private Sprite inactiveStar;
    private Image[] images;
    private GameData gameData;
    private void Awake() 
    {
        activeStar = Resources.Load<Sprite>("MenuUI/ActiveStar");
        inactiveStar = Resources.Load<Sprite>("MenuUI/InactiveStar");
        images = GetComponentsInChildren<Image>(true).Where(t => t.transform != transform).ToArray(); 
    }

    public void SetStars(int score)
    {
        for (int i = 0; i < score; i++)
        {
            images[i].sprite = activeStar;
        }
    }
}
