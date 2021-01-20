using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
    // Cached references
    Text fuelText = null;
    RocketShip rocketShip = null;

    // Start is called before the first frame update
    void Start()
    {
        fuelText = GetComponent<Text>();
        rocketShip = FindObjectOfType<RocketShip>();

        fuelText.text = rocketShip.GetFuel().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        fuelText.text = rocketShip.GetFuel().ToString();
    }
}
