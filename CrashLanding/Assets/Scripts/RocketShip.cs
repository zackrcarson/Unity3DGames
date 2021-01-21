using System.Collections;
using UnityEngine;

public class RocketShip : MonoBehaviour
{
    // Config Parameters
    [Header("Rocket Thrust")]
    [SerializeField] float boosterThrust = 10f;
    [SerializeField] float rotationThrust = 10f;

    [Header("Fuel Related")]
    [SerializeField] float rocketFuel = 100f;
    [SerializeField] float fuelConsumptionPerFrame = 1f;
    [SerializeField] Color fuelOutColor = new Color(1, 0, 0);

    [Header("Extra Gravity Forces")]
    [SerializeField] float extraGravityForce = 1f;
    [SerializeField] float extraGravityForceMainMenu = 40f;
    [SerializeField] float extraGravityForceWinScene = 10f;
    [SerializeField] float extraGravityUpwardsForceWinScene = 30f;

    [Header("Miscellaneous")]
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] Vector3 deathLaunchVector = new Vector3(5f, 5f, 0f);
    [SerializeField] bool debugKeysOn = false;
    [SerializeField] bool freezeMovement = false;
    [SerializeField] bool isWinScreen = false;

    [Header("Visual Effects")]
    [SerializeField] float VFXLifetime = 0.5f;
    [SerializeField] ParticleSystem thrusterVFXGameObject = null;
    [SerializeField] GameObject explosionVFXPrefab = null;
    [SerializeField] GameObject successVFXPrefab = null;

    [Header("Audio")]
    [SerializeField] float audioVolume = 0.2f;
    [SerializeField] AudioClip thrusterSound = null;
    [SerializeField] AudioClip explosionSound = null;
    [SerializeField] AudioClip successSound = null;
    [SerializeField] AudioClip fuelOutSound = null;

    // Cached References
    Rigidbody myRigidBody = null;
    AudioSource myAudioSource = null;

    // State variables
    bool isTransitioning = false;
    bool isConstantForceEnabled = true;
    bool killSwitch = true;
    bool mainMenuStart = false;
    bool isTouchedDown = false;
    bool winScreenRestart = false;
    bool isOutOfFuel = false;
    bool hasStoppedAudioAndThrust = false;

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();

        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.volume = audioVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOutOfFuel)
        {
            NonWinScreenUpdate();
        }
        else
        {
            OutOfFuelSequence();
        }

        WinScreenUpdate();
    }

    private void OutOfFuelSequence()
    {
        CheckDebugKeys();

        if (!hasStoppedAudioAndThrust)
        {
            StopApplyingThrust();

            StopApplyingThrust();
            myAudioSource.PlayOneShot(fuelOutSound);

            hasStoppedAudioAndThrust = true;
        }
    }

    private void NonWinScreenUpdate()
    {
        if (!isWinScreen)
        {
            if (!freezeMovement)
            {
                CheckDebugKeys();

                if (isTransitioning) { return; }

                RespondToThrustInput();
                RespondToRotateInput();

                ApplyConstantDownwardsForce();
            }
            else if (freezeMovement && !mainMenuStart)
            {
                if (!myAudioSource.isPlaying)
                {
                    myAudioSource.PlayOneShot(thrusterSound);
                }
            }

            if (mainMenuStart)
            {
                myAudioSource.Stop();
                ApplyConstantDownwardsForceMainMenu();
            }
        }
    }

    private void WinScreenUpdate()
    {
        if (isWinScreen && !isTouchedDown && !winScreenRestart)
        {
            killSwitch = false;

            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(thrusterSound);
            }

            ApplyConstantDownwardsForceWinScene();
        }

        if (isWinScreen && isTouchedDown && !winScreenRestart)
        {
            if (myAudioSource.isPlaying)
            {
                myAudioSource.Stop();
            }

            ApplyConstantDownwardsForceWinScene();
        }

        if (isWinScreen && winScreenRestart)
        {
            ApplyConstantUpwardsForceWinScene();
        }
    }

    private void CheckDebugKeys()
    {
        if (debugKeysOn)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LevelLoader.LoadNextLevel();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelLoader.ReloadCurrentLevel();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                killSwitch = !killSwitch;
            }
        }
    }

    private void ApplyConstantDownwardsForce()
    {
        if (isConstantForceEnabled)
        {
            myRigidBody.AddForce(0f, -extraGravityForce, 0f);
        }
    }

    private void ApplyConstantDownwardsForceMainMenu()
    {
        myRigidBody.AddForce(0f, -extraGravityForceMainMenu, 0f);
    }

    private void ApplyConstantDownwardsForceWinScene()
    {
        myRigidBody.AddForce(0f, -extraGravityForceWinScene, 0f);
    }

    private void ApplyConstantUpwardsForceWinScene()
    {
        myRigidBody.AddForce(0f, extraGravityUpwardsForceWinScene, 0f);
    }

    private void OnCollisionEnter(Collision otherCollider)
    {
        if (isTransitioning) { return; }

        switch (otherCollider.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                if (killSwitch)
                {
                    StartDeathsequence();
                }
                break;
        }

        if (isWinScreen && otherCollider.gameObject.tag == "Ground")
        {
            isTouchedDown = true;

            myAudioSource.Stop();

            thrusterVFXGameObject.Stop();
        }
    }

    private void StartDeathsequence()
    {
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(explosionSound);

        GameObject tempExplosion = Instantiate(explosionVFXPrefab, gameObject.transform) as GameObject;
        Destroy(tempExplosion, VFXLifetime);

        myRigidBody.AddForce(deathLaunchVector);

        StartCoroutine(DelayReloadLevel());
    }

    private void StartSuccessSequence()
    {
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(successSound);

        GameObject tempSuccess = Instantiate(successVFXPrefab, gameObject.transform) as GameObject;
        Destroy(tempSuccess, VFXLifetime);

        StartCoroutine(DelayLoadNextLevel());
    }

    private void RespondToThrustInput()
    {
        if (Input.GetAxisRaw("Thrusters") !=0)
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        myAudioSource.Stop();

        thrusterVFXGameObject.Stop();

        isConstantForceEnabled = true;
    }

    private void ApplyThrust()
    {
        HandleRocketFuelConsumption();

        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(thrusterSound);
        }

        thrusterVFXGameObject.Play();

        isConstantForceEnabled = false;

        Vector3 forceVector = Vector3.up * boosterThrust * Time.deltaTime;

        myRigidBody.AddRelativeForce(forceVector);
    }

    private void HandleRocketFuelConsumption()
    {
        rocketFuel -= fuelConsumptionPerFrame * Time.deltaTime;

        if (rocketFuel <= 0)
        {
            isOutOfFuel = true;

            Light[] shipLights = GetComponentsInChildren<Light>();
            foreach (Light light in shipLights)
            {
                light.color = fuelOutColor;
            }

            StopApplyingThrust();
        }
    }

    private void RespondToRotateInput()
    {
        myRigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = Time.deltaTime * rotationThrust * Input.GetAxisRaw("Rotational");
        Vector3 rotateVector = Vector3.back * rotationThisFrame;

        transform.Rotate(rotateVector);
    }

    private IEnumerator DelayLoadNextLevel()
    {
        isTransitioning = true;

        yield return new WaitForSeconds(levelLoadDelay);

        LevelLoader.LoadNextLevel();
    }

    private IEnumerator DelayReloadLevel()
    {
        isTransitioning = true;

        yield return new WaitForSeconds(levelLoadDelay);

        LevelLoader.ReloadCurrentLevel();
    }

    public void SetMainMenuStart()
    {
        mainMenuStart = true;
        GetComponentInChildren<ParticleSystem>().Stop();
    }

    public void SetWinScreenRestart()
    {
        winScreenRestart = true;

        GetComponentInChildren<ParticleSystem>().Play();
        myAudioSource.PlayOneShot(thrusterSound);
    }

    public int GetFuel()
    {
        int fuelToReturn = (int)rocketFuel;
        if (fuelToReturn <= 0)
        {
            fuelToReturn = 0;
        }

        return fuelToReturn;
    }
}