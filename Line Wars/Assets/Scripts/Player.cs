using System;
using Unity.Netcode;



public class Player : NetworkBehaviour
{
    public static event Action<Player> OnPlayerSpawned;

    private void Start()
    {
        if(!IsServer)
            return;
        
        OnPlayerSpawned?.Invoke(this);
    }
}