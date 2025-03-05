using System;
using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public float generationIntervalMinutes = 5f; // 5 dakika arayla kaynak üret
    private DateTime lastGenerationTime;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.Instance;
        StartCoroutine(ResourceGenerationLoop());
    }

    private IEnumerator ResourceGenerationLoop()
    {
        while (true)
        {
            yield return new WaitUntil(() => networkManager.IsConnected());
            if (DateTime.UtcNow - lastGenerationTime >= TimeSpan.FromMinutes(generationIntervalMinutes))
            {
                GenerateResource();
                lastGenerationTime = DateTime.UtcNow;
            }
            yield return new WaitForSeconds(30f); // Her 30 saniyede bir kontrol et
        }
    }

    public void CatchUpMissedResources(DateTime serverLastGenerationTime)
    {
        int missedResources = CalculateMissedResources(serverLastGenerationTime);
        
        for (int i = 0; i < missedResources; i++)
        {
            GenerateResource();
        }

        lastGenerationTime = DateTime.UtcNow;
    }

    private int CalculateMissedResources(DateTime serverLastGenerationTime)
    {
        TimeSpan timeSinceLastGeneration = DateTime.UtcNow - serverLastGenerationTime;
        return (int)(timeSinceLastGeneration.TotalMinutes / generationIntervalMinutes);
    }

    private void GenerateResource()
    {
        if (cubePrefab != null)
        {
            Vector3 randomPosition = new Vector3(
                UnityEngine.Random.Range(-10f, 10f),
                1f,
                UnityEngine.Random.Range(-10f, 10f)
            );

            Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        }
    }
}
