using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StartScreen : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Canvas ControlsCanvas = null;
    [SerializeField] Canvas reticleCanves = null;
    [SerializeField] Canvas ammoDisplayCanvas = null;
    [SerializeField] Canvas healthDisplayCanvas = null;
    [SerializeField] FlashLight flashLight = null;
    [SerializeField] Animator playerAnimator = null;
    [SerializeField] GameObject knifeBody = null;

    // Cached References
    Transform player = null;
    Melee melee = null;
    WeaponSwitcher weaponSwitcher = null;
    WeaponZoom weaponZoom = null;
    Weapon weapon = null;

    private void Start()
    {
        reticleCanves.enabled = false;

        player = FindObjectOfType<PlayerHealth>().gameObject.transform;

        melee = FindObjectOfType<Melee>();
        weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
        weaponZoom = FindObjectOfType<WeaponZoom>();
        weapon = FindObjectOfType<Weapon>();

        ControlsCanvas.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        FindObjectOfType<MusicPlayer>().PlayPlayMusic();

        ammoDisplayCanvas.enabled = false;
        healthDisplayCanvas.enabled = false;

        playerAnimator.SetTrigger("playerStart");
        weaponSwitcher.StartGame();

        if (!weapon) { weapon = FindObjectOfType<Weapon>(); }
        weapon.DenyShooting();

        if (!weaponZoom) { weaponZoom = FindObjectOfType<WeaponZoom>(); }
        weaponZoom.DenyZooming();

        knifeBody.SetActive(true);

        gameObject.SetActive(false);
    }

    public void TurnOnFlashLightAndGun()
    {
        flashLight.StartGame();
        reticleCanves.enabled = true;

        ammoDisplayCanvas.enabled = true;
        healthDisplayCanvas.enabled = true;
    }

    public void TurnOnPlayerController()
    {
        if (!weaponZoom) { weaponZoom = FindObjectOfType<WeaponZoom>(); }
        if (!weapon) { weapon = FindObjectOfType<Weapon>(); }

        FindObjectOfType<RigidbodyFirstPersonController>().StartGame();
        melee.AllowStabbing();
        weaponSwitcher.AllowSwitching();
        weaponZoom.AllowZooming();
        weapon.AllowShooting();

        playerAnimator.enabled = false;
    }

    public void DisplayControls()
    {
        ControlsCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TurnOffControls()
    {
        ControlsCanvas.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
