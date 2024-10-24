using Managers;
using Server.Abstractions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public interface ICreatureSpawner
{
    void SpawnCreature(Creature networkObject, Vector2 position, IGamePlayer owner = null);
    // public void SpawnCreature(Creature networkObject, Vector2 position, ulong ownerId);
}

public class CreatureSpawner : NetworkBehaviour, ICreatureSpawner
{
    [SerializeField] private NetworkObject heroPrefab;
    [Inject] private IPlayerManager _playerManager;
    [Inject] private IStageManager _stageManager;
    [Inject] private IGamePlayerManager _gamePlayerManager;
    
    private void Start()
    {
        if (!IsServer)
            return;

        _stageManager.GameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        foreach (var gamePlayer in _gamePlayerManager.GamePlayers)
        {
            SpawnCreature(heroPrefab.GetComponent<Creature>(), Vector2.zero, gamePlayer);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (!IsServer)
            return;

        // server stuff
    }
    

    public void SpawnCreature(Creature creature, Vector2 position, IGamePlayer owner = null)
    {
        var networkObject = creature.GetComponent<NetworkObject>();
        
        var newObject = NetworkManager.SpawnManager.InstantiateAndSpawn(networkObject, owner!.ClientId,
            position: position,
            rotation: Quaternion.identity);
        
        foreach (var component in newObject.GetComponents<Entity>())
        {
            component.PlayerOwnerId.Value = owner.ClientId;
        }
    }
    
    public void SpawnCreature(Creature creature, Vector2 position, ulong ownerId)
    {
        var networkObject = creature.GetComponent<NetworkObject>();
        
        NetworkManager.SpawnManager.InstantiateAndSpawn(networkObject, ownerId,
            position: position,
            rotation: Quaternion.identity);
    }
}