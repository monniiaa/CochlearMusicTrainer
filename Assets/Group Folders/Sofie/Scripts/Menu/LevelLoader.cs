using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string path;
    
    GameData gameData;
    
    public void LoadLevel(int level)
    {
        gameData =  DataManager.ReadJson(path);
        gameData.level = level;
        gameData.levelScore[level - 1] = 0;
        DataManager.SaveDataToJson(gameData, path);
    }
}
