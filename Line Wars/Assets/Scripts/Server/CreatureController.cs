using Unity.Netcode;
using UnityEngine;

namespace Server
{
    [RequireComponent(typeof(Creature))]
    public class CreatureController : NetworkBehaviour
    {
        public Creature Creature { private set; get; }
        
        private void Awake()
        {
            Creature = GetComponent<Creature>();
        }
    }
}