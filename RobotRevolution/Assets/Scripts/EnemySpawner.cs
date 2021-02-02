using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Config Params
    [SerializeField] float initialDelay = 2f;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] int numberOfSpawns = 6;
    [SerializeField] bool isSpawning = true;
    [SerializeField] EnemyMovement enemyPrefab = null;

    // State Variables
    int numberEnemiesSpawned = 0;

    private void Start()
    {
        FindObjectOfType<PathFinder>().GetPath();
    }

    public void GameStarted()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(initialDelay);

        while (isSpawning && numberEnemiesSpawned < numberOfSpawns)
        {
            Instantiate(enemyPrefab.gameObject, transform);
            numberEnemiesSpawned++;

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
