using UnityEngine;
using System.Collections;

public class EnemyDamage : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int health = 10;

    [Header("Death Animation Parameters")]
    [SerializeField] float deathAnimationDropDistance = 2f;
    [SerializeField] float deathAnimationInitialDelay = 0.5f;
    [SerializeField] float deathAnimationdropDelay = 0.008f;
    [SerializeField] float deathAnimationFinalDelay = 1.5f;
    [SerializeField] int deathAnimationDropResolution = 10;
    [SerializeField] float deathAnimationDestroyDelay = 3f;
    [SerializeField] GameObject deathAnimationExplosionPrefab = null;

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
            StartCoroutine(BeginDeathSequence());
        }
    }

    private IEnumerator BeginDeathSequence()
    {
        GetComponent<EnemyMovement>().SetDead();

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
}
