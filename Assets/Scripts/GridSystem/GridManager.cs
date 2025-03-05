using UnityEngine;
using System.Collections.Generic;

public static class GridManager
{
    public static void CreateGrid(int width, int height, int depth, GameObject buildObj, Transform parent, List<GameObject> gridCells)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                GameObject cell = Object.Instantiate(buildObj, new Vector3(x, height, z), Quaternion.identity, parent);
                cell.GetComponent<GridCell>().gridPosition = new Vector2Int(x, z);
                cell.GetComponent<GridCell>().gridID = gridCells.Count;
                gridCells.Add(cell);
            }
        }
    }

    public static List<GridCell> GetCellsInArea(GridData gridData, Vector2Int startPos, int width, int depth)
    {
        List<GridCell> cells = new List<GridCell>();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Debug.Log(startPos.x + x + "   " + startPos.x );
                Vector2Int pos = new Vector2Int(startPos.x, startPos.y + z);
                GameObject cellObj = gridData.grids.Find(g => g.GetComponent<GridCell>().gridPosition == pos);
                if (cellObj != null)
                {
                    cells.Add(cellObj.GetComponent<GridCell>());
                }
            }
        }
        return cells;
    }

    public static bool CanPlaceBuilding(GridData gridData, Vector2Int startPos, int width, int depth)
    {
        List<GridCell> cells = GetCellsInArea(gridData, startPos, width, depth);
        return cells.Count == width * depth && cells.TrueForAll(cell => !cell.isOccupied);
    }

    public static Vector3 SetCellsOccupied(GridData gridData, Vector2Int startPos, int width, int depth, bool occupied)
    {
        List<GridCell> cells = GetCellsInArea(gridData, startPos, width, depth);
        foreach (var cell in cells)
        {
            cell.isOccupied = occupied;
        }
        float centerX = startPos.x + (width / 2f) - 0.5f;
        float centerZ = startPos.y + (depth / 2f) - 0.5f;

        return new Vector3(centerX, gridData.gridHeight, centerZ);
    }

}
