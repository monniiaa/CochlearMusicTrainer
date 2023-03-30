using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

public struct CSVFile
{
    public string Path { get; private set; }
    public string[] Columns { get; private set; }

    public CSVFile(string path, string[] columns)
    {
        Path = path;
        Columns = columns;

        // No file exists and the first row with column names has to be made
        if (!File.Exists(path))
        {
            FileStream stream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(ColumnArrayToString);
            stream.Close();
            return;
        }
        
        // The file exists but no column names on the first line
        if(IsColumnsSet) File.WriteAllLines(Path,new[] { ColumnArrayToString });
    }

    private bool IsColumnsSet => File.ReadLines(Path).FirstOrDefault() == null;
    
    private string ColumnArrayToString => string.Join(",", Columns);
}
