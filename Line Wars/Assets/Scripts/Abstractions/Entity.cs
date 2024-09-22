using Unity.Netcode;

namespace Server.Abstractions
{
    public class Entity : NetworkBehaviour
    {
        public NetworkVariable<ulong> PlayerOwner { get; private set; } = new();
    }
}