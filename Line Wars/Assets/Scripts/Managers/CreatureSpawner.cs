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

    private void Start()
    {
        if (!IsServer)
            return;

        _playerManager.PlayerStarted += OnPlayerSpawnedServer;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (!IsServer)
            return;

        _playerManager.PlayerStarted -= OnPlayerSpawnedServer;
    }

    private void OnPlayerSpawnedServer(Player player)
    {
        SpawnCreature(heroPrefab.GetComponent<Creature>(), Vector2.zero, player.GamePlayer);
    }

    public void SpawnCreature(Creature creature, Vector2 position, IGamePlayer owner = null)
    {
        var networkObject = creature.GetComponent<NetworkObject>();
        
        var newObject = NetworkManager.SpawnManager.InstantiateAndSpawn(networkObject, owner!.Id,
            position: position,
            rotation: Quaternion.identity);
        
        foreach (var component in newObject.GetComponents<Entity>())
        {
            component.PlayerOwnerId.Value = owner.Id;
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