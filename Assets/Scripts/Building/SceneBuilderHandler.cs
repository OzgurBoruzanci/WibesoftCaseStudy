using UnityEngine;

public class SceneBuilderHandler : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefabs;
    public GridData gridData;
    void Start()
    {
        gridData.GridObj = groundPrefabs;
        gridData = new GridData(transform, groundPrefabs, 150, 0, 150);
    }
}
