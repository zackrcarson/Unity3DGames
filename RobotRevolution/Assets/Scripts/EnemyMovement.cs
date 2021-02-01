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
            isDead = true;
            float distanceToMove = Vector3.Distance(transform.position, path[0].transform.position);
            for (int i = 0; i < 100; i ++)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, distanceToMove / 100);

                yield return new WaitForSeconds(0.01f);
            }
            transform.position = path[0].transform.position;
            isDead = false;

            foreach (Waypoint waypoint in path)
            {
                if (waypoint == path[0]) { continue; }

                yield return new WaitForSeconds(waypointDelay);

                transform.position = waypoint.transform.position;
            }
            yield return new WaitForSeconds(waypointDelay);

            isDead = true;
            Transform baseTransform = pathFinder.GetBaseLocation();
            float finalDistanceToMove = Vector3.Distance(transform.position, baseTransform.position);
            for (int i = 0; i < 100; i++)
            {
                transform.position = Vector3.MoveTowards(transform.position, baseTransform.position, finalDistanceToMove / 100);

                yield return new WaitForSeconds(0.01f);
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
