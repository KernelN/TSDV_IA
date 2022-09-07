using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public List<Vector2Int> GetPath(Node[] map, Node origin, Node destination)
    {
        Node currentNode = origin;
        while (currentNode.position != destination.position)
        {
            currentNode.state = Node.NodeState.Closed;

            for (int i = 0; i < currentNode.adjacentNodeIDs.Count; i++)
            {
                if (currentNode.adjacentNodeIDs[i] != -1)
                {
                    if (map[currentNode.adjacentNodeIDs[i]].state == Node.NodeState.Ready)
                    {
                        map[currentNode.adjacentNodeIDs[i]].Open(currentNode.ID);
                    }
                }
            }

            currentNode = GetNextNode(map, currentNode);
            if (currentNode == null)
                return new List<Vector2Int>();
        }

        List<Vector2Int> path = GeneratePath(map, origin, currentNode);
        foreach (Node node in map)
        {
            node.Reset();
        }
        return path;
    }

    private List<Vector2Int> GeneratePath(Node[] map, Node origin, Node current)
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

    private Node GetNextNode(Node[] map, Node currentNode)
    {
        for (int i = 0; i < currentNode.adjacentNodeIDs.Count; i++)
        {
            if (currentNode.adjacentNodeIDs[i] != -1)
            {
                if (map[currentNode.adjacentNodeIDs[i]].state == Node.NodeState.Open)
                {
                    //You should try every OPEN node, even if it wasn't opened by you
                    //if (map[currentNode.adjacentNodeIDs[i]].openerID == currentNode.ID)
                    {
                        return map[currentNode.adjacentNodeIDs[i]];
                    }
                }
            }
        }

        if (currentNode.openerID == -1)
            return null;

        return GetNextNode(map, map[currentNode.openerID]);
    }
}
