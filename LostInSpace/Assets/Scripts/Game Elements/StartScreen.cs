using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StartScreen : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Canvas reticleCanves = null;
    [SerializeField] Canvas ammoDisplayCanvas = null;
    [SerializeField] Canvas healthDisplayCanvas = null;
    [SerializeField] FlashLight flashLight = null;
    [SerializeField] Animator playerAnimator = null;

    // Cached References
    Transform player = null;

    private void Start()
    {
        reticleCanves.enabled = false;

        player = FindObjectOfType<PlayerHealth>().gameObject.transform;
    }

    public void StartGame()
    {
        ammoDisplayCanvas.enabled = false;
        healthDisplayCanvas.enabled = false;

        playerAnimator.SetTrigger("playerStart");

        gameObject.SetActive(false);
    }

    public void TurnOnFlashLightAndGun()
    {
        flashLight.StartGame();
        reticleCanves.enabled = true;
        FindObjectOfType<WeaponSwitcher>().StartGame();

        ammoDisplayCanvas.enabled = true;
        healthDisplayCanvas.enabled = true;
    }

    public void TurnOnPlayerController()
    {
        FindObjectOfType<RigidbodyFirstPersonController>().StartGame();
        playerAnimator.enabled = false;
    }
}
