using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public List<Vector2Int> GetPath(PathNode[] map, PathNode origin, PathNode destination)
    {
        PathNode currentPathNode = origin;
        while (currentPathNode.position != destination.position)
        {
            currentPathNode.state = PathNode.NodeState.Closed;

            for (int i = 0; i < currentPathNode.adjacentNodeIDs.Count; i++)
            {
                if (currentPathNode.adjacentNodeIDs[i] != -1)
                {
                    if (map[currentPathNode.adjacentNodeIDs[i]].state == PathNode.NodeState.Ready)
                    {
                        map[currentPathNode.adjacentNodeIDs[i]].Open(currentPathNode.ID);
                    }
                }
            }

            currentPathNode = GetNextNode(map, currentPathNode);
            if (currentPathNode == null)
                return new List<Vector2Int>();
        }

        List<Vector2Int> path = GeneratePath(map, origin, currentPathNode);
        foreach (PathNode node in map)
        {
            node.Reset();
        }
        return path;
    }

    private List<Vector2Int> GeneratePath(PathNode[] map, PathNode origin, PathNode current)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        while (current.openerID != -1)
        {
            path.Add(current.position);
            current = map[current.openerID];
        }

        path.Add(origin.position); //origin is not included by default, 'cause id is -1
        path.Reverse();

        return path;
    }

    private PathNode GetNextNode(PathNode[] map, PathNode currentPathNode)
    {
        for (int i = 0; i < currentPathNode.adjacentNodeIDs.Count; i++)
        {
            if (currentPathNode.adjacentNodeIDs[i] != -1)
            {
                if (map[currentPathNode.adjacentNodeIDs[i]].state == PathNode.NodeState.Open)
                {
                    //You should try every OPEN node, even if it wasn't opened by you
                    //if (map[currentNode.adjacentNodeIDs[i]].openerID == currentNode.ID)
                    {
                        return map[currentPathNode.adjacentNodeIDs[i]];
                    }
                }
            }
        }

        if (currentPathNode.openerID == -1)
            return null;

        return GetNextNode(map, map[currentPathNode.openerID]);
    }
}
