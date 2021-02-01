using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Transform objectToPan = null;
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileParent = null;
    [SerializeField] float fireDelayTime = 2f;
    [SerializeField] float attackRange = 30;
    [SerializeField] int damage = 2;

    // State Varbiables
    Transform targetEnemy = null;

    bool shootingEnabled = true;
    bool allEnemiesDead = false;

    // Cached References
    Quaternion initialRotation = new Quaternion(0f, 0f, 0f, 0f);

    private void Start()
    {
        initialRotation = objectToPan.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        FindTargetEnemy();

        if (targetEnemy && !allEnemiesDead)// && !targetEnemy.gameObject.GetComponent<EnemyMovement>().GetDead())
        {
            FollowEnemies();

            ShootEnemies();
        }
        else
        {
            shootingEnabled = true;
            StopAllCoroutines();

            objectToPan.localRotation = initialRotation;
        }
    }

    private void FindTargetEnemy()
    {
        EnemyMovement[] enemiesOnScreen = FindObjectsOfType<EnemyMovement>();
        if (enemiesOnScreen.Length == 0) { return; }

        Transform closestEnemy = enemiesOnScreen[0].transform;

        allEnemiesDead = true;
        foreach (EnemyMovement testEnemy in enemiesOnScreen)
        {
            closestEnemy = GetClosest(closestEnemy, testEnemy.transform);

            targetEnemy = closestEnemy;

            if (!testEnemy.GetDead())
            {
                allEnemiesDead = false;
            }
        }
    }

    private Transform GetClosest(Transform enemy1, Transform enemy2)
    {
        float distance1 = Vector3.Distance(transform.position, enemy1.position);
        float distance2 = Vector3.Distance(transform.position, enemy2.position);

        if (distance1 < distance2)
        {
            return enemy1;
        }
        else
        {
            return enemy2;
        }
    }

    private void FollowEnemies()
    {
        objectToPan.LookAt(targetEnemy);
    }

    private void ShootEnemies()
    {
        float distanceToEnemy = Vector3.Distance(targetEnemy.position, gameObject.transform.position);

        if (distanceToEnemy < attackRange && !targetEnemy.GetComponent<EnemyMovement>().GetDead())
        {
            if (shootingEnabled)
            {
                shootingEnabled = false;

                StartCoroutine(DelayedFire());
            }
        }
        else
        {
            shootingEnabled = true;
            StopAllCoroutines();
        }
    }

    private IEnumerator DelayedFire()
    {
        while (true)
        {
            Instantiate(projectilePrefab, projectileParent);

            yield return new WaitForSeconds(fireDelayTime);
        }
    }

    public int GetDamage()
    {
        return damage;
    }
}