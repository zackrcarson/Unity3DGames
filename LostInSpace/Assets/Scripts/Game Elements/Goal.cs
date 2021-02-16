using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Config Parameters
    [Header("Misc.")]
    [SerializeField] Canvas winScreen = null;
    [SerializeField] Animator doorAnimator = null;

    [Header("Audio")]
    [SerializeField] float computerAudioVolume = 1f;
    [SerializeField] float typingAudioVolume = 1f;
    [SerializeField] float doorCloseAudioVolume = 1f;
    [SerializeField] float doorOpenAudioVolume = 1f;
    [SerializeField] float alarmAudioVolume = 1f;
    [SerializeField] float vacuumAudioVolume = 1f;
    [SerializeField] float screechAudioVolume = .3f;
    [SerializeField] AudioClip computerAudio = null;
    [SerializeField] AudioClip typingAudio = null;
    [SerializeField] AudioClip doorCloseAudio = null;
    [SerializeField] AudioClip doorOpenAudio = null;
    [SerializeField] AudioClip alarmAudio = null;
    [SerializeField] AudioClip vacuumAudio = null;
    [SerializeField] AudioClip screechAudio = null;

    [Header("Timing Delays")]
    [SerializeField] float typingToComputerDelay = 1f;
    [SerializeField] float computerToLightsOnDelay = 1f;
    [SerializeField] float lightsOnToDoorCloseDelay = 1f;
    [SerializeField] float doorCloseToAlarmDelay = 1f;
    [SerializeField] float alarmToVacuumDelay = 1f;
    [SerializeField] float vacuumToScreechDelay = 1f;
    [SerializeField] float screechToDoorOpenDelay = 1f;
    [SerializeField] float doorOpenToWinScreenDelay = 1f;

    // Cached References
    GoalLight[] goalLights = null;
    AudioSource audioSource = null;

    // State variables
    bool hasWon = false;
    bool winScreenOn = false;

    // Start is called before the first frame update
    void Start()
    {
        winScreen.enabled = false;

        audioSource = GetComponent<AudioSource>();
        goalLights = FindObjectsOfType<GoalLight>();

        TurnOffAllLights();
    }

    private void Update()
    {
        if (!winScreenOn) { return; }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void TurnOffAllLights()
    {
        foreach (GoalLight goalLight in goalLights)
        {
            goalLight.LightOff();
        }
    }

    private void TurnOnAllLights()
    {
        foreach (GoalLight goalLight in goalLights)
        {
            goalLight.LightOn();
        }

        FindObjectOfType<FlashLight>().GameWon();
    }

    private void ClearAllEnemies()
    {
        EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();

        foreach (EnemyHealth enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasWon)
        {
            StartCoroutine(WinSequence());
        }
    }

    private IEnumerator WinSequence()
    {
        hasWon = true;

        audioSource.PlayOneShot(typingAudio, typingAudioVolume);

        yield return new WaitForSeconds(typingToComputerDelay);

        audioSource.PlayOneShot(computerAudio, computerAudioVolume);

        yield return new WaitForSeconds(computerToLightsOnDelay);

        TurnOnAllLights();

        yield return new WaitForSeconds(lightsOnToDoorCloseDelay);

        audioSource.PlayOneShot(doorCloseAudio, doorCloseAudioVolume);
        doorAnimator.SetTrigger("doorClose");

        yield return new WaitForSeconds(doorCloseToAlarmDelay);

        audioSource.PlayOneShot(alarmAudio, alarmAudioVolume);
        ClearAllEnemies();

        yield return new WaitForSeconds(alarmToVacuumDelay);

        audioSource.PlayOneShot(vacuumAudio, vacuumAudioVolume);

        yield return new WaitForSeconds(vacuumToScreechDelay);

        audioSource.PlayOneShot(screechAudio, screechAudioVolume);

        yield return new WaitForSeconds(screechToDoorOpenDelay);

        audioSource.PlayOneShot(doorOpenAudio, doorOpenAudioVolume);
        doorAnimator.SetTrigger("doorOpen");

        yield return new WaitForSeconds(doorOpenToWinScreenDelay);

        FindObjectOfType<PauseScreen>().DisablePauseAndUI();

        winScreenOn = true;
        winScreen.enabled = true;

        FindObjectOfType<MusicPlayer>().PlayWinMusic();
    }
}
