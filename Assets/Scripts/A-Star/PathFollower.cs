using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [Header("References")]
    // A* solver used to build waypoint paths.
    [SerializeField] private AStarPathfinder pathfinder;
    // Object this agent moves toward.
    [SerializeField] private Transform target;
    // Optional 3D Rigidbody for physics-based movement.
    [SerializeField] private Rigidbody rb3D;

    [Header("Movement")]
    // Units per second toward the next waypoint.
    [SerializeField] private float moveSpeed = 3f;
    // Distance to a waypoint before advancing to the next one.
    [SerializeField] private float waypointTolerance = 0.1f;
    // How often to recompute the path (seconds).
    [SerializeField] private float repathInterval = 0.5f;

    // Cached waypoint list from the pathfinder.
    private List<Vector2> path;
    // Current waypoint index in the path.
    private int pathIndex;
    // Timer for repathing.
    private float repathTimer;

    // Auto-assign references if not set in Inspector.
    private void Awake()
    {
        if (pathfinder == null)
        {
            pathfinder = FindObjectOfType<AStarPathfinder>();
        }

        if (rb3D == null)
        {
            rb3D = GetComponent<Rigidbody>();
        }
    }

    // Recompute path on a timer or if no path exists.
    private void Update()
    {
        if (pathfinder == null || target == null)
        {
            return;
        }

        repathTimer += Time.deltaTime;
        if (repathTimer >= repathInterval || path == null || path.Count == 0)
        {
            Vector2 from = rb3D != null
                ? new Vector2(rb3D.position.x, rb3D.position.y)
                : new Vector2(transform.position.x, transform.position.y);
            Vector2 to = new Vector2(target.position.x, target.position.y);
            path = pathfinder.FindPath(from, to);
            pathIndex = 0;
            repathTimer = 0f;
        }
    }

    // Move toward the current waypoint using physics if available.
    private void FixedUpdate()
    {
        if (path == null || pathIndex >= path.Count)
        {
            return;
        }

        Vector2 currentPos = rb3D != null
            ? new Vector2(rb3D.position.x, rb3D.position.y)
            : new Vector2(transform.position.x, transform.position.y);
        Vector2 nextPoint = path[pathIndex];
        Vector2 toTarget = nextPoint - currentPos;

        if (toTarget.magnitude <= waypointTolerance)
        {
            pathIndex++;
            return;
        }

        Vector2 moveDelta = toTarget.normalized * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPos = currentPos + moveDelta;

        if (rb3D != null)
        {
            Vector3 currentPos3D = rb3D.position;
            rb3D.MovePosition(new Vector3(newPos.x, newPos.y, currentPos3D.z));
            return;
        }

        float currentZ = transform.position.z;
        transform.position = new Vector3(newPos.x, newPos.y, currentZ);
    }

    // Assign a new target and force an immediate repath.
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        path = null;
        pathIndex = 0;
        repathTimer = repathInterval;
    }
}
