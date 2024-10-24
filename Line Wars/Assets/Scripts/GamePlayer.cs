using Unity.Netcode;
using UnityEngine;

public interface IGamePlayer
{
    public decimal Gold { get; }
    ulong ClientId { get; }
}

public class GamePlayer : NetworkBehaviour, IGamePlayer
{
    public decimal Gold => _gold.Value;
    public ulong ClientId => base.OwnerClientId;
    [field: SerializeField] public ulong GameId { get; set; }

    private NetworkVariable<decimal> _gold = new NetworkVariable<decimal>(0);
}