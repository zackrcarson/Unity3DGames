using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Config Parameters
    [Header("Enemy Parameters")]
    [SerializeField] int health = 20;
    [SerializeField] int points = 17;

    [Header("Death Related")]
    [SerializeField] float killDelay = 0f;
    [SerializeField] float explosionDelay = 2f;
    [SerializeField] GameObject explosionVFX = null;

    // Cached References
    ScoreBoard scoreBoard = null;
    PlayerController player = null;

    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        player = FindObjectOfType<PlayerController>();
        AddBoxCollider();
    }

    private void AddBoxCollider()
    {
        if (!GetComponent<BoxCollider>())
        {
            Collider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = false;
        }
    }

    private void OnParticleCollision(GameObject otherCollider)
    {
        health--;
         
        if (health <=0)
        {
            InitiateEnemyDeathSequence();
        }
    }

    private void InitiateEnemyDeathSequence()
    {
        scoreBoard.UpdateScore(points);

        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity) as GameObject;

        Destroy(explosion, explosionDelay);
        Destroy(gameObject, killDelay);
    }
}
