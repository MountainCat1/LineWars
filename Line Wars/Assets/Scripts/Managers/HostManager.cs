﻿using System;
using System.Collections;
using System.Net;
using Server.Exceptions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Utilities;
using Zenject;

public interface IHostManager
{
    private const ushort DefaultPort = 7777;

    #region Events

    event Action<NetworkClient> OnClientConnected;
    event Action<ulong> OnClientDisconnected;
    event Action<string> OnLocalClientStopped;
    event Action<string> OnComebackAfterDisconnect;
    event Action OnServerStopped;
    event Action OnGameStarted;
    event Action OnGameReset;
    event Action OnServerStarted;

    #endregion

    void HostGame();
    void StopHosting();
    void StartGameCountdown();
    void StartClient(string address, ushort port = DefaultPort);
    void StartServer();
    void StopClient();
    void RestartGame();

    bool IsHosting { get; }
    bool AcceptConnections { get; }
    string DefaultAddress { get; }
}

public class HostManager : MonoBehaviour, IHostManager
{
    #region Events

    public event Action<NetworkClient> OnClientConnected;
    public event Action<ulong> OnClientDisconnected;
    public event Action<string> OnLocalClientStopped;
    public event Action<string> OnComebackAfterDisconnect;
    public event Action OnGameStarted;
    public event Action OnGameReset;
    public event Action OnServerStarted;
    public event Action OnServerStopped;

    #endregion

    [Inject] private NetworkManager _networkManager;
    [Inject] private GameServerSettings _serverSettings;
    [SerializeField] public SceneReference gameScene;
    [SerializeField] public SceneReference menuScene;
    [SerializeField] public NetworkObject networkSystemsPrefab;

    public bool IsHosting => _networkManager.IsServer;
    public bool Started { get; private set; } = false;
    public bool AcceptConnections => !Started;

    public string DefaultAddress
    {
        get => GetDefaultAddress();
    }

    private NetworkObject _networkSystems;

    private Coroutine _startGameCoroutine;

    private void OnEnable()
    {
        _networkManager.OnServerStarted += HandleServerStarted;
        _networkManager.OnServerStopped += HandleServerStopped;
        _networkManager.OnClientStopped += HandleClientStopped;
    }


    private void OnDisable()
    {
        _networkManager.OnServerStarted -= HandleServerStarted;
        _networkManager.OnServerStopped -= HandleServerStopped;
        _networkManager.OnClientStopped -= HandleClientStopped;
    }


    private void HandleClientStopped(bool isHost)
    {
        if (SceneManager.GetActiveScene().path == menuScene.ScenePath)
        {
            OnLocalClientStopped?.Invoke(_networkManager.DisconnectReason);
            OnComebackAfterDisconnect?.Invoke(_networkManager.DisconnectReason);
            return;
        }

        OnLocalClientStopped?.Invoke(_networkManager.DisconnectReason);
        var operation = SceneManager.LoadSceneAsync(menuScene.ScenePath, LoadSceneMode.Single);

        operation.completed += _ =>
        {
            OnComebackAfterDisconnect?.Invoke(_networkManager.DisconnectReason);
        };
    }

    private void HandleServerStopped(bool obj)
    {
        OnServerStopped?.Invoke();
        
        Debug.Log("Unregistering host events...");
        _networkManager.OnClientConnectedCallback -= HandleClientConnectedAsServer;
        _networkManager.OnClientDisconnectCallback -= HandleClientDisconnectedAsServer;
        Debug.Log("Server stopped");
    }

    private void HandleServerStarted()
    {
        Debug.Log("Reregistering host events...");
        _networkManager.OnClientConnectedCallback += HandleClientConnectedAsServer;
        _networkManager.OnClientDisconnectCallback += HandleClientDisconnectedAsServer;
        Debug.Log("Server started");
    }


