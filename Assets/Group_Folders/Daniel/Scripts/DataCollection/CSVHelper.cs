using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVHelper
{
    public static void InsertColumnNames(string path, params string[] columnNames)
    {
        string tempfile = Path.GetTempFileName();
        using (var writer = new StreamWriter(tempfile))
        using (var reader = new StreamReader(path))
        {
            writer.WriteLine(columnNames);
            while (!reader.EndOfStream)
                writer.WriteLine(reader.ReadLine());
        }
        File.Copy(tempfile, path, true);
    }
}
