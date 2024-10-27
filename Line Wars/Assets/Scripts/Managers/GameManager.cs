using System;
using System.Collections;
using Managers;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private IStageManager _setupManager;
    [Inject] private IGamePlayerManager _gamePlayerManager;
    [Inject] private ICreatureSpawner _creatureSpawner;

    [SerializeField] private GameObject heroPrefab;

    private void Start()
    {
        StartCoroutine(SetupCoroutine());
    }
    
    private IEnumerator SetupCoroutine()
    {
        yield return new WaitForEndOfFrame();
        var stageManager = _setupManager as StageManager;
        stageManager!.StartGame(); // we do it so that no other manager can start the game
        
        foreach (var gamePlayer in _gamePlayerManager.GamePlayers)
        {
            _creatureSpawner.SpawnCreature(heroPrefab.GetComponent<Creature>(), Vector2.zero, gamePlayer, grantNetworkOwnership: true);
        }
    }
}
