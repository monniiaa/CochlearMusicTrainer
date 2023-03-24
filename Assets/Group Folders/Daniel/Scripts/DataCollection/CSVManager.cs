using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    private void Start()
    {
        SaveScore(10);
    }

    public void SaveScore(float score)
    {
        /*
        if (File.ReadLines(_csvFiles.Path).FirstOrDefault() == string.Empty)
        {
            CSVHelper.InsertColumnNames(_csvFiles.Path, "Date", "Score", "NewThing");
        }
        */
        using (StreamWriter sw = File.AppendText(_csvFiles.Path))
        {
            sw.WriteLine("Hello");
            sw.WriteLine("And");
            sw.WriteLine("Welcome");
        }
    }
    
    public void SaveProgress(float score)
    {
        
    }
}
