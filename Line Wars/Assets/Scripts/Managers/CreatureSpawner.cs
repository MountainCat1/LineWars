using Unity.Netcode;
using UnityEngine;

public interface ICreatureSpawner
{
}

public class CreatureSpawner : NetworkBehaviour, ICreatureSpawner
{
    [SerializeField] private NetworkObject heroPrefab;
    
    private void Start()
    {
        if (!IsServer)
            return;
        
        Player.OnPlayerSpawned += OnPlayerSpawned;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        Player.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(Player player)
    {
        SpawnCreature(heroPrefab, player);
    }

    public void SpawnCreature(NetworkObject networkObject, Player owner = null)
    {
        // Spawn creature
        var spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        
        NetworkManager.SpawnManager.InstantiateAndSpawn(networkObject, owner!.OwnerClientId, 
            position: spawnPosition,
            rotation: Quaternion.identity);
    }
}