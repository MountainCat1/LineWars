using Server.Exceptions;
using Unity.Netcode;

namespace Extensions
{
    public static class NetworkBehaviourExtensions
    {
        public static void AssertIsAnOwner(this NetworkBehaviour networkBehaviour)
        {
            if (!networkBehaviour.IsOwner)
            {
                throw new MustBeAnOwnerException();
            }
        }
    }
}