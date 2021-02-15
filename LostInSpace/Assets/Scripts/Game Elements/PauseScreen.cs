using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseScreen : MonoBehaviour
{
    // Cached References
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] Text pauseInfoDisplay = null;
    [SerializeField] Canvas reticleCanvas = null;

    // State Variables
    bool isPaused = false;
    public bool canPause = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    PauseGame();
                }
            }
        }
    }

    private void PauseGame()
    {
        FindObjectOfType<Weapon>().IsPaused(true);

        reticleCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Weapon weapon = FindObjectOfType<Weapon>();
        if (weapon) { weapon.DenyShooting(); }

        Melee melee = FindObjectOfType<Melee>();
        if (melee) { melee.DisallowStabbing(); }

        WeaponZoom weaponZoom = FindObjectOfType<WeaponZoom>();
        if (weaponZoom) { weaponZoom.DenyZooming(); }

        FindObjectOfType<RigidbodyFirstPersonController>().DenyMovement();
        FindObjectOfType<WeaponSwitcher>().DenySwitching();

        AudioListener.pause = true;

        Time.timeScale = 0;

        pauseMenu.SetActive(true);
        pauseInfoDisplay.enabled = false;

        isPaused = true;
    }

    public void ResumeGame()
    {
        Weapon weapon = FindObjectOfType<Weapon>();
        if (weapon) { weapon.AllowShooting(); }

        Melee melee = FindObjectOfType<Melee>();
        if (melee) { melee.AllowStabbing(); }

        WeaponZoom weaponZoom = FindObjectOfType<WeaponZoom>();
        if (weaponZoom) { weaponZoom.AllowZooming(); }

        FindObjectOfType<RigidbodyFirstPersonController>().AllowMovement();
        FindObjectOfType<WeaponSwitcher>().AllowSwitching();

        AudioListener.pause = false;

        Time.timeScale = 1;

        pauseMenu.SetActive(false);
        pauseInfoDisplay.enabled = true;

        isPaused = false;

        FindObjectOfType<Weapon>().IsPaused(false);

        reticleCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        StartCoroutine(SceneLoader.ReloadCurrentScene(0f));
    }

    public void DenyPause()
    {
        canPause = false;
    }
}
