using UnityEngine;

public class GoalLight : MonoBehaviour
{
    // Cached References
    Light lightSource = null;
    MeshRenderer meshRenderer = null;
    AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponentInChildren<Light>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void LightOff()
    {
        GetComponent<AudioSource>().Pause();
        GetComponentInChildren<Light>().enabled = false;
        GetComponentInChildren<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");
    }

    public void LightOn()
    {
        audioSource.UnPause();
        lightSource.enabled = true;
        meshRenderer.materials[0].EnableKeyword("_EMISSION");
    }
}
