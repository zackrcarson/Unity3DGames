using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Config Params
    [SerializeField] float initialDelay = 2f;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] int numberOfSpawns = 6;
    [SerializeField] EnemyMovement enemyPrefab = null;
    [SerializeField] AudioClip robotSpawnSound = null;

    // State Variables
    int numberEnemiesSpawned = 0;
    public bool isSpawning = true;
    public bool doneSpawning = false;

    // Cached References
    AudioSource audioSource = null;

    private void Start()
    {
        FindObjectOfType<PathFinder>().GetPath();

        audioSource = GetComponent<AudioSource>();
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

            if (robotSpawnSound)
            {
                audioSource.PlayOneShot(robotSpawnSound);
            }

            yield return new WaitForSeconds(spawnTime);
        }

        doneSpawning = true;
    }
}
