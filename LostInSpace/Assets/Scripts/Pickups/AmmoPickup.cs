using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] AmmoType ammoType = 0;
    [SerializeField] int ammoAmmount = 5;
    [SerializeField] float pickupAudioVolume = 1f;
    [SerializeField] AudioClip pickupAudio = null;

    // Cached References
    Ammo ammo = null;
    Animator animator = null;
    AudioSource audioSource = null;

    private void Start()
    {
        ammo = FindObjectOfType<Ammo>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.PlayOneShot(pickupAudio, pickupAudioVolume);

            ammo.IncreaseCurrentAmmo(ammoType, ammoAmmount);
            animator.SetTrigger("isOpened");

            GetComponent<SphereCollider>().enabled = false;

            // TODO: Play pickup sound
        }
    }
}
