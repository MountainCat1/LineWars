using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class test : MonoBehaviour
    {
        [Inject] private IInputMapper inputMapper;
        [Inject] private ICreatureSpawner creature;

        private void Start()
        {
            Debug.Log(inputMapper);
            Debug.Log(creature);
        }
    }
}