using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTextHandler : MonoBehaviour
{
    [SerializeField] private GameObject easyIntroText;
    [SerializeField] private GameObject mediumHardIntroText;
    
    GameDataManager gameDataManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameDataManager = GameDataManager.Instance;
        if (gameDataManager.currentLevel < 4)
        {
            easyIntroText.SetActive(true);
            mediumHardIntroText.SetActive(false);
        }
        else
        {
            easyIntroText.SetActive(false);
            mediumHardIntroText.SetActive(true);
        }
    }
    
}
