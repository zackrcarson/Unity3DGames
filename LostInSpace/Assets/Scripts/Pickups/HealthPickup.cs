using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int healthAmount = 50;
    [SerializeField] float pickupAudioVolume = 1f;
    [SerializeField] AudioClip pickupAudio = null;

    // Cached References
    PlayerHealth playerHealth = null;
    Animator animator = null;
    AudioSource audioSource = null;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.PlayOneShot(pickupAudio, pickupAudioVolume);

            playerHealth.IncreaseHealth(healthAmount);
            animator.SetTrigger("isOpened");

            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
