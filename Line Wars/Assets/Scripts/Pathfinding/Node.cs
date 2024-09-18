using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    private readonly Dictionary<Node, NodePath> _cachedPaths = new();

    public void CachePath(Node target, NodePath path)
    {
        if (!_cachedPaths.ContainsKey(target))
        {
            _cachedPaths.Add(target, path);
        }
    }

    public NodePath? GetCachedPath(Node target)
    {
        if (_cachedPaths.ContainsKey(target))
        {
            return _cachedPaths[target];
        }

        return null;
    }
}