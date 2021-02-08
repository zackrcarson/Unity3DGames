using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float zoomedInFieldOfView = 20f;
    [SerializeField] float zoomedInMouseSensitivity = 0.5f;

    // State Variables
    bool isZoomed = false;

    // Cached References
    float initialFieldOfView = 0f;
    float initialMouseSensitivity = 0f;
    Camera playerCamera = null;
    RigidbodyFirstPersonController fpsController;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponentInParent<Camera>();
        fpsController = GetComponentInParent<RigidbodyFirstPersonController>();

        initialFieldOfView = playerCamera.fieldOfView;
        initialMouseSensitivity = fpsController.mouseLook.XSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isZoomed = !isZoomed;
            Zoom();
        }
    }

    private void OnDisable()
    {
        isZoomed = false;
        Zoom();
    }

    private void Zoom()
    {
        if (isZoomed)
        {
            playerCamera.fieldOfView = zoomedInFieldOfView;

            fpsController.mouseLook.XSensitivity = zoomedInMouseSensitivity;
            fpsController.mouseLook.YSensitivity = zoomedInMouseSensitivity;
        }
        else
        {
            playerCamera.fieldOfView = initialFieldOfView;

            fpsController.mouseLook.XSensitivity = initialMouseSensitivity;
            fpsController.mouseLook.YSensitivity = initialMouseSensitivity;
        }
    }
}
