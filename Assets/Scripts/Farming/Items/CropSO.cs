using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropSO", menuName = "ScriptableObjects/CropSO", order = 1)]
public class CropSO : ScriptableObject
{
    public GameObject[] growthStages;
    public int currentStage = 0;
    public int harvestAmount = 3;
    public float growthTime = 60f;

    public bool IsFullyGrown()
    {
        return currentStage == growthStages.Length - 1;
    }
}
