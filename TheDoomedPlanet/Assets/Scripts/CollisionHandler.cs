using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    // Config Parameters
    [Header("Miscellaneous")]
    [SerializeField] int playerHealth = 3;
    [SerializeField] float levelLoadDelay = 2f;

    [Header("VFX")]
    [SerializeField] GameObject explosionVFX = null;
    [SerializeField] GameObject hitVFX = null;
    [SerializeField] float explosionDelay = 2f;

    [Header("Shield Parameters")]
    [SerializeField] AudioClip shieldActivatedAudio = null;
    [SerializeField] AudioClip shieldDeactivatedAudio = null;
    [SerializeField] GameObject shipShield = null;
    [SerializeField] float shieldDelayTime = 5f;
    [SerializeField] int numBlinks = 10;
    [SerializeField] float blinkTime = 0.05f;

    // Cached references
    int initialPlayerHealth = 3;
    HealthBar healthBar = null;
    MeshRenderer meshRenderer = null;
    PlayerController playerController = null;
    AudioSource audioSource = null;

    // State Variables
    bool shieldOn = true;
    bool isDead = false;
    bool isPlayerInvulnerable = false;

    private void Start()
    {
        initialPlayerHealth = playerHealth;
        healthBar = FindObjectOfType<HealthBar>();

        meshRenderer = GetComponent<MeshRenderer>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        explosionVFX.SetActive(false);

        shipShield.SetActive(true);
    }

    private void OnParticleCollision(GameObject otherCollider)
    {
        if (otherCollider.name == "Enemy Gun")
        {
            HandleDamage();
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        HandleDamage();
    }

    public void HandleDamage()
    {
        if (isPlayerInvulnerable) { return; }

        GameObject hit = Instantiate(hitVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(hit, explosionDelay);

        if (shieldOn)
        {
            StartCoroutine(DeactivateShipShields());

            StartCoroutine(SetShipInvulnerability());
        }
        else
        {
            playerHealth--;

            healthBar.UpdateHealthBar((float)playerHealth / (float)initialPlayerHealth);

            if (playerHealth <= 0)
            {
                StartDeathSequence();
            }
            else
            {
                StartCoroutine(SetShipInvulnerability());
            }
        }
    }

    private IEnumerator DeactivateShipShields()
    {
        audioSource.PlayOneShot(shieldDeactivatedAudio, 1f);

        healthBar.TurnOffShields();

        for (int i = 0; i < numBlinks; i++)
        {
            shieldOn = false;
            shipShield.SetActive(false);

            yield return new WaitForSeconds(blinkTime);

            shieldOn = true;
            shipShield.SetActive(true);

            yield return new WaitForSeconds(blinkTime);
        }
        shieldOn = false;
        shipShield.SetActive(false);

        yield return new WaitForSeconds(shieldDelayTime);

        if (!isDead)
        {
            audioSource.PlayOneShot(shieldActivatedAudio, 1f);

            healthBar.TurnOnShields();

            for (int i = 0; i < numBlinks; i++)
            {
                shieldOn = true;
                shipShield.SetActive(true);

                yield return new WaitForSeconds(blinkTime);

                shieldOn = false;
                shipShield.SetActive(false);

                yield return new WaitForSeconds(blinkTime);
            }
            shieldOn = true;
            shipShield.SetActive(true);
        }
        else
        {
            yield return null;
        }
        
    }

    private IEnumerator SetShipInvulnerability()
    {
        isPlayerInvulnerable = true;
        
        for (int i = 0; i < numBlinks; i++)
        {
            meshRenderer.enabled = false;
            playerController.SetThrustersActive(false);

            yield return new WaitForSeconds(blinkTime);

            meshRenderer.enabled = true;
            playerController.SetThrustersActive(true);

            yield return new WaitForSeconds(blinkTime);
        }

        isPlayerInvulnerable = false;

        meshRenderer.enabled = true;
        playerController.SetThrustersActive(true);
    }

    private void StartDeathSequence()
    {
        isDead = true;

        shipShield.SetActive(false);

        SendMessage("OnPlayerDeath");

        GetComponent<MeshRenderer>().enabled = false;

        explosionVFX.SetActive(true);

        StartCoroutine(LevelLoader.ReloadLevel(levelLoadDelay));
    }
}
