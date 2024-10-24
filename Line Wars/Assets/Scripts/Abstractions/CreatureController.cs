using Extensions;
using Managers;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Abstractions
{
    [RequireComponent(typeof(Creature))]
    public class CreatureController : NetworkBehaviour
    {
        [Inject] private ITickManager _tickManager;
        
        public Creature Creature { private set; get; }

        protected virtual void Awake()
        {
            Creature = GetComponent<Creature>();
        }

        protected virtual void Start()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            this.Inject();
            
            _tickManager.OnShortTickEvent += OnShortTick;
            _tickManager.OnLongTickEvent += OnLongTick;
            
            OnShortTick();
            OnLongTick();
        }

        public virtual void OnShortTick()
        {
        }

        public virtual void OnLongTick()
        {
        }
    }
}