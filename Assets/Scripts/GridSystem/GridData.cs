using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridData
{
    [Header("Objects")]
    [Space(2)]
    public GameObject GridObj;

    [Header("Values")]
    [Space(2)]
    public int gridWidth;
    public int gridHeight;
    public int gridDepth;
    public GameObject[,] grid;

    [Header("Lists")]
    [Space(2)]
    public List<GameObject> grids = new();

    public GridData(Transform transform, GameObject gridObj, int gridWidth, int gridHeight, int gridDepth)
    {
        GridManager.CreateGrid(gridWidth, gridHeight, gridDepth, gridObj, transform, grids);
    }
}