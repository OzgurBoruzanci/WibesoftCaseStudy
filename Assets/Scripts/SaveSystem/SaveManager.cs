using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string saveFilePath => Path.Combine(Application.persistentDataPath, "savegame.json");
    public DataTime saveDataList = new();
    public Dictionary<CropSO, int> harvestedCrops = new();
    public List<CropSaveData> cropFields = new();
    public List<BuildSaveData> buildData = new();
    public Action LoadGameEvent;
    public SaveData currentSaveData;
    public static SaveManager Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Invoke("LoadGame",1f);
    }
    public void SaveGame(SaveData saveData)
    {
        try
        {
            saveData.dataTime = saveDataList;
            saveData.harvestedCrops = harvestedCrops;
            saveData.cropFields = cropFields;
            saveData.buildSaveDatas = buildData;

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
            harvestedCrops = saveData.harvestedCrops;
            cropFields = saveData.cropFields;
            buildData = saveData.buildSaveDatas;
            LoadGameEvent?.Invoke();
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

    // Yükleme tamamlandıktan sonra tarlaları geri getir
    RestoreFields();
}

void RestoreFields()
{
    foreach (CropSaveData cropData in cropFields)
    {
        GameObject groundObject = FindGroundByPosition(cropData.gridPosition);
        if (groundObject != null)
        {
            HelperGround helperGround = groundObject.GetComponent<HelperGround>();
            if (helperGround != null)
            {
                helperGround.ConvertToField(cropData);
            }
        }
    }
}

GameObject FindGroundByPosition(Vector2Int gridPosition)
{
    HelperGround[] allGrounds = FindObjectsOfType<HelperGround>();
    foreach (var ground in allGrounds)
    {
        if (ground.gridCell.gridPosition == gridPosition)
        {
            return ground.gameObject;
        }
    }
    return null;
}

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save File Deleted!");
        }
    }
    void  OnApplicationQuit()
    {
        SaveGame(currentSaveData);
    }
}
