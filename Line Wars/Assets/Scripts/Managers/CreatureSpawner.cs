using Managers;
using Server.Abstractions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public interface ICreatureSpawner
{
    Creature SpawnCreature(Creature networkObject, Vector2 position, IGamePlayer owner = null, bool grantNetworkOwnership = false);
    // public void SpawnCreature(Creature networkObject, Vector2 position, ulong ownerId);
}

public class CreatureSpawner : NetworkBehaviour, ICreatureSpawner
{
    [SerializeField] private NetworkObject heroPrefab;
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
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (!IsServer)
            return;

        // server stuff
    }
    

    public Creature SpawnCreature(Creature creature, Vector2 position, IGamePlayer owner, bool grantNetworkOwnership = false)
    {
        var networkObject = creature.GetComponent<NetworkObject>();
        
        var newObject = Instantiate(networkObject,
            position: position,
            rotation: Quaternion.identity);
        
        if (grantNetworkOwnership)
        {
            newObject.SpawnWithOwnership(owner.ClientId);
        }
        else
        {
            newObject.Spawn();
        }
        
        
        foreach (var component in newObject.GetComponents<Entity>())
        {
            component.PlayerOwnerId.Value = owner.ClientId;
        }
        
        return newObject.GetComponent<Creature>();
    }
}