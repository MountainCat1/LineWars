using System;
using System.Collections;
using Managers;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private IStageManager _setupManager;

    private void Start()
    {
        StartCoroutine(SetupCoroutine());
    }
    
    private IEnumerator SetupCoroutine()
    {
        yield return new WaitForEndOfFrame();
        var stageManager = _setupManager as StageManager;
        stageManager!.StartGame(); // we do it so that no other manager can start the game
    }
}
