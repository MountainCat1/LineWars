using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Managers
{
    public interface IPlayerManager
    {
        public IReadOnlyList<Player> Players { get; }
    }

    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        public IReadOnlyList<Player> Players => _players;

        [Inject] private IHostManager _hostManager;

        private readonly List<Player> _players = new();

        void Start()
        {
            _hostManager.HostStarted += OnHostStarted;

            Player.PlayerSpawned += OnPlayerSpawned;
            Player.PlayerDespawned += OnPlayerSpawned;
        }

        private void OnHostStarted()
        {
            _players.Clear();
        }

        private void OnPlayerSpawned(Player player)
        {
            _players.Add(player);
        }
    }
}