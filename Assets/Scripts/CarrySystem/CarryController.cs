using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CarryController : MonoBehaviour
{
    public int sizeX = 2;
    public int sizeZ = 3;

    private Renderer objectRenderer;
    private Color originalColor;
    private List<HelperGround> currentCells = new List<HelperGround>();
    private Vector3 originalPosition;
    private bool startCarrying;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    public void StartCarrying()
    {
        objectRenderer.material.color = new Color(1, 1, 1, 0.5f);
        if (!startCarrying)
        {
            originalPosition = transform.position;
            startCarrying = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HelperGround>(out HelperGround cell))
        {
            if (!currentCells.Contains(cell))
            {
                currentCells.Add(cell);
                cell.gridCell.isOccupied = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HelperGround>(out HelperGround cell))
        {
            currentCells.Remove(cell);
            cell.gridCell.isOccupied = false;
        }
    }

    public void MoveWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<HelperGround>(out HelperGround cell))
            {
                Vector2Int startPos = cell.gridCell.gridPosition;
                if (GridManager.CanPlaceBuilding(cell.sceneBuilderHandler.gridData, startPos, sizeX, sizeZ))
                {
                    Vector3 newPosition = new Vector3(
                        startPos.x + (sizeX / 2f) - 0.5f,
                        transform.position.y,
                        startPos.y + (sizeZ / 2f) - 0.5f
                    );

                    transform.position = newPosition;
                }
            }
        }
    }

    public void MoveWithTouch()
    {
        Vector3 touchPosition = Vector3.zero;

        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            touchPosition = Input.mousePosition;
        }

        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<HelperGround>(out HelperGround cell))
            {
                Vector2Int startPos = cell.gridCell.gridPosition;
                if (GridManager.CanPlaceBuilding(cell.sceneBuilderHandler.gridData, startPos, sizeX, sizeZ))
                {
                    Vector3 correctPosition = GridManager.SetCellsOccupied(cell.sceneBuilderHandler.gridData, startPos, sizeX, sizeZ, false);
                    correctPosition.y = transform.position.y;
                    transform.position = correctPosition;
                }
            }
        }
    }
}
