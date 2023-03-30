using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    private static JsonManager _instance;   
    public static JsonManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<JsonManager>();
            }

            return _instance;
        }
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        WriteDataToFile<SoundLocalizationDataContainer>(new SoundLocalizationDataContainer(10, 1));
        WriteDataToFile<SoundLocalizationDataContainer>(new SoundLocalizationDataContainer(12, 1));
        WriteDataToFile<SoundLocalizationDataContainer>(new SoundLocalizationDataContainer(10, 1));
        WriteDataToFile<SoundLocalizationDataContainer>(new SoundLocalizationDataContainer(10, 90));
    }

    public static void WriteDataToFile<T>(AbstractDataContainer dataContainer) where T : AbstractDataContainer
    {
        string fullPath = Application.dataPath + "\\TestDataFiles\\" + dataContainer.Path;
        if(!Directory.Exists(fullPath)) Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());

        DataContainerList<T> dataContainerList = new DataContainerList<T>();;
        if (File.Exists(fullPath))
        {
            dataContainerList = JsonUtility.FromJson<DataContainerList<T>>(File.ReadAllText(fullPath));
        }

        dataContainerList.DataList.Add((T) dataContainer);

        using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            stream.Write(new UTF8Encoding(true).GetBytes(dataContainerList.ToJson()));
        }
    }
}
