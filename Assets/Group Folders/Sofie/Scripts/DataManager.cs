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
        data.levelScore[currentLevel - 1] = currentScore;
        data.level = currentLevel;
        return data;
    }

    public static void SaveDataToJson(GameData data, string minigame)
    {
        string json = JsonUtility.ToJson(data);
        string filePath;
#if UNITY_EDITOR
        filePath = Application.dataPath + "/MiniGameData/" + minigame + "/ScoreData.json";
#elif PLATFORM_ANDROID
        filePath = Application.persistentDataPath + "/MiniGameData/" + minigame + "/ScoreData.json";
#endif
        StreamWriter writer = new StreamWriter(filePath);

        writer.Write(json);
        writer.Close();
    }

    public static GameData ReadJson(string minigame)
    {
        GameData data = CreateGameData(10, 0, 1);
        string filePath;

#if UNITY_EDITOR
        filePath = Application.dataPath + "/MiniGameData/" + minigame + "/ScoreData.json";
#elif PLATFORM_ANDROID
        filePath = Application.persistentDataPath + "/MiniGameData/" + minigame + "/ScoreData.json";
#endif

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            FileStream streamfile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            streamfile.Close();

            SaveDataToJson(data, minigame);
        }
        StreamReader reader = new StreamReader(filePath);
        string json = reader.ReadToEnd();
        reader.Close();

        string jsonData = File.ReadAllText(filePath);
        if (jsonData != string.Empty)
        {
            data = JsonUtility.FromJson<GameData>(json);
        }
        
        return data;
    }
}

