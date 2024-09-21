using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Creature : NetworkBehaviour
{
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] private float speed = 5f;
    
    private List<Vector2> _path = new();

    public void SetPath(Vector2[] path)
    {
        _path = new List<Vector2>(path);
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (_path.Count > 0)
        {
            // draw path

            if (_path.Count > 0)
            {
                Debug.DrawLine(transform.position, _path[0], Color.green);
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    Debug.DrawLine(_path[i], _path[i + 1], Color.red);
                }
            }

            var targetPosition = _path[0];
            var direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                _path.RemoveAt(0);
            }
        }
    }
}