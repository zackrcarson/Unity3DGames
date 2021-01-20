using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] GameObject cloudPrefab = null;
    [SerializeField] float xMin = 0;
    [SerializeField] float xMax = 0;
    [SerializeField] float spawnY = -40f;
    [SerializeField] float zMin = 0;
    [SerializeField] float zMax = 0;

    [SerializeField] float radiusMin = 0;
    [SerializeField] float radiusMax = 0;
    [SerializeField] float speedMin = 0;
    [SerializeField] float speedMax = 0;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] float deleteDelay = 20f;

    bool spawn = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRandomClouds());
    }

    // Update is called once per frame
    private IEnumerator SpawnRandomClouds()
    {
        while (spawn)
        {
            float spawnX = Random.Range(xMin, xMax);
            float spawnZ = Random.Range(zMin, zMax);
            Vector3 spawnVector = new Vector3(spawnX, spawnY, spawnZ);

            float spawnVelocity = Random.Range(speedMin, speedMax);
            float spawnRadius = Random.Range(radiusMin, radiusMax);

            GameObject newCloud = Instantiate(cloudPrefab, spawnVector, Quaternion.identity) as GameObject;

            newCloud.GetComponent<VerticalDrift>().SetSpeed(spawnVelocity);
            newCloud.GetComponent<VerticalDrift>().SetRadius(spawnRadius);

            Destroy(newCloud, deleteDelay);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public float GetVelocity()
    {
        return Random.Range(speedMin, speedMax);
    }
}
