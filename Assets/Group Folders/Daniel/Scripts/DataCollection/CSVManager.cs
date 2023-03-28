using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Text;

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
    }

    private void Start()
    {
        SaveMiniGameData(new TimbreIdentification(DateTime.Today - DateTime.Today, 1));
    }

    public void SaveMiniGameData(IMiniGameData data)
    {
        if (!File.Exists(data.Path))
        {
            SetupDataFile($"{Application.dataPath}/TestDataFiles" + data.Path, data.CsvColumns, data.FileName);
        }
        using (StreamWriter stream = File.AppendText(data.Path))
        {
            stream.WriteLine(data.ToCsv());
        }
    }

    private void SetupDataFile(string path, string columns, string fileName)
    {
        Directory.CreateDirectory(path);
        FileStream fileStream = File.Create(path + fileName + ".csv");
        fileStream.Write(new UTF8Encoding().GetBytes(columns));
        fileStream.Close();
    }
}
