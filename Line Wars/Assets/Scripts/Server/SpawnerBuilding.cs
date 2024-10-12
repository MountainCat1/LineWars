using System;
using System.Collections;
using Abstractions;
using UnityEngine;
using Zenject;

namespace Server
{
    public class SpawnerBuilding : Building
    {
        [field: SerializeField] public Creature CreaturePrefab { get; set; }
        [field: SerializeField] public float SpawnDelay { get; set; }

        [SerializeField] private Transform spawnPoint;

        private DateTime _lastSpawnTime;

        [Inject] ICreatureSpawner _creatureSpawner;

        protected override void OnShortTick()
        {
            base.OnShortTick();

            if ((DateTime.UtcNow - _lastSpawnTime).TotalSeconds < SpawnDelay)
            {
                return;
            }

            _lastSpawnTime = DateTime.UtcNow;

            SpawnCreature();
        }

        protected virtual void SpawnCreature()
        {
            var position = spawnPoint.position;
            _creatureSpawner.SpawnCreature(CreaturePrefab, position, PlayerOwner);
        }
    }
}