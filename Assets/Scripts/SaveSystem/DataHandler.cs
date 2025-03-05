using UnityEngine;

public class DataTime
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int second;
}

[System.Serializable]
public class CropSaveData
{
    public string cropSOName;
    public int gridID;
    public int currentStage;
    public Vector3 position;
    public Vector2Int gridPosition;
    public bool isOccupied;
}

[System.Serializable]
public class BuildSaveData
{
    public int buildId;
    public Vector3 position;
    public Quaternion rotation;
}