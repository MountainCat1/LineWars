using System;
using Client;
using Extensions;
using Managers;
using Server;
using Server.Consts;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Player : NetworkBehaviour
{
    public event Action<Player> PlayerStarted;

    public IGamePlayer GamePlayer => gamePlayer;
    public IBuildingController BuildingController => buildingController;
    
    [field: SerializeField] private GamePlayer gamePlayer;
    [field: SerializeField] public BuildingController buildingController;
    [field: SerializeField] public Building TestBuilding { get; private set; }

    [Inject] private IInputManager _inputManager;
    [Inject] private IInputMapper _inputMapper;
    [Inject] private IPlayerManager _playerManager;

    [SerializeField] private NetworkVariable<Color> _color = new(Color.white);

    private void Start()
    {
        this.Inject();

        _playerManager.RegisterPlayer(this);

        if (IsServer)
        {
            _color.Value = PlayerColors.Colors[(int)OwnerClientId % PlayerColors.Colors.Length];
        }

        if (IsOwner)
        {
            _inputManager.Pointer1Pressed += OnPointer1Pressed;
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

    private void OnPointer1Pressed(Vector2 obj)
    {
        BuildingController.Build(TestBuilding, _inputMapper.PointerWorldPosition);
    }
}