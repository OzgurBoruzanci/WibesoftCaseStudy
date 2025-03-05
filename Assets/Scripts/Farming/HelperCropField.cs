using UnityEngine;

public class HelperCropField : MonoBehaviour, IHandler
{
    public GameObject groundCell;
    public Crop crop;
    public GridCell gridCell;

    void Start()
    {
        gridCell = GetComponentInParent<GridCell>();
        crop = gameObject.GetComponent<Crop>();
    }
    
    public void HandleRayInteraction(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (TouchDetector.Instance.IsCreateField && !gridCell.isOccupied)
            {
                Vector3 fieldPosition = groundCell.transform.localPosition;
                fieldPosition.y = 0;
                groundCell.transform.localPosition = fieldPosition;

                Vector3 groundPosition = transform.localPosition;
                groundPosition.y = -1;
                transform.localPosition = groundPosition;
            }
            else if (!gridCell.isOccupied && FarmingManager.Instance.currentCropSO != null && !crop.isPlanted)
            {
                crop.cropData = FarmingManager.Instance.currentCropSO;
                PlantCrop();
            }
            else if (crop.IsReadyToHarvest())
            {
                HarvestCrop();
            }
        }
    }

    void PlantCrop()
    {
        gridCell.isOccupied = true;
        StartCoroutine(crop.GrowCrop());
    }

    public void HarvestCrop()
    {
        if (crop != null && crop.IsReadyToHarvest())
        {
            FarmingManager.Instance.AddHarvestedCrops(crop.cropData, crop.cropData.harvestAmount);
            crop.Harvest();
            ResetField();
        }
    }
    public CropSaveData GetCropSaveData()
    {
        if (crop == null || !crop.isPlanted) return null;

        return new CropSaveData
        {
            cropSOName = crop.cropData.name,
            currentStage = crop.currentStage,
            position = transform.position
        };
    }

    public void RestoreCrop(CropSaveData cropData)
    {
        CropSO cropSO = FarmingManager.Instance.crops.Find(c => c.name == cropData.cropSOName);
        if (cropSO != null)
        {
            crop.cropData = cropSO;
            crop.currentStage = cropData.currentStage;
            StartCoroutine(crop.ContinueGrowing(crop.cropData.growthTime));
        }
    }

    void ResetField()
    {
        gridCell.isOccupied = false;
        Vector3 fieldPosition = transform.localPosition;
        fieldPosition.y = 0;
        transform.localPosition = fieldPosition;

        Vector3 groundPosition = groundCell.transform.localPosition;
        groundPosition.y = -1;
        groundCell.transform.localPosition = groundPosition;
    }
}
