using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string saveFilePath => Path.Combine(Application.persistentDataPath, "savegame.json");
    public DataTime saveDataList = new();

    public SaveData currentSaveData;
    public static SaveManager Instance;
    void Awake()
    {
        Instance = this;
    }
    public void SaveGame(SaveData saveData)
    {
        try
        {
            saveData.dataTime = saveDataList;
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game Saved Successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save Error: {e.Message}");
        }
    }
    public void LoadGame()
    {
        SaveData saveData;
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
                saveDataList = saveData.dataTime;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Load Error: {e.Message}");
            }
        }
        else
        {
            saveData = new();
            saveData.dataTime = saveDataList;
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save File Deleted!");
        }
    }
}
