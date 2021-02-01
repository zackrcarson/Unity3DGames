using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    /// <summary>
    /// 
    /// Pathfinding Algorithms Comparison
    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    /// 
    /// Pathfinding Algorithm         |   Always shortest      Varying Movement Cost       Multiple End points     Speed (for 100x100)                         Notes
    /// ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// Breadth First Search (BFS)    |         y                        n                          y              V+E        (medium)          Simple, versatile, fast enough most of the time
    /// Dijkstra's Algorithm          |         y                        y                          y              E+VlogV    (slow)            Movement cost allows for swamps, roads, etc
    /// A Star                        |      choice                      y                          n              Varies     (fast)            Complex. Can trade speed against accuracy
    /// 
    /// Breadth First Search (BFS) Algorithm
    /// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    /// 
    /// To find all the paths:
    /// 1) Start at the starting waypoint
    /// 2) Add the starting waypoint to the queue
    /// 3) while waypoints in the queue
    ///     - Dequeue the queue (this takes out the first thing entered to it), this element popped out is our search center
    ///     - Search all the neighbors from the center, adding each to the queue if they haven't already been added or searched in the past
    ///     - Add a breadcrumb trail, each time we search a waypoint, add a "fromWaypoint" that it was searched from so we can work backwards later)
    ///     - break when we find the finishing waypoint
    /// 
    /// Then to calculate the path:
    /// 1) Start at the finish waypoint
    /// 2) Add it to a list
    /// 3) Add all the intermediate waypoints in the breadcrumb trail in a loop
    /// 4) Add the start Waypoint
    /// 5) Reverse the list to get the forward path
    /// 
    /// </summary>

    // Config Parameters
    [Header("Waypoint Properties")]
    [SerializeField] Waypoint startWaypoint = null;
    [SerializeField] Waypoint finishWaypoint = null;
    [SerializeField] Transform baseLocation = null;

    // State Variables
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left};
    Queue<Waypoint> queue = new Queue<Waypoint>();
    List<Waypoint> path = new List<Waypoint>();

    Waypoint searchCenter = null;
    bool isRunning = true;

    bool isPathFound = false;

    public List<Waypoint> GetPath()
    {
        if (!isPathFound)
        {
            CalculatePath();
        }
        
        return path;
    }

    private void CalculatePath()
    {
        LoadBlocks();

        BreadthFirstSearch();

        FormPath();
    }

    private void LoadBlocks()
    {
        Waypoint[] waypoints = GetComponentsInChildren<Waypoint>();

        foreach (Waypoint waypoint in waypoints)
        {
            Vector2Int key = waypoint.GetGridPosition();
            bool isKeyOverlapping = grid.ContainsKey(key);

            if (isKeyOverlapping)
            {
                Debug.LogWarning("Skipping overlapping block at " + key);
            }
            else
            {
                grid.Add(key, waypoint);
            }
        }
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;

            ExploreNeighbors();

            FinishIfEndFound();
        }
    }

    private void FormPath()
    {
        Waypoint workingWaypoint = finishWaypoint;
        while (workingWaypoint != null)
        {
            path.Add(workingWaypoint);
            workingWaypoint.isPlaceable = false;

            workingWaypoint = workingWaypoint.exploredFrom;
        }

        path.Reverse();

        isPathFound = true;
    }


    private void ExploreNeighbors()
    {
        if (!isRunning) { return; }

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinate = searchCenter.GetGridPosition() + direction;

            if (grid.ContainsKey(neighborCoordinate))
            {
                QueueNewNeighbors(neighborCoordinate);
            }
        }
    }

    private void QueueNewNeighbors(Vector2Int neighborCoordinate)
    {
        Waypoint neighbor = grid[neighborCoordinate];
        
        if (neighbor.isExplored || queue.Contains(neighbor) || neighbor == finishWaypoint)
        {
            if (neighbor == finishWaypoint)
            {
                finishWaypoint.exploredFrom = searchCenter;
                isRunning = false;
            }
        }
        else
        {
            queue.Enqueue(neighbor);

            neighbor.exploredFrom = searchCenter;
        }
    }

    private void FinishIfEndFound()
    {
        if (searchCenter == finishWaypoint)
        {
            isRunning = false;
        }
    }

    public Transform GetBaseLocation()
    {
        return baseLocation;
    }
}
