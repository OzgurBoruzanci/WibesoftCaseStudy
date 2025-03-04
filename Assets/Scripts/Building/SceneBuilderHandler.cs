using UnityEngine;

public class SceneBuilderHandler : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefabs;
    void Start()
    {
        BuildScene(groundPrefabs, 150, 0, 150);
    }
    public void BuildScene(GameObject buildObj, int x, int y, int z)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Instantiate(buildObj, new Vector3(i, y, j), Quaternion.identity);
            }
        }
    }
}
