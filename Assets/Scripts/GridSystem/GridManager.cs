using UnityEngine;

public class GridManager
{
    public static bool CanFitItem(int itemWidth, int itemHeight, GridData gridData)
    {
        for (int x = 0; x <= gridData.gridWidth - itemWidth; x++)
        {
            for (int z = 0; z <= gridData.gridHeight - itemHeight; z++)
            {
                if (IsAreaAvailable(x, z, itemWidth, itemHeight, gridData))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public static Vector3 GetAreaCenter(int startX, int startZ, int areaWidth, int areaHeight, GridData gridData)
    {
        Vector3 totalPosition = Vector3.zero;
        int totalSlots = areaWidth * areaHeight;

        for (int x = startX; x < startX + areaWidth; x++)
        {
            for (int z = startZ; z < startZ + areaHeight; z++)
            {
                totalPosition += gridData.grid[x, z].transform.localPosition;
            }
        }
        return totalPosition / totalSlots;
    }
    
    public static bool TryFindAndMarkSpace(int itemWidth, int itemHeight, GridData gridData, out int startX, out int startZ)
    {
        for (int x = 0; x <= gridData.gridWidth - itemWidth; x++)
        {
            for (int z = 0; z <= gridData.gridHeight - itemHeight; z++)
            {
                if (IsAreaAvailable(x, z, itemWidth, itemHeight, gridData))
                {
                    MarkAreaOccupied(x, z, itemWidth, itemHeight, gridData);
                    startX = x;
                    startZ = z;
                    return true;
                }
            }
        }
        startX = -1;
        startZ = -1;
        return false;
    }
    
    public static bool IsGridFullyOccupied(GridData gridData, int itemWidth, int itemHeight)
    {
        for (int x = 0; x <= gridData.gridWidth - itemWidth; x++)
        {
            for (int z = 0; z <= gridData.gridHeight - itemHeight; z++)
            {
                if (IsAreaAvailable(x, z, itemWidth, itemHeight, gridData))
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    static bool IsAreaAvailable(int startX, int startZ, int areaWidth, int areaHeight, GridData gridData)
    {
        for (int x = startX; x < startX + areaWidth; x++)
        {
            for (int z = startZ; z < startZ + areaHeight; z++)
            {
                var socket = gridData.grid[x, z].GetComponent<GridCell>();
                if (gridData.grid != null && socket != null && socket.isSlotFull)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    public static void MarkAreaOccupied(int startX, int startZ, int areaWidth, int areaHeight, GridData gridData)
    {
        for (int x = startX; x < startX + areaWidth; x++)
        {
            for (int z = startZ; z < startZ + areaHeight; z++)
            {
                gridData.grid[x, z].GetComponent<GridCell>().isSlotFull = true;
            }
        }
    }
    
    public static void ClearArea(GridCell itemData, int areaWidth, int areaHeight, GridData gridData)
    {
        for (int x = itemData.startX; x < itemData.startX + areaWidth; x++)
        {
            for (int z = itemData.startZ; z < itemData.startZ + areaHeight; z++)
            {
                gridData.grid[x, z].GetComponent<GridCell>().isSlotFull = false;
            }
        }
        itemData.startX = -1;
        itemData.startZ = -1;
    }
    
    public static void ClearArea(int startX, int startZ, int areaWidth, int areaHeight, GridData gridData)
    {
        for (int x = startX; x < startX + areaWidth; x++)
        {
            for (int z = startZ; z < startZ + areaHeight; z++)
            {
                gridData.grid[x, z].GetComponent<GridCell>().isSlotFull = false;
            }
        }
    }
    
    public static void ResetGrid(GridData gridData)
    {
        for (int x = 0; x < gridData.gridWidth; x++)
        {
            for (int z = 0; z < gridData.gridHeight; z++)
            {
                if (gridData.grid[x, z].TryGetComponent<GridCell>(out var socket))
                {
                    socket.isSlotFull = false;
                }
            }
        }
    }
    
    public static int GetAvailableAreaCount(int itemWidth, int itemHeight, GridData gridData)
    {
        int totalSlots = gridData.gridWidth * gridData.gridHeight;
        int itemArea = itemWidth * itemHeight;
        return totalSlots / itemArea;
    }
    
    public static void InitializeGrid(GridData gridData)
    {
        if (gridData.socketGrid != null)
        {
            foreach (Transform child in gridData.socketGrid.transform)
            {
                gridData.gridSockets.Add(child.gameObject);
            }
            BuildGrid(gridData);
        }
    }
    
    static void BuildGrid(GridData gridData)
    {
        gridData.grid = new GameObject[gridData.gridWidth, gridData.gridHeight];

        int index = 0;
        for (int x = 0; x < gridData.gridWidth; x++)
        {
            for (int z = 0; z < gridData.gridHeight; z++)
            {
                if (index < gridData.gridSockets.Count)
                {
                    gridData.grid[x, z] = gridData.gridSockets[index];
                    index++;
                }
                else
                {
                    Debug.LogWarning($"Not enough objects to fill the grid: {gridData.socketMain.name}");
                    return;
                }
            }
        }
    }
}