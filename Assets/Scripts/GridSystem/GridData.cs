using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridData
{
    [Header("Objects")]
    [Space(2)]
    public GameObject socketMain;
    public GameObject socketGrid;

    [Header("Values")]
    [Space(2)]
    public int gridWidth;
    public int gridHeight;
    public GameObject[,] grid;
    public bool isSocketTwoStoreys;

    [Header("Lists")]
    [Space(2)]
    public List<GameObject> gridSockets;
    public List<GameObject> listItemObjects;
}