﻿using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Config Parameters
    [SerializeField] bool isFlickering = true;
    [SerializeField] float flickerDelayMin = 0.1f;
    [SerializeField] float flickerDelayMax = 0.5f;

    // Cached References
    Light lightSource = null;
    MeshRenderer meshRenderer = null;
    AudioSource audioSource = null;

    // State variables
    bool lightOn = true;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponentInChildren<Light>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (isFlickering)
        {
            yield return new WaitForSeconds(RandomFlickerTime());

            SwitchLight();
        }
    }

    private void SwitchLight()
    {
        if (lightOn)
        {
            audioSource.Pause();
            lightSource.enabled = false;
            meshRenderer.materials[0].DisableKeyword("_EMISSION");

            lightOn = false;
        }
        else
        {
            audioSource.UnPause();
            lightSource.enabled = true;
            meshRenderer.materials[0].EnableKeyword("_EMISSION");

            lightOn = true;
        }
    }

    private float RandomFlickerTime()
    {
        return Random.Range(flickerDelayMin, flickerDelayMax);
    }
}
