using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Config Variables
    [Header("Miscellaneous Properties")]
    [SerializeField] bool isMainMenu = false;
    [SerializeField] ParticleSystem[] guns = null;
    [SerializeField] ParticleSystem[] thrusters = null;
    [SerializeField] GameObject shipShield = null;
    [SerializeField] AudioClip laserAudio = null;
    [SerializeField] float laserAudioDelay = 0.2f;

    [Header("Ship Translation Properties")]
    [Tooltip("In ms^-1")] [SerializeField] float xSpeed = 20f;
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 20f;
    [Tooltip("In m")] [SerializeField] float xRange = 3.2f;
    [Tooltip("In m")] [SerializeField] float yRange = 2.6f;

    [Header("Ship Rotation Properties")]
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float controlPitchFactor = -20f;

    [SerializeField] float positionYawFactor = 5f;

    [SerializeField] float controlRollFactor = -20f;

    // State Variables
    float xThrow, yThrow;
    bool isDead = false;
    bool isWon = false;
    bool isPaused = false;
    bool audioRunning = false;

    // Cached References
    AudioSource audioSource = null;
    PauseScreen pauseScreen = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        try
        {
            pauseScreen = Resources.FindObjectsOfTypeAll<PauseScreen>()[0];
        }
        catch
        {
            pauseScreen = null;
        }

        if (isMainMenu)
        {
            SetGunsActive(false);
            SetThrustersActive(true);
            shipShield.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMainMenu || isDead || isWon) { return; }

        ProcessPause();

        if (isPaused) { return; }

        ProcessShipTranslation();

        ProcessShipRotation();

        ProcessFiring();
    }

    private void ProcessPause()
    {
        if (!pauseScreen)
        {
            pauseScreen = Resources.FindObjectsOfTypeAll<PauseScreen>()[0];
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            pauseScreen.gameObject.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pauseScreen.gameObject.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
    }

    private void ProcessShipTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * xSpeed;
        float yOffset = yThrow * Time.deltaTime * ySpeed;

        float xPos = Mathf.Clamp(transform.localPosition.x + xOffset, -xRange, xRange);
        float yPos = Mathf.Clamp(transform.localPosition.y + yOffset, -yRange, yRange);
        float zPos = transform.localPosition.z;

        Vector3 newPosition = new Vector3(xPos, yPos, zPos);

        transform.localPosition = newPosition;
    }

    private void ProcessShipRotation()
    {
        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll =  xThrow * controlRollFactor;

        // (x, y, z) = (pitch, yaw, roll) in degrees. Applies it in the order z : x : y (or roll : pitch : yaw).
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll); 
    }

    private void ProcessFiring()
    {
        if (Input.GetButton("Fire"))
        {
            if (!audioRunning)
            {
                StartCoroutine(LaserAudio());
            }

            SetGunsActive(true);
        }
        else
        {
            audioRunning = false;
            StopCoroutine(LaserAudio());

            SetGunsActive(false);
        }
    }
    
    private void SetGunsActive(bool isActive)
    {
        foreach(ParticleSystem gun in guns)
        {
            var emissionModule = gun.emission;
            emissionModule.enabled = isActive;
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

    public void SetThrustersActive(bool isActive)
    {
        foreach (ParticleSystem thruster in thrusters)
        {
            var emissionModule = thruster.emission;
            emissionModule.enabled = isActive;
        }
    }

    // Called by a string reference when player dies.
    public void OnPlayerDeath()
    {
        SetGunsActive(false);
        SetThrustersActive(false);

        isDead = true;
    }

    public void UnpauseGame()
    {
        pauseScreen.gameObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void PlayerWon()
    {
        isWon = true;
    }
}
