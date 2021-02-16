using System;
using System.Collections;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float lightDecay = 0.1f;
    [SerializeField] float angleDecay = 1f;
    [SerializeField] float minAngle = 40f;
    [SerializeField] float initialTimeBeforeDecay = 5f;
    [SerializeField] float minFlickerTime = .1f;
    [SerializeField] float maxFlickerTime = 1f;
    [SerializeField] float flickerDarkTime = 0.2f;
    [SerializeField] float flashlightClickAudioVolume = 1f;
    [SerializeField] AudioClip flashlightClickAudio = null;

    // Cached References
    Light flashLight = null;
    AudioSource audioSource = null;
    float initialAngle = 0f;
    float initialIntensity = 0f;

    // State Variables
    bool gameStarted = false;
    float timeUntilDecay = 5f;
    bool isFlickering = false;
    bool gameWon = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void StartGame()
    {
        gameObject.SetActive(true);
        FlashlightClick();

        audioSource = GetComponent<AudioSource>();
        flashLight = GetComponent<Light>();
        initialAngle = flashLight.spotAngle;
        initialIntensity = flashLight.intensity;

        timeUntilDecay = initialTimeBeforeDecay;

        gameStarted = true;
    }

    private void Update()
    {
        if (!gameStarted) { return; }

        if (!gameWon) { timeUntilDecay -= Time.deltaTime; }

        if (timeUntilDecay <= 0) { timeUntilDecay = 0;}

        if (timeUntilDecay == 0)
        {
            DecreaseLightIntensity();

            DecreaseLightAngle();

            StartCoroutine(FlickerLight());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private void DecreaseLightAngle()
    {
        if (flashLight.spotAngle <= minAngle)
        {
            return;
        }
        else
        {
            flashLight.spotAngle -= Time.deltaTime * angleDecay;
        }
    }

    private void DecreaseLightIntensity()
    {
        flashLight.intensity -= Time.deltaTime * lightDecay;
    }

    private IEnumerator FlickerLight()
    {
        if (!isFlickering)
        {
            isFlickering = true;
            while (timeUntilDecay == 0 && flashLight.intensity > 0)
            {
                yield return new WaitForSeconds(RandomFlickerInterval());
                
                float currentIntensity = flashLight.intensity;
                flashLight.intensity = 0f;

                yield return new WaitForSeconds(flickerDarkTime);

                if (currentIntensity > 0.2f) { FlashlightClick(); }
                flashLight.intensity = currentIntensity;
            }
            isFlickering = false;
        }
    }

    private float RandomFlickerInterval()
    {
        return UnityEngine.Random.Range(minFlickerTime, maxFlickerTime);
    }

    public void AddToDecayTimer(float timeToAdd)
    {
        if (timeUntilDecay <= 0)
        {
            FlashlightClick();
        }

        timeUntilDecay += timeToAdd;

        flashLight.spotAngle = initialAngle;
        flashLight.intensity = initialIntensity;
    }

    private void FlashlightClick()
    {
        if (!audioSource) { audioSource = GetComponent<AudioSource>(); }

        audioSource.PlayOneShot(flashlightClickAudio, flashlightClickAudioVolume);
    }

    public void GameWon()
    {
        AddToDecayTimer(9999f);

        gameWon = true;
    }
}
