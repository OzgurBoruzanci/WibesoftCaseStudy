using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

public enum ConnectionType
{
    Server,
    Local
}

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    private HubConnection connection;
    private string serverUrl;

    [Tooltip(" External Server IP = x.x.x.x, Local Server IP = 127.0.0.1")]
    [SerializeField] private string ipAddress;
    [SerializeField] private ushort port;
    [SerializeField] private ConnectionType connectionType;

    private DateTime lastProductionTime;
    private int productionInterval = 300; // 5 dakika (saniye cinsinden)

    public GameObject cube;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (connectionType == ConnectionType.Local)
        {
            ipAddress = "127.0.0.1";
        }
        Debug.Log("NetworkManager created. Server IP: " + ipAddress + ", Port: " + port);
        serverUrl = $"http://{ipAddress}:{port}/gameHub";
    }

    public async Task Connect()
    {
        try
        {
            connection = new HubConnectionBuilder()
            .WithUrl(serverUrl, options =>
            {
                options.SkipNegotiation = true;
                options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                options.WebSocketConfiguration = conf =>
                {
                    conf.KeepAliveInterval = TimeSpan.FromSeconds(15);
                };
            })
            .WithAutomaticReconnect()
            .Build();

            if (connection == null)
            {
                Debug.Log("Connection is null " + connection.ConnectionId);
            }

            connection.On<string>("OnConnected", OnConnected);

            await connection.StartAsync();
            Debug.Log("Connected to SignalR server.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to connect to server: {ex.Message}");
            Debug.LogError($"Stack trace: {ex.StackTrace}");
        }
    }

    public bool IsConnected()
    {
        return connection.State == HubConnectionState.Connected;
    }

    private async void SendExitTime()
    {
        if (connection != null && IsConnected())
        {
            DateTime exitTime = DateTime.UtcNow;
            await connection.InvokeAsync("SaveExitTime", exitTime);
        }
    }

    public async void OnApplicationQuit()
    {
        SendExitTime();
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
            connection = null;
        }
    }

    private async void OnConnected(string connectionId)
    {
        Debug.Log($"Connected to server. Connection ID: {connectionId}");
        RequestLastProductionTime();
    }

    private async void RequestLastProductionTime()
    {
        if (connection != null && IsConnected())
        {
            lastProductionTime = await connection.InvokeAsync<DateTime>("GetLastProductionTime");
            CalculateMissedResources();
        }
    }

    private void CalculateMissedResources()
    {
        DateTime now = DateTime.UtcNow;
        TimeSpan elapsedTime = now - lastProductionTime;
        int missedCount = (int)(elapsedTime.TotalSeconds / productionInterval);

        for (int i = 0; i < missedCount; i++)
        {
            ProduceResource();
        }

        lastProductionTime = now;
        InvokeRepeating(nameof(ProduceResource), productionInterval, productionInterval);
    }

    private void ProduceResource()
    {
        Instantiate(cube, transform.position, Quaternion.identity);
        if (connection != null && IsConnected())
        {
            connection.InvokeAsync("UpdateProductionTime", DateTime.UtcNow);
        }
    }
}
