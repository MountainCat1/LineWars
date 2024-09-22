using System;
using Client;
using Extensions;
using Server;
using Server.Consts;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Player : NetworkBehaviour
{
    public static event Action<Player> PlayerStarted;
    public static event Action<Player> PlayerSpawned;
    public static event Action<Player> PlayerDespawned;

    [field: SerializeField] public BuildingController BuildingController { get; private set; }
    [field: SerializeField] public Building TestBuilding { get; private set; }

    [Inject] private IInputManager _inputManager;
    [Inject] private IInputMapper _inputMapper;

    [SerializeField] private NetworkVariable<Color> _color = new(Color.white);

    private void Start()
    {
        this.Inject();


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

        PlayerSpawned?.Invoke(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        Debug.Log($"Player despawned | {OwnerClientId} | {IsServer} | {IsClient} | {IsOwner}");

        PlayerDespawned?.Invoke(this);
    }

    private void OnPointer1Pressed(Vector2 obj)
    {
        BuildingController.Build(TestBuilding, _inputMapper.PointerWorldPosition);
    }
}