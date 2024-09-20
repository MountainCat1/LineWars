using System.Collections.Generic;

public class NodePath : List<Node>
{
    public static NodePath Empty => new();
}

public static class PathExtensions
{
    public static NodePath ToPath(this List<Node> nodes)
    {
        NodePath nodePath = new NodePath();
        nodePath.AddRange(nodes);
        return nodePath;
    }
}