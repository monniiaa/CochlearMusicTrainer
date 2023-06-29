using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class LevelData
{
    public int level;
    public int score;
}

public class ScoreManagerChatGPT : MonoBehaviour
{

    public Text scoreText;
    public Text timeText;
    public Text incorrectText;

    private int score = 0;
    private float time = 0.0f;
    private int incorrect = 0;
    private int level = 1;
    private bool gameOver = false;

    private const string LEVEL_DATA_FILE = "level_data.json";
    private List<LevelData> levelDataList = new List<LevelData>();

    void Start()
    {
        LoadData();
    }

    void Update()
    {
        if (!gameOver)
        {
            time += Time.deltaTime;
            timeText.text = "Time: " + time.ToString("F2");
        }
    }

    public void AddScore(int points)
    {
        if (!gameOver)
        {
            score += points;
            scoreText.text = "Score: " + score.ToString();

            if (score >= level * 100)
            {
                LevelUp();
            }
        }
    }

    public void AddIncorrect()
    {
        if (!gameOver)
        {
            incorrect++;
            incorrectText.text = "Incorrect: " + incorrect.ToString();
            score -= 10;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    private void LevelUp()
    {
        level++;
        Debug.Log("Level Up! Current Level: " + level.ToString());
        SaveData();
        // Add code to increase difficulty or progress to the next level
    }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over! Final Score: " + score.ToString());
        SaveData();
        // Add code to display game over screen or restart the game
    }

    private void SaveData()
    {
        LevelData levelData = new LevelData();
        levelData.level = level;
        levelData.score = score;

        // Add new level data or update existing level data
        bool found = false;
        for (int i = 0; i < levelDataList.Count; i++)
        {
            if (levelDataList[i].level == level)
            {
                levelDataList[i] = levelData;
                found = true;
                break;
            }
        }

        if (!found)
        {
            levelDataList.Add(levelData);
        }

        // Save level data to file
        string dataAsJson = JsonUtility.ToJson(levelDataList);
        File.WriteAllText(Application.persistentDataPath + "/" + LEVEL_DATA_FILE, dataAsJson);
    }

    private void LoadData()
    {
        // Load level data from file
        if (File.Exists(Application.persistentDataPath + "/" + LEVEL_DATA_FILE))
        {
            string dataAsJson = File.ReadAllText(Application.persistentDataPath + "/" + LEVEL_DATA_FILE);
            levelDataList = JsonUtility.FromJson<List<LevelData>>(dataAsJson);

            // Find current level data
            LevelData currentLevelData = null;
            for (int i = 0; i < levelDataList.Count; i++)
            {
                if (levelDataList[i].level == level)
                {
                    currentLevelData = levelDataList[i];
                    break;
                }
            }

            // Set current level data
            if (currentLevelData != null)
            {
                score = currentLevelData.score;
            }
        }
    }
}