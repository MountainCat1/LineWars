using System;
using Abstractions;
using Client;
using Extensions;
using Managers;
using Server.Consts;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Player : NetworkBehaviour
{
    public event Action<Player> PlayerStarted;

    public IGamePlayer GamePlayer { get; set; }
    public IBuildingController BuildingController => buildingController;
    
    [field: SerializeField] private BuildingController buildingController;
    [field: SerializeField] public Building TestBuilding { get; private set; } // TODO: Remove this
    
    [Inject] private IPlayerManager _playerManager;

    [SerializeField] private NetworkVariable<Color> color = new(Color.white);

    private void Start()
    {
        this.Inject();

        _playerManager.RegisterPlayer(this);

        if (IsServer)
        {
            color.Value = PlayerColors.Colors[(int)OwnerClientId % PlayerColors.Colors.Length];
        }
        
        
        PlayerStarted?.Invoke(this);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log($"Player spawned | {OwnerClientId} | {IsServer} | {IsClient} | {IsOwner}");
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        Debug.Log($"Player despawned | {OwnerClientId} | {IsServer} | {IsClient} | {IsOwner}");
    }
}