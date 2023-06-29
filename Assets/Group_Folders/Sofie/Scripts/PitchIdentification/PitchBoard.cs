using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchBoard : MonoBehaviour
{
    public UIMenu pitchModeMenu;
    public string path = "PitchIdentification";
    GameData levelData;

    void Awake()
    {
        levelData = DataManager.ReadJson(path);

        pitchModeMenu.LockLevels(levelData.levelScore.Length);
        pitchModeMenu.ActivateLevels(levelData.level);
    }

}
