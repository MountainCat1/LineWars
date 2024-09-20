using System.Collections.Generic;
using System.Linq;
using Server;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Client
{
    public class HeroController : CreatureController
    {
        [Inject] IInputMapper _inputMapper;
        [Inject] IPathfinding _pathfinding;

        private List<Vector2> _path = new();

        [SerializeField] private float speed = 5f;

        private void Start()
        {
            FindObjectOfType<SceneContext>().Container.Inject(this);
            
            if (!IsOwner)
            {
                enabled = false;
                return;
            }


            _inputMapper.Moved += OnMoved;
        }

        private void OnMoved(Vector2 targetPosition)
        {
            SetTargetServerRpc(targetPosition);
        }

        [ServerRpc]
        private void SetTargetServerRpc(Vector2 targetPosition)
        {
            SetTargetPosition(targetPosition);
        }

        // [ServerRpc]
        private void SetTargetPosition(Vector2 targetPosition)
        {
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