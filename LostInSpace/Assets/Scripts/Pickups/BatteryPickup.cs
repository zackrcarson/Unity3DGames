using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float timerAddition = 5f;

    // Cached References
    FlashLight flashLight = null;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = FindObjectOfType<FlashLight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            flashLight.AddToDecayTimer(timerAddition);

            GetComponentInChildren<MeshRenderer>().materials[0].DisableKeyword("_EMISSION");

            GetComponent<SphereCollider>().enabled = false;

            // TODO: Play pickup sound
        }
    }
}
