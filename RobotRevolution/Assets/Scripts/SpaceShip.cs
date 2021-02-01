using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    // Config Params
    [SerializeField] ParticleSystem[] thrusters = null;

    public void DisableThrusters()
    {
        foreach (ParticleSystem thruster in thrusters)
        {
            thruster.gameObject.SetActive(false);
        }
    }
}
