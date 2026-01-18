using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    // World-space width/height covered by the grid, centered on this transform.
    [SerializeField] private Vector2 gridWorldSize = new Vector2(40f, 40f);
    // Radius of each node used for spacing and obstacle checks.
    [SerializeField] private float nodeRadius = 0.5f;
    // 2D collider layers treated as unwalkable.
    [SerializeField] private LayerMask obstacleMask;
    // Allow diagonal neighbor connections.
    [SerializeField] private bool allowDiagonal = true;

    // Node lookup by grid index.
    private Node[,] grid;
    // Node spacing (radius * 2).
    private float nodeDiameter;
    // Grid resolution in nodes.
    private int gridSizeX;
    private int gridSizeY;

    // Expose diagonal setting to pathfinder.
    public bool AllowDiagonal => allowDiagonal;

    // Build the grid at runtime.
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.Max(1, Mathf.RoundToInt(gridWorldSize.x / nodeDiameter));
        gridSizeY = Mathf.Max(1, Mathf.RoundToInt(gridWorldSize.y / nodeDiameter));
        CreateGrid();
    }

    // Keep editor values consistent when tweaked in Inspector.
    private void OnValidate()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.Max(1, Mathf.RoundToInt(gridWorldSize.x / nodeDiameter));
        gridSizeY = Mathf.Max(1, Mathf.RoundToInt(gridWorldSize.y / nodeDiameter));
    }

    // Convert a world position to the nearest grid node.
    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2f) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2f) / gridWorldSize.y);

        int x = Mathf.Clamp(Mathf.RoundToInt((gridSizeX - 1) * percentX), 0, gridSizeX - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((gridSizeY - 1) * percentY), 0, gridSizeY - 1);
        return grid[x, y];
    }

    // Get neighboring nodes for A* expansion.
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                if (!allowDiagonal && Mathf.Abs(x) + Mathf.Abs(y) > 1)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    // Build nodes and mark walkability from obstacle overlaps.
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position
                                  - Vector2.right * gridWorldSize.x / 2f
                                  - Vector2.up * gridWorldSize.y / 2f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft
                                     + Vector2.right * (x * nodeDiameter + nodeRadius)
                                     + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, obstacleMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // Visualize the grid and blocked nodes in the editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1f));

        if (grid == null)
        {
            return;
        }

        foreach (Node node in grid)
        {
            Gizmos.color = node.walkable ? new Color(1f, 1f, 1f, 0.3f) : new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter * 0.5f));
        }
    }

    [System.Serializable]
    public class Node
    {
        // Pathfinding data.
        public bool walkable;
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public Node parent;

        // A* total cost.
        public int fCost => gCost + hCost;

        // Construct a grid node.
        public Node(bool walkable, Vector2 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}
