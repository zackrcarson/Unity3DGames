using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int towerLimit = 5;
    [SerializeField] Tower towerPrefab = null;
    [SerializeField] AnimationClip burrowAnimationClipForLength = null;

    // State Variables
    Queue<Tower> towerQueue = new Queue<Tower>();

    public void PlaceTower(Waypoint waypoint)
    {
        int numTowers = towerQueue.Count;

        if (numTowers < towerLimit)
        {
            InstantiateNewTower(waypoint);
        }
        else
        {
            bool allNotIdling = true;
            Tower[] towers = FindObjectsOfType<Tower>();
            foreach (Tower tower in towers)
            {
                if (tower.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Tower Idle"))
                {
                    allNotIdling = false;
                }
            }

            if (!allNotIdling)
            {
                MoveExistingTower(waypoint);
            }
        }
    }

    private void InstantiateNewTower(Waypoint waypoint)
    {
        var newTower = Instantiate(towerPrefab, waypoint.transform.position, Quaternion.identity);
        newTower.transform.parent = transform;

        towerQueue.Enqueue(newTower);
        
        waypoint.isPlaceable = false;
        newTower.SetWaypoint(waypoint);
    }

    private void MoveExistingTower(Waypoint waypoint)
    {
        Tower oldTower = towerQueue.Dequeue();
        towerQueue.Enqueue(oldTower);

        oldTower.GetWaypoint().isPlaceable = true;
        waypoint.isPlaceable = false;

        oldTower.SetWaypoint(waypoint);

        StartCoroutine(BurrowMoveAndRiseTower(oldTower, waypoint));
    }

    private IEnumerator BurrowMoveAndRiseTower(Tower oldTower, Waypoint waypoint)
    {
        Animator animator = oldTower.GetComponent<Animator>();

        animator.SetTrigger("isDigging");

        yield return new WaitForSeconds(burrowAnimationClipForLength.length);

        oldTower.transform.position = waypoint.transform.position;

        oldTower.ResetTower();
        animator.SetTrigger("reset");
    }
}
