using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int health = 10;
    [SerializeField] int pointsPerKill = 1;
    [SerializeField] AudioClip robotHitSound = null;
    [SerializeField] AudioClip robotKillSound = null;

    [Header("Death Animation Parameters")]
    [SerializeField] float deathAnimationDropDistance = 2f;
    [SerializeField] float deathAnimationInitialDelay = 0.5f;
    [SerializeField] float deathAnimationdropDelay = 0.008f;
    [SerializeField] float deathAnimationFinalDelay = 1.5f;
    [SerializeField] int deathAnimationDropResolution = 10;
    [SerializeField] float deathAnimationDestroyDelay = 3f;
    [SerializeField] GameObject deathAnimationExplosionPrefab = null;

    // Cached References
    ScoreBoard scoreBoard = null;
    AudioSource audioSource = null;
    EnemySpawner enemySpawner = null;
    BaseHealth baseHealth = null;

    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        audioSource = GetComponent<AudioSource>();

        enemySpawner = FindObjectOfType<EnemySpawner>();
        baseHealth = FindObjectOfType<BaseHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int damage = collision.gameObject.GetComponentInParent<Tower>().GetDamage();

        Destroy(collision.gameObject);

        ProcessHit(damage);
    }

    private void ProcessHit(int damage)
    {
        health -= damage;

        if (health < 1)
        {
            if (robotKillSound)
            {
                audioSource.PlayOneShot(robotKillSound);
            }

            scoreBoard.IncreaseScore(pointsPerKill);

            StartCoroutine(BeginDeathSequence());
        }
        else
        {
            if (robotHitSound)
            {
                audioSource.PlayOneShot(robotHitSound);
            }
        }
    }

    private IEnumerator BeginDeathSequence()
    {
        GetComponent<EnemyMovement>().SetDead();

        CheckIfGameWon();

        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(deathAnimationInitialDelay);

        for (int i = 0; i < deathAnimationDropResolution; i++)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= deathAnimationDropDistance / deathAnimationDropResolution;

            transform.position = newPosition;

            yield return new WaitForSeconds(deathAnimationdropDelay);
        }
        yield return new WaitForSeconds(deathAnimationFinalDelay);

        Transform location = GetComponentInChildren<Animator>().gameObject.transform;

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }

        Instantiate(deathAnimationExplosionPrefab, location);
        Destroy(gameObject, deathAnimationDestroyDelay);
    }

    public void CheckIfGameWon()
    {
        if (enemySpawner.doneSpawning)
        {
            if (baseHealth.GetHealth() > 0)
            {
                EnemyMovement[] allEnemies = FindObjectsOfType<EnemyMovement>();

                if (allEnemies.Length == 0)
                {
                    FindObjectOfType<WinScreen>().WonGame();
                }
                else
                {
                    bool allEnemiesDead = true;

                    foreach (EnemyMovement enemy in allEnemies)
                    {
                        if (!enemy.isDead)
                        {
                            allEnemiesDead = false;
                            break;
                        }
                    }

                    if (allEnemiesDead)
                    {
                        FindObjectOfType<WinScreen>().WonGame();
                    }
                }
            }
        }
    }

    public IEnumerator BeginFailedSequence()
    {
        GetComponent<EnemyMovement>().SetDead();

        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(deathAnimationInitialDelay * 5);

        for (int i = 0; i < deathAnimationDropResolution * 5; i++)
        {
            
            Vector3 newPosition = transform.position;
            newPosition.y -= deathAnimationDropDistance / (deathAnimationDropResolution * 5);

            transform.position = newPosition;

            yield return new WaitForSeconds(deathAnimationdropDelay * 5);
        }
    }
}
