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
    [SerializeField] GameObject enemyBombPrefab = null;
    [SerializeField] int baseDamagePerHit = 1;

    // Cached references
    PathFinder pathFinder = null;
    BaseHealth baseHealth = null;

    // State variable
    List<Waypoint> path = new List<Waypoint>();
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = FindObjectOfType<PathFinder>();
        baseHealth = FindObjectOfType<BaseHealth>();

        path = pathFinder.GetPath();

        StartCoroutine(FollowPath(path));
    }

    private IEnumerator FollowPath(List<Waypoint> path)
    {
        if (!isDead)
        {
            // Move off the enemy Ship
            isDead = true;

            ChangeFacingDirection(path[0].transform);

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

                ChangeFacingDirection(waypoint.transform);
                
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

            ChangeFacingDirection(pathFinder.GetBaseLocation());

            Transform baseTransform = pathFinder.GetBaseLocation();
            float finalDistanceToMove = Vector3.Distance(transform.position, baseTransform.position);
            for (int i = 0; i < enteringLeavingMoveFrames; i++)
            {
                transform.position = Vector3.MoveTowards(transform.position, baseTransform.position, finalDistanceToMove / enteringLeavingMoveFrames);

                yield return new WaitForSeconds(movementTimeDelay);
            }
            transform.position = baseTransform.position;

            DamageTown();

            GetComponent<EnemyDamage>().CheckIfGameWon();
        }
    }

    private void ChangeFacingDirection(Transform nextWaypointTransform)
    {
        Vector3 directionVector = Vector3.Normalize(nextWaypointTransform.position - transform.position);

        float yRotationAmount = 0f;

        if (directionVector == new Vector3(0f, 0f, 1.0f))
        {
            yRotationAmount = 180f;
        }
        else if (directionVector == new Vector3(1.0f, 0f, 0f))
        {
            yRotationAmount = 270f;
        }
        else if (directionVector == new Vector3(0f, 0f, -1.0f))
        {
            yRotationAmount = 0f;
        }
        else if (directionVector == new Vector3(-1.0f, 0f, 0f))
        {
            yRotationAmount = 90f; 
        }

        GetComponentInChildren<Animator>().gameObject.transform.localRotation = Quaternion.Euler(0f, yRotationAmount, 0f);
    }

    private void DamageTown()
    {
        baseHealth.ReduceHealth(baseDamagePerHit);

        Instantiate(enemyBombPrefab, pathFinder.GetBaseLocation());
        Destroy(gameObject);
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
