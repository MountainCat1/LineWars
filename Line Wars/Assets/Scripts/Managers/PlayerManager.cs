using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Managers
{
    public interface IPlayerManager
    {
        public IReadOnlyList<Player> Players { get; }
        Player LocalPlayer { get; }
        event Action<Player> PlayerStarted;
        event Action<Player> PlayerSpawnded;
        void RegisterPlayer(Player player);
        Player GetPlayer(ulong playerId);
    }

    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        public event Action<Player> PlayerStarted;
        public event Action<Player> PlayerSpawnded;

        public IReadOnlyList<Player> Players => _players;
        public Player LocalPlayer => GetLocalPlayer();

        [Inject] private IHostManager _hostManager;

        private readonly List<Player> _players = new();

        // Unity Methods

        private void Awake()
        {
            _hostManager.HostStopped += OnHostStopped;
        }

        // Methods

        private Player GetLocalPlayer()
        {
            return _players.Find(player => player.IsLocalPlayer);
        }
        
        public void RegisterPlayer(Player player)
        {
            OnPlayerSpawned(player);
        }

        public Player GetPlayer(ulong playerId)
        {
            return _players.Find(player => player.OwnerClientId == playerId);
        }


        // Handlers
        private void OnHostStopped()
        {
            _players.Clear();
        }

        private void OnPlayerSpawned(Player player)
        {
            Debug.Log("Adding a player");
            _players.Add(player);
            Debug.Log(_players.Count);

            player.PlayerStarted += OnPlayerStarted;

            PlayerSpawnded?.Invoke(player);
        }

        private void OnPlayerStarted(Player player)
        {
            PlayerStarted?.Invoke(player);
        }
    }
}