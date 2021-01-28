using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int playerHealth = 3;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] GameObject explosionVFX = null;

    [Header("Shield Parameters")]
    [SerializeField] GameObject shipShield = null;
    [SerializeField] float shieldDelayTime = 5f;
    [SerializeField] int numBlinks = 10;
    [SerializeField] float blinkTime = 0.05f;

    // Cached references
    int initialPlayerHealth = 3;
    HealthBar healthBar = null;
    MeshRenderer meshRenderer = null;
    PlayerController playerController = null;

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

        explosionVFX.SetActive(false);

        shipShield.SetActive(true);
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (isPlayerInvulnerable) { return; }

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
                StartDeathSequence(otherCollider);
            }
            else
            {
                StartCoroutine(SetShipInvulnerability());
            }
        }
    }

    private IEnumerator DeactivateShipShields()
    {
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

    private void StartDeathSequence(Collider otherCollider)
    {
        isDead = true;

        shipShield.SetActive(false);

        SendMessage("OnPlayerDeath");

        GetComponent<MeshRenderer>().enabled = false;

        explosionVFX.SetActive(true);

        StartCoroutine(LevelLoader.ReloadLevel(levelLoadDelay));
    }
}
