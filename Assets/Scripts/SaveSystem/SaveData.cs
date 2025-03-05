using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public DataTime dataTime;
    public Dictionary<CropSO, int> harvestedCrops = new();
    public List<CropSaveData> cropFields = new();
    public List<BuildSaveData> buildSaveDatas = new();
}


