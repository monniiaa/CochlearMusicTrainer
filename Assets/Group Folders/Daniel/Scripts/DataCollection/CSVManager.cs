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

    public void SaveMiniGameData(IMiniGameData data)
    {
        if (!File.Exists(data.Folder))
        {
            SetupDataFile($"{Application.dataPath}/TestDataFiles" + data.Folder, data.FileName, data.CsvColumns);
        }
        using (StreamWriter stream = File.AppendText($"{Application.dataPath}/TestDataFiles" + data.Folder + data.FileName + ".csv"))
        {
            stream.WriteLine(data.ToCsv());
        }
    }

    private void SetupDataFile(string path, string fileName, string columns)
    {
        Directory.CreateDirectory(path);
        FileStream fileStream = File.Create(path + fileName + ".csv");
        fileStream.Write(new UTF8Encoding().GetBytes(columns + Environment.NewLine));
        fileStream.Close();
    }
}
