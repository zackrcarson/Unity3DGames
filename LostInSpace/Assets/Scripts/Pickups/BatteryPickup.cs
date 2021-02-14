using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float timerAddition = 5f;
    [SerializeField] float pickupAudioVolume = 1f;
    [SerializeField] AudioClip pickupAudio = null;

    // Cached References
    FlashLight flashLight = null;
    AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = FindObjectOfType<FlashLight>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.PlayOneShot(pickupAudio, pickupAudioVolume);

            if (!flashLight) { flashLight = FindObjectOfType<FlashLight>(); }

            flashLight.AddToDecayTimer(timerAddition);

            GetComponentInChildren<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");

            GetComponent<SphereCollider>().enabled = false;

            // TODO: Play pickup sound
        }
    }
}
