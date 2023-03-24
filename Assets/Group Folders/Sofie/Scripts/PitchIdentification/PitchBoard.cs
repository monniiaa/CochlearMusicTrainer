using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PitchBoard : MonoBehaviour
{

    public UIMenu pitchModeMenu;
    public string path = "PitchIdentification";
    GameData levelData;
    // Start is called before the first frame update
    void Start()
    {
        levelData = DataManager.ReadJson(path);

        pitchModeMenu.LockLevels(levelData.levelScore.Length);
        pitchModeMenu.ActivateLevels(levelData.level);
           // pitchModeMenu.ShowLevelInfo(i, levelData.levelScore[i - 1]);
        
        
    }

}
