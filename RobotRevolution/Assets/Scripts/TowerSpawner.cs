using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    // Config Parameters
    [SerializeField] GameObject towerPrefab = null;

    public void PlaceTower(Transform location)
    {
        GameObject tower = Instantiate(towerPrefab, location) as GameObject;
        tower.transform.parent = transform;
    }
}
