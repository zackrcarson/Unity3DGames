using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int playerHealth = 200;
    [SerializeField] Canvas gameOverCanvas = null;
    [SerializeField] Canvas reticleCanvas = null;
    [SerializeField] Text healthDisplay = null;
    [SerializeField] StartScreen startScreen = null;
    [SerializeField] Animator deathAnimator = null;

    // Cached References
    Rigidbody rigidBody = null;

    private void Start()
    {
        gameOverCanvas.enabled = false;

        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DisplayHealth();
    }

    private void DisplayHealth()
    {
        healthDisplay.text = playerHealth.ToString();
    }

    public void DamagePlayer(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        reticleCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GetComponent<RigidbodyFirstPersonController>().enabled = false;
        GetComponentInChildren<Weapon>().isDead = true;
        FindObjectOfType<WeaponSwitcher>().enabled = false;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            StartCoroutine(enemy.PlayerDead());
        }

        deathAnimator.enabled = true;

        deathAnimator.SetTrigger("playerDead");

        StartCoroutine(gameOverCanvas.GetComponent<GameOverScreen>().PlayerDead());
    }

    public void TurnOnFlashLightAndGun()
    {
        startScreen.TurnOnFlashLightAndGun();
    }

    public void TurnOnPlayerController()
    {
        startScreen.TurnOnPlayerController();
    }
}