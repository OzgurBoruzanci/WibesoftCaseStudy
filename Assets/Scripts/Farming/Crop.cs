using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropSO cropData;
    public bool isPlanted;
    public int currentStage = 0;
    private GameObject currentCropModel;
    private bool isGrown = false;


    public IEnumerator GrowCrop()
    {
        isPlanted = true;
        while (currentStage < cropData.growthStages.Length)
        {
            if (currentCropModel != null)
            {
                Destroy(currentCropModel);
            }
            currentCropModel = Instantiate(cropData.growthStages[currentStage], transform.position, Quaternion.identity, transform);
            currentStage++;

            if (currentStage == cropData.growthStages.Length)
            {
                isGrown = true;
                break;
            }
            yield return new WaitForSeconds(cropData.growthTime);
        }
    }

    public bool IsReadyToHarvest()
    {
        return isGrown;
    }

    public void Harvest()
    {
        if (isGrown)
        {
            Destroy(currentCropModel);
            currentStage = 0;
            isGrown = false;
            isPlanted = false;
            currentCropModel = null;
        }
    }
    public IEnumerator ContinueGrowing(float remainingTime)
    {
        isPlanted = true;

        if (currentCropModel != null)
        {
            Destroy(currentCropModel);
        }
        currentCropModel = Instantiate(cropData.growthStages[currentStage], transform.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(remainingTime);

        while (currentStage < cropData.growthStages.Length)
        {
            Destroy(currentCropModel);
            currentCropModel = Instantiate(cropData.growthStages[currentStage], transform.position, Quaternion.identity, transform);
            currentStage++;

            if (currentStage == cropData.growthStages.Length)
            {
                isGrown = true;
                break;
            }
            yield return new WaitForSeconds(cropData.growthTime);
        }
    }

}

