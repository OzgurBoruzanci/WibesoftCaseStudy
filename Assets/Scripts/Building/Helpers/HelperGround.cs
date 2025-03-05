using UnityEngine;

public class HelperGround : MonoBehaviour, IHandler
{
    public SceneBuilderHandler sceneBuilderHandler;
    public GameObject cropFieldCell;
    public GridCell gridCell;

    void Start()
    {
        gridCell = GetComponentInParent<GridCell>();
        sceneBuilderHandler = GetComponentInParent<SceneBuilderHandler>();
    }
    public void ConvertToField(CropSaveData savedCrop)
    {
        gameObject.AddComponent<HelperCropField>();
        HelperCropField cropField = GetComponent<HelperCropField>();
        cropField.groundCell = this.gameObject;
        cropField.gridCell = this.gridCell;

        Vector3 fieldPosition = transform.localPosition;
        fieldPosition.y = 0;
        transform.localPosition = fieldPosition;
        cropField.RestoreCrop(savedCrop);
    }

    public void HandleRayInteraction(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (TouchDetector.Instance.IsCreateField && !gridCell.isOccupied)
            {
                Vector3 fieldPosition = cropFieldCell.transform.localPosition;
                fieldPosition.y = 0;
                cropFieldCell.transform.localPosition = fieldPosition;

                Vector3 groundPosition = transform.localPosition;
                groundPosition.y = -1;
                transform.localPosition = groundPosition;

                CropSaveData cropSaveData = new CropSaveData
                {
                    gridPosition = gridCell.gridPosition,
                    currentStage = 0,
                    position = transform.localPosition,
                    isOccupied = gridCell.isOccupied,
                    gridID = gridCell.gridID
                };
                if (!SaveManager.Instance.cropFields.Exists(c => c.gridPosition == cropSaveData.gridPosition))
                {
                    SaveManager.Instance.cropFields.Add(cropSaveData);
                }

            }
        }
    }
}