using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public List<CropSO> crops = new();
    public CropSO currentCropSO;
    public static FarmingManager Instance;

    void Awake()
    {
        Instance = this;
    }
    public void AddHarvestedCrops(CropSO crop, int amount)
    {
        if (SaveManager.Instance.harvestedCrops.ContainsKey(crop))
        {
            SaveManager.Instance.harvestedCrops[crop] += amount;
        }
        else
        {
            SaveManager.Instance.harvestedCrops[crop] = amount;
        }
        Debug.Log($"{crop.name} hasat edildi. Toplam: {SaveManager.Instance.harvestedCrops[crop]} adet.");
    }
}
