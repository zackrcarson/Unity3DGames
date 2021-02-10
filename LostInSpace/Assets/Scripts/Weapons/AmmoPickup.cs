using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] AmmoType ammoType = 0;
    [SerializeField] int ammoAmmount = 5;

    // Cached References
    Ammo ammo = null;
    Animator animator = null;

    private void Start()
    {
        ammo = FindObjectOfType<Ammo>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ammo.IncreaseCurrentAmmo(ammoType, ammoAmmount);
            animator.SetTrigger("isOpened");
            //Destroy(gameObject);
            // TODO: Play pickup sound
        }
    }
}
