using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveAndLoadFileHandler
{
    public static void SaveToFile(string path, string json)
    {
        File.WriteAllText(path, json);
    }

    public static string LoadFromFile(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return json;
        }
        else
        {
            return null;
        }
    }

    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

}
