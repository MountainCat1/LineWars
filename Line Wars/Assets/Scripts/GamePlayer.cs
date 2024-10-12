using Unity.Netcode;

public interface IGamePlayer
{
    public decimal Gold { get; }
    ulong Id { get; }
}

public class GamePlayer : NetworkBehaviour, IGamePlayer
{
    public decimal Gold => _gold.Value;
    public new ulong Id => base.OwnerClientId;

    private NetworkVariable<decimal> _gold = new NetworkVariable<decimal>(0);
}