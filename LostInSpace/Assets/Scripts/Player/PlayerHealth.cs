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
    [SerializeField] Text pauseInfoDisplay = null;
    [SerializeField] Text ammoDisplay = null;
    [SerializeField] StartScreen startScreen = null;
    [SerializeField] Animator deathAnimator = null;

    // Cached References
    Rigidbody rigidBody = null;

    // State variables
    bool gameStarted = false;

    private void Start()
    {
        gameOverCanvas.enabled = false;
        ammoDisplay.enabled = false;
        pauseInfoDisplay.enabled = false;

        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DisplayHealth();
    }

    private void DisplayHealth()
    {
        if (gameStarted)
        {
            healthDisplay.text = playerHealth.ToString();
        }
        else
        {
            healthDisplay.enabled = false;
        }
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

        RigidbodyFirstPersonController fpsController = FindObjectOfType<RigidbodyFirstPersonController>();
        bool crouching = fpsController.Crouching;

        fpsController.enabled = false;

        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon) { weapon.isDead = true; }

        FindObjectOfType<WeaponSwitcher>().enabled = false;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            StartCoroutine(enemy.PlayerDead());
        }

        deathAnimator.enabled = true;

        
        if (!crouching)
        {
            deathAnimator.SetTrigger("playerDead");
        }

        StartCoroutine(gameOverCanvas.GetComponent<GameOverScreen>().PlayerDead());
    }

    public void IncreaseHealth(int amount)
    {
        playerHealth += amount;
    }

    public void TurnOnFlashLightAndGun()
    {
        startScreen.TurnOnFlashLightAndGun();

        gameStarted = true;

        healthDisplay.enabled = true;
        ammoDisplay.enabled = true;
    }

    public void TurnOnPlayerController()
    {
        pauseInfoDisplay.enabled = true;

        startScreen.TurnOnPlayerController();

        FindObjectOfType<PauseScreen>().canPause = true;
    }
}