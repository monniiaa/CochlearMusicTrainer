using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Level : MonoBehaviour
{
    private Sprite activeStar;
    private Sprite inactiveStar;
    [SerializeField]
    private Image[] images;
    private GameData gameData;
    private void Awake() 
    {
        activeStar = Resources.Load<Sprite>("MenuUI/ActiveStar");
        inactiveStar = Resources.Load<Sprite>("MenuUI/InactiveStar");
        images = GetComponentsInChildren<Image>(true).ToArray();
       
    }

    public void SetStars(int score)
    {
        for (int i = 1; i < score + 1; i++)
        {
             images[i].sprite = activeStar;
        }
    }
}
