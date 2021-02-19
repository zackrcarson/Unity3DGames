using System.Collections;
using UnityEngine;

public class prop_LightFlicker : MonoBehaviour
{
    // Config Parameters
    [SerializeField] bool isFlickering = true;
    [SerializeField] float flickerDelayMin = 0.1f;
    [SerializeField] float flickerDelayMax = 0.5f;
    [SerializeField] MeshRenderer meshRenderer = null;
    [SerializeField] float clickSoundVolume = 0.3f;
    [SerializeField] AudioClip clickSound = null;

    // Cached References
    Light lightSource = null;
    AudioSource audioSource = null;

    // State variables
    bool lightOn = true;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponentInChildren<Light>();
        meshRenderer.materials[0].EnableKeyword("_EMISSION");
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
            audioSource.PlayOneShot(clickSound, clickSoundVolume);
            lightSource.enabled = false;
            meshRenderer.materials[0].DisableKeyword("_EMISSION");
            lightOn = false;
        }
        else
        {
            audioSource.PlayOneShot(clickSound, clickSoundVolume);
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
