using System;
using Extensions;
using Managers;
using Server.Abstractions;
using Unity.Netcode;
using Zenject;

namespace Abstractions
{
    public class Building : Entity
    {
        [Inject] private ITickManager _tickManager;

        private void Start()
        {
            if (!IsServer)
            {
                return;
            }

            this.Inject();
            
            _tickManager.OnShortTickEvent += OnShortTick;
            _tickManager.OnLongTickEvent += OnLongTick;
            
            OnStartServer();
        }

        protected virtual void OnShortTick()
        {
        }
        
        protected virtual void OnLongTick()
        {
        }
        
        protected virtual void OnStartServer()
        {
        }
    }
}