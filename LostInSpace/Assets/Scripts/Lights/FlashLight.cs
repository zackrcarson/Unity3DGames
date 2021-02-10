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

    // Cached References
    Light flashLight = null;
    float initialAngle = 0f;
    float initialIntensity = 0f;

    // State Variables
    public float timeUntilDecay = 5f;
    public bool isFlickering = false;

    private void Start()
    {
        flashLight = GetComponent<Light>();
        initialAngle = flashLight.spotAngle;
        initialIntensity = flashLight.intensity;

        timeUntilDecay = initialTimeBeforeDecay;
    }

    private void Update()
    {
        timeUntilDecay -= Time.deltaTime;
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
            while (timeUntilDecay == 0)
            {
                yield return new WaitForSeconds(RandomFlickerInterval());

                float currentIntensity = flashLight.intensity;
                flashLight.intensity = 0f;

                yield return new WaitForSeconds(flickerDarkTime);

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
        timeUntilDecay += timeToAdd;

        flashLight.spotAngle = initialAngle;
        flashLight.intensity = initialIntensity;
    }
}
