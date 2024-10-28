using System.Collections.Generic;
using ModestTree;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Managers
{
    public interface IGamePlayerManager
    {
        ICollection<GamePlayer> GamePlayers { get; }
    }

    public class GamePlayerManager : NetworkBehaviour, IGamePlayerManager
    {
        [Inject] private IPlayerManager _playerManager;
        [Inject] private NetworkManager _networkManager;

        public ICollection<GamePlayer> GamePlayers => _gamePlayers;

        private IList<GamePlayer> _gamePlayers = new List<GamePlayer>();

        private int _assignedIndex = 0;

        private void Start()
        {
            if (!_networkManager.IsServer)
                return;
         
            _gamePlayers = Object.FindObjectsOfType<GamePlayer>();
            
            // Assign players to game players
            foreach (var player in _playerManager.Players)
            {
                AssignGamePlayer(player);
            }
            
            // Subscribe to player started event to asssign late joiners
            _playerManager.PlayerStarted += AssignGamePlayer;
        }

        private void AssignGamePlayer(Player player)
        {
            if (_assignedIndex >= _gamePlayers.Count)
            {
                Debug.LogError("Not enough game players");
                return;
            }

            var gamePlayer = _gamePlayers[_assignedIndex++];
            player.GamePlayer = gamePlayer;
            
            gamePlayer.NetworkObject.ChangeOwnership(player.OwnerClientId);
            
            AssignGamePlayerClientRpc(player.OwnerClientId, _assignedIndex - 1);
        }
        
        [ClientRpc]
        public void AssignGamePlayerClientRpc(ulong playerId, int gamePlayerIndex)
        {
            // TODO: THIS IS NOT HOW ITS SUPPOSE DO BE?!
            if(_gamePlayers.IsEmpty())
                _gamePlayers = Object.FindObjectsOfType<GamePlayer>();
            
            var player = _playerManager.GetPlayer(playerId);
            var gamePlayer = _gamePlayers[gamePlayerIndex];
            player.GamePlayer = gamePlayer;
        }
    }
}