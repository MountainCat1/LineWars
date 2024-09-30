using Unity.Netcode;

public interface IGamePlayer
{
    public decimal Gold { get; }
}

public class GamePlayer : NetworkBehaviour, IGamePlayer
{
    public decimal Gold => _gold.Value;
    
    private NetworkVariable<decimal> _gold = new NetworkVariable<decimal>(0);
}