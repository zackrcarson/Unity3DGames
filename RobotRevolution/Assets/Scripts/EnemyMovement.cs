using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float waypointDelay = 1f;
    [SerializeField] int waypointMoveFrames = 30;
    [SerializeField] int enteringLeavingMoveFrames = 100;
    [SerializeField] float movementTimeDelay = 0.01f;
    
    // Cached references
    PathFinder pathFinder = null;

    // State variable
    List<Waypoint> path = new List<Waypoint>();
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = FindObjectOfType<PathFinder>();

        path = pathFinder.GetPath();

        StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<Waypoint> path)
    {
        if (!isDead)
        {
            // Move off the enemy Ship
            isDead = true;
            float distanceToMove = Vector3.Distance(transform.position, path[0].transform.position);
            for (int i = 0; i < enteringLeavingMoveFrames; i ++)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, distanceToMove / enteringLeavingMoveFrames);

                yield return new WaitForSeconds(movementTimeDelay);
            }
            transform.position = path[0].transform.position;
            isDead = false;

            // Move between each waypoint in the path
            yield return new WaitForSeconds(waypointDelay);
            foreach (Waypoint waypoint in path)
            {
                if (waypoint == path[0]) { continue; }

                float waypointDistance = Vector3.Distance(transform.position, waypoint.transform.position);
                for (int i = 0; i < waypointMoveFrames; i++)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoint.transform.position, waypointDistance / waypointMoveFrames);

                    yield return new WaitForSeconds(movementTimeDelay);
                }
                transform.position = waypoint.transform.position;

                yield return new WaitForSeconds(waypointDelay);
            }

            // Move onto the player base
            isDead = true;
            Transform baseTransform = pathFinder.GetBaseLocation();
            float finalDistanceToMove = Vector3.Distance(transform.position, baseTransform.position);
            for (int i = 0; i < enteringLeavingMoveFrames; i++)
            {
                transform.position = Vector3.MoveTowards(transform.position, baseTransform.position, finalDistanceToMove / enteringLeavingMoveFrames);

                yield return new WaitForSeconds(movementTimeDelay);
            }
            transform.position = baseTransform.position;
            Destroy(gameObject);
        }
    }

    public void SetDead()
    {
        isDead = true;
        StopAllCoroutines();
    }

    public bool GetDead()
    {
        return isDead;
    }
}
