using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLights : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Light spotLight = null;

    public void TurnOffSpotLight()
    {
        spotLight.enabled = false;
    }
}
