using System;
using Unity.Netcode;
using UnityEngine;


public class Player : NetworkBehaviour
{
    public static event Action<Player> OnPlayerSpawned;

    private void Start()
    {
        if(!IsServer)
            return;
        
        OnPlayerSpawned?.Invoke(this);
    }

    private void Update()
    {
        Debug.Log(name + OwnerClientId);
    }
}