    private void HandleClientConnectedAsServer(ulong id)
    {
        Debug.Log($"Client connected with an id of {id}");

        if (!AcceptConnections)
        {
            Debug.Log("Client connected but connections are not being accepted. Disconnecting...");
            _networkManager.DisconnectClient(id, DisconnectReasons.ServerFull);
            return;
        }

        var networkClient = _networkManager.ConnectedClients[id];
        OnClientConnected?.Invoke(networkClient);

        if (_networkManager.ConnectedClients.Count >= _serverSettings.MaxPlayers)
        {
            StartGameCountdown();
        }
    }

    private void HandleClientDisconnectedAsServer(ulong id)
    {
        OnClientDisconnected?.Invoke(id);

        Debug.Log(
            $"Client disconnected with an id of {id}. Now there is {_networkManager.ConnectedClients.Count - 1} client(s) connected.");

        if (Started && _networkManager.ConnectedClients.Count <= 1)
        {
            Debug.Log("Stopping server...");
            _networkManager.Shutdown();
            Destroy(_networkSystems.gameObject);
            Started = false;
            OnGameReset?.Invoke();
            SceneManager.LoadScene("Scenes/MainMenuScene");
        }
    }

    public void HostGame()
    {
        Debug.Log("Starting host...");
        _networkManager.StartHost();

        SpawnNetworkSystems();
    }

    public void StartServer()
    {
        Debug.Log("Starting server...");
        _networkManager.StartServer();

        SpawnNetworkSystems();
    }

    public void StopHosting()
    {
        Debug.Log("Stopping host...");
        Started = false;
        _networkManager.Shutdown();
    }

    public void StartGameCountdown()
    {
        if (!_networkManager.IsServer)
            throw new ServerOnlyMethodException();

        if (_startGameCoroutine is not null)
        {
            Debug.Log("Countdown already started! Cancelling previous countdown...");
            StopCoroutine(_startGameCoroutine);
        }

        _startGameCoroutine = StartCoroutine(WaitToStartGame());
    }

    private void MoveToGameScene()
    {
        if (!_networkManager.IsServer)
            throw new ServerOnlyMethodException();

        if (Started)
        {
            Debug.LogError("Cannot start game twice!");
            return;
        }

        Started = true;
        Debug.Log("Starting game...");
        OnGameStarted?.Invoke();
        _networkManager.SceneManager.LoadScene(gameScene.ScenePath, LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        if (!Started)
        {
            Debug.LogError("Cannot restart game if it hasn't started!");
            return;
        }

        Debug.Log("Restarting game...");
        _networkManager.SceneManager.LoadScene(gameScene.ScenePath, LoadSceneMode.Single);
    }

    public void StartClient(string address, ushort port)
    {
        Debug.Log("Starting client...");
        var transport = _networkManager.NetworkConfig.NetworkTransport as UnityTransport;

        if (transport is null)
            throw new NullReferenceException("Transport is null! Is it not set? Or maybe it's not a UnityTransport?");

        if (!IsIPAddress(address))
            address = GetIpFromDomain(address);

        transport.ConnectionData.Address = address;
        transport.ConnectionData.Port = port;

        Debug.Log($"Connecting to {address}:{port}...");
        _networkManager.StartClient();
    }

    public void StopClient()
    {
        Debug.Log("Disconnecting...");
        _networkManager.Shutdown();
    }

    private string GetIpFromDomain(string domain)
    {
        IPHostEntry ipHostEntry = Dns.GetHostEntry(domain);

        return ipHostEntry.AddressList[0].ToString();
    }

    static bool IsIPAddress(string input)
    {
        IPAddress ipAddress;
        return IPAddress.TryParse(input, out ipAddress);
    }

    private void SpawnNetworkSystems()
    {
        _networkSystems = Instantiate(networkSystemsPrefab);
        _networkSystems.Spawn(false);
    }

    private string GetDefaultAddress()
    {
        var serverConfigAsset = Resources.Load<TextAsset>("server");
        var address = serverConfigAsset.text;
        return address;
    }

    private IEnumerator WaitToStartGame()
    {
        yield return new WaitForSeconds(_serverSettings.DelayToStartGameFromLobby);

        if (Started)
        {
            Debug.LogError("Trying to start game twice!");
            yield break;
        }

        MoveToGameScene();
    }
}

public static class DisconnectReasons
{
    public const string ServerFull = "Server is full";
}