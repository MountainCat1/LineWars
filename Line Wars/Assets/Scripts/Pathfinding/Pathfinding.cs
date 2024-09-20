using System.Collections.Generic;
using UnityEngine;

public interface IPathfinding
{
    NodePath GetPath(Vector3 startPos, Vector3 targetPos);
    bool IsClearPath(Vector2 a, Vector2 b);
}

[RequireComponent(typeof(GridGenerator))]
public class Pathfinding : MonoBehaviour, IPathfinding
{
    private GridGenerator _grid;

    void Awake()
    {
        _grid = GetComponent<GridGenerator>();
    }

    public NodePath GetPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        NodePath cachedPath = startNode.GetCachedPath(targetNode);
        if (cachedPath != null)
            return cachedPath;

        NodePath path = FindPath(startPos, targetPos);
        return path;
    }
    
    public NodePath FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                var path = RetracePath(startNode, targetNode);

                AddCache(path);
                
                return path;
            }

            foreach (Node neighbour in _grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        // Path not found
        return NodePath.Empty;
    }

    private void AddCache(NodePath path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            path[i].CachePath(path[path.Count - 1], path.GetRange(i, path.Count - i).ToPath());
        }
    }

    public bool IsClearPath(Vector2 a, Vector2 b)
    {
        // Calculate the direction from point a to point b
        Vector2 direction = b - a;
        float distance = direction.magnitude;

        // Perform the Raycast
        RaycastHit2D hit = Physics2D.Raycast(a, direction, distance, _grid.unwalkableMask);
        if (hit.collider != null)
        {
            // If a collider is hit on the specified layer, return false
            return false;
        }

        // If no collider is hit on the specified layer, return true
        return true;
    }


    NodePath RetracePath(Node startNode, Node endNode)
    {
        List<Node> nodes = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            nodes.Add(currentNode);
            currentNode = currentNode.parent;
        }
        nodes.Reverse();

        return nodes.ToPath();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
