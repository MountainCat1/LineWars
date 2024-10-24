using System;
using Extensions;
using Managers;
using Unity.Netcode;
using Zenject;

namespace Server.Abstractions
{
    public class Entity : NetworkBehaviour
    {
        [Inject] private IPlayerManager _playerManager;
        
        public event Action OnEntitySpawned;
        
        public NetworkVariable<ulong> PlayerOwnerId { get; private set; } = new(long.MaxValue);
        public IGamePlayer PlayerOwner => PlayerOwnerId.Value != long.MaxValue 
            ? _playerManager.GetPlayer(PlayerOwnerId.Value).GamePlayer
            : throw new NullReferenceException("PlayerOwnerId is null");
        
        public bool ControlledByLocalPlayer => PlayerOwner == _playerManager.LocalPlayer.GamePlayer;

        private void Awake()
        {
            this.Inject();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            OnEntitySpawned?.Invoke();
        }
        
    }
}