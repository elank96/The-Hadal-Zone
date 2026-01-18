using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    // Reference to the grid used for node lookups.
    [SerializeField] private AStarGrid grid;

    // Auto-grab the grid if both components live on the same GameObject.
    private void Awake()
    {
        if (grid == null)
        {
            grid = GetComponent<AStarGrid>();
        }
    }

    // Compute a path from start to target and return world-space waypoints.
    public List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        if (grid == null)
        {
            return null;
        }

        AStarGrid.Node startNode = grid.NodeFromWorldPoint(startPos);
        AStarGrid.Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (!startNode.walkable || !targetNode.walkable)
        {
            return null;
        }

        List<AStarGrid.Node> openSet = new List<AStarGrid.Node>();
        HashSet<AStarGrid.Node> closedSet = new HashSet<AStarGrid.Node>();

        openSet.Add(startNode);

        // Standard A* search loop.
        while (openSet.Count > 0)
        {
            AStarGrid.Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (AStarGrid.Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    // Walk back through parents to build the final waypoint list.
    private List<Vector2> RetracePath(AStarGrid.Node startNode, AStarGrid.Node endNode)
    {
        List<AStarGrid.Node> path = new List<AStarGrid.Node>();
        AStarGrid.Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        List<Vector2> waypoints = new List<Vector2>(path.Count);
        for (int i = 0; i < path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition);
        }

        return waypoints;
    }

    // Heuristic distance with optional diagonal cost.
    private int GetDistance(AStarGrid.Node nodeA, AStarGrid.Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (grid != null && grid.AllowDiagonal)
        {
            int diagonal = Mathf.Min(dstX, dstY);
            int straight = Mathf.Abs(dstX - dstY);
            return diagonal * 14 + straight * 10;
        }

        return (dstX + dstY) * 10;
    }
}
