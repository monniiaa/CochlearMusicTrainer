using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNavigation : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private UIMenu uiMenu;
    private GameDataManager gameDataManager;
    private GameData gameData;
    private Button[] buttons;
    private Level[] levels;
    private void Awake() 
    {
        gameData = DataManager.ReadJson(path);
        gameDataManager = GameDataManager.Instance;
        buttons = GetComponentsInChildren<Button>();
        levels = GetComponentsInChildren<Level>(true);
    }

    private void Start() 
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject parent = levels[i].transform.parent.parent.parent.gameObject;
            parent.SetActive(true);
            levels[i].SetStars(gameData.levelScore[i]);
            parent.SetActive(false);
        }
    }

    private void OnEnable() 
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int value = i + 1;
            buttons[i].onClick.AddListener(delegate { SetCurrentLevel(value); });

        }
    }

    private void OnDisable() 
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int value = i + 1;
            buttons[i].onClick.RemoveListener(delegate { SetCurrentLevel(value); });
        }
    }

    private void SetCurrentLevel(int level)
    {
        gameDataManager.currentLevel = level;
    }
}
