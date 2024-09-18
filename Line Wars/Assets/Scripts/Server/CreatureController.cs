using UnityEngine;

namespace Server
{
    [RequireComponent(typeof(Creature))]
    public class CreatureController : MonoBehaviour
    {
        public Creature Creature { private set; get; }
        
        private void Awake()
        {
            Creature = GetComponent<Creature>();
        }
    }
}