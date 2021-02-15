using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int playerHealth = 200;
    [SerializeField] float hitAudioVolume = 1f;
    [SerializeField] float ouchAudioVolume = 1f;
    [SerializeField] float deathAudioVolume = 1f;
    [SerializeField] AudioClip hitAudio = null;
    [SerializeField] AudioClip[] ouchAudioClips = null;
    [SerializeField] AudioClip deathAudio = null;

    [Header("Misc. Parameters")]
    [SerializeField] Canvas gameOverCanvas = null;
    [SerializeField] Canvas reticleCanvas = null;
    [SerializeField] Text healthDisplay = null;
    [SerializeField] Text pauseInfoDisplay = null;
    [SerializeField] Text ammoDisplay = null;
    [SerializeField] StartScreen startScreen = null;
    [SerializeField] Animator deathAnimator = null;

    // Cached References
    Rigidbody rigidBody = null;
    AudioSource audioSource = null;

    // State variables
    bool gameStarted = false;
    bool isDead = false;

    private void Start()
    {
        gameOverCanvas.enabled = false;
        ammoDisplay.enabled = false;
        pauseInfoDisplay.enabled = false;

        audioSource = GetComponent<AudioSource>();
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

        if (!isDead)
        {
            audioSource.PlayOneShot(hitAudio, hitAudioVolume);
            audioSource.PlayOneShot(RandomOuchSound(), ouchAudioVolume);
        }

        if (playerHealth <= 0 && !isDead)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        FindObjectOfType<PauseScreen>().DenyPause();

        audioSource.PlayOneShot(deathAudio, deathAudioVolume);

        isDead = true;

        FindObjectOfType<MusicPlayer>().PlayDeathMusic();

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

    private AudioClip RandomOuchSound()
    {
        if (ouchAudioClips.Length > 0)
        {
            int randomClipIndex = Random.Range(0, ouchAudioClips.Length);

            return ouchAudioClips[randomClipIndex];
        }
        else
        {
            return null;
        }
    }
}