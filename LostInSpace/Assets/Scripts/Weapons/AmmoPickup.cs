using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] AmmoType ammoType = 0;
    [SerializeField] int ammoAmmount = 5;

    // Cached References
    Ammo ammo = null;

    private void Start()
    {
        ammo = FindObjectOfType<Ammo>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ammo.IncreaseCurrentAmmo(ammoType, ammoAmmount);

            Destroy(gameObject);
            // TODO: Play pickup sound
        }
    }
}
