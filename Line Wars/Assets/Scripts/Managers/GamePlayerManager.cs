using System.Collections.Generic;
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

    public class GamePlayerManager : MonoBehaviour, IGamePlayerManager
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
            _playerManager.PlayerStarted += OnPlayerStarted;
        }

        private void OnPlayerStarted(Player player)
        {
            if (_assignedIndex >= _gamePlayers.Count)
            {
                Debug.LogError("Not enough game players");
                return;
            }

            var gamePlayer = _gamePlayers[_assignedIndex++];
            player.GamePlayer = gamePlayer;
        }
    }
}