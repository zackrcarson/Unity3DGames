using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Config Parameters
    [Header("Enemy Parameters")]
    [SerializeField] bool doesShoot = true;
    [SerializeField] ParticleSystem[] guns = null;
    [SerializeField] int health = 20;
    [SerializeField] int points = 17;

    [Header("Damage VFX")]
    [SerializeField] float killDelay = 0f;
    [SerializeField] float explosionDelay = 2f;
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] GameObject hitVFX = null;

    [Header("Audio")]
    [SerializeField] AudioClip laserAudio = null;
    [SerializeField] float laserAudioDelay = 0.2f;

    // Cached References
    ScoreBoard scoreBoard = null;
    PlayerController player = null;
    AudioSource audioSource = null;

    // State Variables
    bool audioRunning = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        scoreBoard = FindObjectOfType<ScoreBoard>();
        player = FindObjectOfType<PlayerController>();
        AddBoxCollider();

        if (doesShoot)
        {
            StartCoroutine(LaserAudio());
        }
        else
        {
            foreach (ParticleSystem gun in guns)
            {
                gun.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator LaserAudio()
    {
        audioRunning = true;

        while (audioRunning)
        {
            audioSource.PlayOneShot(laserAudio);
            yield return new WaitForSeconds(laserAudioDelay);
        }
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
        if (otherCollider.name == "Enemy Gun") { return; }

        health--;
         
        if (health <=0)
        {
            InitiateEnemyDeathSequence();
        }
        else
        {
            GameObject hit = Instantiate(hitVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(hit, explosionDelay);
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
