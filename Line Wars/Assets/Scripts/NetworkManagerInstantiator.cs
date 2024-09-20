using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class NetworkManagerInstantiator : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManagerPrefab;

        public NetworkManager Instantiate()
        {
            var instance = Instantiate(networkManagerPrefab);

            return instance;
        }
    }
}