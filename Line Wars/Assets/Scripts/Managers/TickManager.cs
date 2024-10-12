using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public interface ITickManager
    {
        event Action OnShortTickEvent;
        event Action OnLongTickEvent;
    }

    public class TickManager : NetworkBehaviour, ITickManager
    {
        public const float LongTickRate = 1;
        public const float ShortTickRate = 5;
     
        public event Action OnShortTickEvent;
        public event Action OnLongTickEvent;
        
        private void Start()
        {
            if (!IsServer)
            {
                return;
            }

            StartCoroutine(ShortTickCoroutine());
            StartCoroutine(LongTickCoroutine());
        }

        private IEnumerator ShortTickCoroutine()
        {
            var wait = new WaitForSeconds(1f / ShortTickRate);

            while (true)
            {
                OnShortTick();
                yield return wait;
            }

            // ReSharper disable once IteratorNeverReturns
        }

        private IEnumerator LongTickCoroutine()
        {
            var wait = new WaitForSeconds(1f / LongTickRate);

            while (true)
            {
                OnLongTick();
                yield return wait;
            }

            // ReSharper disable once IteratorNeverReturns
        }

        public virtual void OnShortTick()
        {
            OnShortTickEvent?.Invoke();
        }

        public virtual void OnLongTick()
        {
            OnLongTickEvent?.Invoke();
        }
    }
}