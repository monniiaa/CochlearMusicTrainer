using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class CSVManager : MonoBehaviour
{
    private CSVFile _csvFiles;

    private static CSVManager _instance;

    public static CSVManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<CSVManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _csvFiles = new CSVFile($"{Application.dataPath}/TestDataFiles/player_data.csv");
        FileStream stream = File.Create(_csvFiles.Path);
        stream.Close();
    }

    public void SaveScore(float score)
    {
        if (File.ReadLines(_csvFiles.Path).FirstOrDefault() == string.Empty)
        {
            CSVHelper.InsertColumnNames(_csvFiles.Path, "score");
        }
    }
    
    public void SaveProgress(float score)
    {
        
    }
}
