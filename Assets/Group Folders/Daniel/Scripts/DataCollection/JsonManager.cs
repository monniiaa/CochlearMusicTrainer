using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    private static string _path;
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
#if UNITY_EDITOR
        _path = Application.dataPath + "\\TestDataFiles\\";
#elif PLATFORM_ANDROID
        _path = Application.persistentDataPath;
#endif
    }

    public static void WriteDataToFile<T>(AbstractDataContainer dataContainer) where T : AbstractDataContainer
    {
        string fullPath = _path + dataContainer.Path;
        if(!Directory.Exists(fullPath)) Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());

        DataContainerList<T> dataContainerList = new DataContainerList<T>();;
        if (File.Exists(fullPath))
        {
            dataContainerList = JsonUtility.FromJson<DataContainerList<T>>(File.ReadAllText(fullPath)) ?? new DataContainerList<T>();
        }

        dataContainerList.DataList.Add((T) dataContainer);

        using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            stream.Write(new UTF8Encoding(true).GetBytes(dataContainerList.ToJson()));
        }
    }

    public static List<T> ReadDataFromFile<T>() where T : AbstractDataContainer
    {
        string fullPath = _path + Activator.CreateInstance<T>().Path;
        if (!File.Exists(fullPath)) return null;
        return JsonUtility.FromJson<DataContainerList<T>>(File.ReadAllText(fullPath)).DataList;
    }
}
