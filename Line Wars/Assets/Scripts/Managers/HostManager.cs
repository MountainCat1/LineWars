#if UNITY_EDITOR
using ParrelSync;
#endif
using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public interface IHostManager
{
    event Action HostStarted;
}

public class HostManager : MonoBehaviour, IHostManager
{
    public event Action HostStarted;

    [Inject] private NetworkManager _networkManager;

    private void Start()
    {
        // ReSharper disable once ReplaceWithSingleAssignment.True
        var isHost = true;

#if UNITY_EDITOR
        if (ClonesManager.IsClone())
        {
            isHost = false;
        }
#endif

        HostStarted?.Invoke();
        if (isHost)
        {
            _networkManager.StartHost();
        }
        else
        {
            _networkManager.StartClient();
        }
    }
}