using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNavigation : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private string path;
    [SerializeField] private UIMenu uiMenu;
    private SceneTransitionManager sceneTransitionManager;
    private GameDataManager gameDataManager;
    private GameData gameData;
    private Button[] buttons;
    private Level[] levels;
    private void Awake() 
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
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
        if(level > gameData.level)
        {
            return;
        }
        gameDataManager.currentLevel = level;
        sceneTransitionManager.GoToSceneAsync(sceneIndex);
    }
}
