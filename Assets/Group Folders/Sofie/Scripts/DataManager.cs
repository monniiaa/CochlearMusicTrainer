using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager 
{
    public static GameData CreateGameData(int numberOfLevels, int currentScore, int currentLevel)
    {
        GameData data = new GameData();
        data.levelScore = new int[numberOfLevels];
        data.levelScore[currentLevel -1] = currentScore;
        data.level = currentLevel;
        return data;
    }

    public static void SaveDataToJson(GameData data, string minigame)
    {
        string json = JsonUtility.ToJson(data);

        string filePath = Application.dataPath + "/MiniGameData/" + minigame + "/ScoreData.json";
        StreamWriter writer = new StreamWriter(filePath);

        writer.Write(json);
        writer.Close();
    }

    public static GameData ReadJson(string minigame)
    {
        string filePath = Application.dataPath + "/MiniGameData/" + minigame +  "/ScoreData.json";
        StreamReader reader = new StreamReader(filePath);
        string json = reader.ReadToEnd();
        reader.Close();
        GameData data = JsonUtility.FromJson<GameData>(json);
        return data;
    }
}
