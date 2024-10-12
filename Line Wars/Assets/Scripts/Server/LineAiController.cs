using System.Linq;
using Abstractions;
using Server.Debug;
using UnityEngine;
using Zenject;

namespace Server
{
    public class LineAiController : CreatureController
    {
        [Inject] IPathfinding _pathfinding;
        
        [field: SerializeField] private Transform Target { get; set; }
        
        private Vector2 _targetPosition;

        protected override void Awake()
        {
            base.Awake();
            Target = DebugGoal.Instance.transform;
        }

        public override void OnLongTick()
        {
            base.OnLongTick();
            
            var targetPosition = Target.position;
            
            if (_pathfinding.IsClearPath(Creature.transform.position, targetPosition))
            {
                Creature.SetPath(new Vector2[] { targetPosition });
                return;
            }

            var path = _pathfinding.GetPath(Creature.transform.position, targetPosition);
            if (path.Count > 0)
            {
                Creature.SetPath(path.Select(x => (Vector2)x.worldPosition).ToArray());
            }
        }
    }
}