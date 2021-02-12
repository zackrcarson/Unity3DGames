using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int healthAmount = 50;

    // Cached References
    PlayerHealth playerHealth = null;
    Animator animator = null;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHealth.IncreaseHealth(healthAmount);
            animator.SetTrigger("isOpened");

            GetComponent<SphereCollider>().enabled = false;

            // TODO: Play pickup sound
        }
    }
}
