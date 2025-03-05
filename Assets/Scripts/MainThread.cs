using System;
using System.Threading;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    public static SynchronizationContext UnitySynchronizationContext { get; private set; }
    private static MainThread _instance;

    public static MainThread Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("MainThread is not initialized. Make sure it is attached to a GameObject in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        _instance = this;
        UnitySynchronizationContext = SynchronizationContext.Current;

        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    public static void Run(Action action)
    {
        if (UnitySynchronizationContext == null)
        {
            Debug.LogError("SynchronizationContext is not initialized!");
            return;
        }

        UnitySynchronizationContext.Post(_ => action(), null);
    }
}
