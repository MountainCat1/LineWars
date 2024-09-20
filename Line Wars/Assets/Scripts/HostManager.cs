#if UNITY_EDITOR
using ParrelSync;
#endif
using Unity.Netcode;
using UnityEngine;
using Zenject;

public interface IHostManager
{
}

public class HostManager : MonoBehaviour, IHostManager
{
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