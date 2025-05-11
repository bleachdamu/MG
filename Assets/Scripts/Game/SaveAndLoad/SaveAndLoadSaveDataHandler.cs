using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoadSaveDataHandler : MonoBehaviour
{
    public string path;

    public void SaveData(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        SaveAndLoadFileHandler.SaveToFile(Application.persistentDataPath + path, json);
    }

    public SaveData LoadData()
    {
        string json = SaveAndLoadFileHandler.LoadFromFile(Application.persistentDataPath + path);
        if (json != null)
        {
            SaveData saveData = JsonSerializer.SerializeFromJSON<SaveData>(json);
            return saveData;
        }
        else
        {
            return null;
        }
    }

    public void DeleteSaveData()
    {
        SaveAndLoadFileHandler.DeleteFile(Application.persistentDataPath + path);
    }
}
