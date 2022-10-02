using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigateTilemap : MonoBehaviour
{
    class Node
    {
        Node _parent;
        Vector3Int _position;

        public int f = 0;
        public int g = 0;
        public int h = 0;

        public Node(Node parent, Vector3Int position)
        {
            _parent = parent;
            _position = position;
            f = g = h = 0;
        }

        public bool Equals(Node other)
        {
            return _position == other.GetPosition();
        }

        public Node GetParent()
        {
            return _parent;
        }

        public Vector3Int GetPosition()
        {
            return _position;
        }
    };

    Tilemap tilemap;

    // Start is called before the first frame update
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3Int> navigate(Vector3Int start, Vector3Int end, out bool reached)
    {
        var startNode = new Node(null, start);
        var endNode = new Node(null, end);

        List<Node> openedList = new List<Node>();
        List<Node> closedList = new List<Node>();

        List<Vector3Int> result = new List<Vector3Int>();
        List<Node> children = new List<Node>();
        List<Vector2Int> possible_moves = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.left, Vector2Int.down };

        openedList.Add(startNode);

        int it = 0;
        while (openedList.Count > 0 && it < 5000)
        {
            it++;
            // Retrieve the node with the lower f value from the openedList
            var currentNode = openedList[0];
            for (int i = 1; i < openedList.Count; ++i)
            {
                var node = openedList[i];
                if (node.f < currentNode.f)
                {
                    currentNode = node; 
                }
            }

            closedList.Add(currentNode);
            openedList.Remove(currentNode);

            // End condition
            if (currentNode.Equals(endNode))
            {
                var node = currentNode;
                while (node is not null)
                {
                    result.Add(node.GetPosition());
                    node = node.GetParent();
                }
                result.Reverse();
                reached = true;
                return result;
            }

            // Otherwise generate children
            children.Clear();
            foreach (var move in possible_moves)
            {
                var newPosition = currentNode.GetPosition() + new Vector3Int(move.x, move.y);

                // FIXME: check position against tilemap size

                // Check if move is possible
                var newTile = tilemap.GetTile<Tile>(newPosition);
                if (newTile.colliderType == Tile.ColliderType.None)
                {
                    var newNode = new Node(currentNode, newPosition);
                    children.Add(newNode);
                }
            }

            // Loop through children
            foreach (var child in children)
            {
                bool addChild = true;
                foreach (var node in closedList)
                {
                    if (node.Equals(child))
                        addChild = false;

                }
                if (addChild)
                {
                    child.g = currentNode.g + 1;
                    child.h = Mathf.RoundToInt(Mathf.Pow(child.GetPosition().x - endNode.GetPosition().x, 2) + Mathf.Pow(child.GetPosition().y - endNode.GetPosition().y, 2));
                    child.f = child.g + child.h;

                    // Only add the child if there is not already a shorted path leading to it
                    foreach (var openedNode in openedList)
                    {
                        if (child.Equals(openedNode) && child.g > openedNode.g)
                        {
                            addChild = false;
                        }
                    }
                    if (addChild)
                    {
                        openedList.Add(child);
                    }
                }   
            }
        }
        reached = false;
        return result;
    }
}
