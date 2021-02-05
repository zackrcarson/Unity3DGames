using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    // Config Params
    [SerializeField] ParticleSystem[] thrusters = null;
    [SerializeField] AudioClip takeoffClip = null;

    public void DisableThrusters()
    {
        foreach (ParticleSystem thruster in thrusters)
        {
            thruster.gameObject.SetActive(false);
        }
    }

    public void EnableThrusters()
    {
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.clip = takeoffClip;
        audioSource.Play();

        foreach (ParticleSystem thruster in thrusters)
        {
            thruster.gameObject.SetActive(true);
        }
    }
}
