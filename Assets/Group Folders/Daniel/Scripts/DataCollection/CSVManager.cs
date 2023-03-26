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
        
        _csvFiles = new CSVFile($"{Application.dataPath}/TestDataFiles/player_data.csv", "Date,Month,Year".Split(","));
        SaveScore(10);
    }

    private void Start()
    {
        
    }

    public void SaveLocalizationPerformance(SoundLocalizationData data)
    {
        IMiniGameData miniGameData = new SoundLocalizationData(1, 1);
    }
    
    public void SaveScore(float score)
    {
        using (StreamWriter stream = File.AppendText(_csvFiles.Path))
        {
            for (int i = 0; i < 10; i++)
            {
                stream.WriteLine("Hello,No,Yes");
            }
        }
    }
    
    public void SaveProgress(float score)
    {
        
    }
}
