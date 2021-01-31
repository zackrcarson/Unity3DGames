using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float waypointDelay = 1f;

    // Cached references
    PathFinder pathFinder = null;

    // State variable
    List<Waypoint> path = new List<Waypoint>();

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = FindObjectOfType<PathFinder>();

        path = pathFinder.GetPath();

        StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<Waypoint> path)
    {
        foreach (Waypoint waypoint in path)
        {
            yield return new WaitForSeconds(waypointDelay);

            transform.position = waypoint.transform.position;
        }
    }
}
