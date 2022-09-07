using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class NodeGenerator : MonoBehaviour
{
    public Vector2Int mapSize;
    [SerializeField] List<Vector2Int> blockedNodes;
    private Node[] map;
    private Pathfinder pathfinder;
    List<Vector2Int> path;
    // Start is called before the first frame update
    void Start()
    {
        pathfinder = new Pathfinder();
        NodeUtils.MapSize = mapSize;
        map = new Node[mapSize.x * mapSize.y];
        int ID = 0;
        for (int i = 0; i < mapSize.y; i++)
        {
            for (int j = 0; j < mapSize.x; j++)
            {
                map[ID] = new Node(ID, new Vector2Int(j, i));
                if (blockedNodes.Contains(new Vector2Int(i, j)))
                    map[ID].Block();
                ID++;
            }
        }

        path = pathfinder.GetPath(map, 
            map[NodeUtils.PositionToIndex(new Vector2Int(0, 0))], 
            map[NodeUtils.PositionToIndex(new Vector2Int(8, 3))]);

        //for (int i = 0; i < path.Count; i++)
        //{
        //    Debug.Log(path[i]);
        //}
    }

    private void OnDrawGizmos()
    {
        if (map == null)
            return;
        Gizmos.color = Color.green;
        GUIStyle style = new GUIStyle() { fontSize = 25  };
        foreach (Node node in map)
        {
            Vector3 worldPosition = new Vector3((float)node.position.x, (float)node.position.y, 0.0f);
            if (node.state == Node.NodeState.Blocked)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(worldPosition, 0.2f);
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.DrawWireSphere(worldPosition, 0.2f);
            }
            Handles.Label(worldPosition, node.position.ToString(), style);
        }

        if (path == null) return;

        List<Node> nodePath = new List<Node>();

        foreach (var pos in path)
        {
            nodePath.Add(map[NodeUtils.PositionToIndex(pos)]);
        }


        Gizmos.color = Color.red;
        foreach (Node node in nodePath)
        {
            Vector3 worldPosition = new Vector3((float)node.position.x, (float)node.position.y, 0.0f);
            Gizmos.DrawWireSphere(worldPosition, 0.2f);
        }
    }
}
