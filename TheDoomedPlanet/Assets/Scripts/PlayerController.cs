using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Config Variables
    [Header("Miscellaneous Properties")]
    [SerializeField] bool isMainMenu = false;
    [SerializeField] GameObject[] guns = null;

    [Header("Ship Translation Properties")]
    [Tooltip("In ms^-1")] [SerializeField] float xSpeed = 20f;
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 20f;
    [Tooltip("In m")] [SerializeField] float xRange = 3.2f;
    [Tooltip("In m")] [SerializeField] float yRange = 2.6f;

    [Header("Ship Rotation Properties")]
    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float controlPitchFactor = -20f;

    [SerializeField] float positionYawFactor = 5f;

    [SerializeField] float controlRollFactor = -20f;

    // State Variables
    float xThrow, yThrow;
    bool isDead = false;

    private void Start()
    {
        if (isMainMenu)
        {
            ParticleSystem[] lasers = GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem laser in lasers)
            {
                laser.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMainMenu || isDead) { return; }

        ProcessShipTranslation();

        ProcessShipRotation();

        ProcessFiring();
    }

    private void ProcessShipTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * xSpeed;
        float yOffset = yThrow * Time.deltaTime * ySpeed;

        float xPos = Mathf.Clamp(transform.localPosition.x + xOffset, -xRange, xRange);
        float yPos = Mathf.Clamp(transform.localPosition.y + yOffset, -yRange, yRange);
        float zPos = transform.localPosition.z;

        Vector3 newPosition = new Vector3(xPos, yPos, zPos);

        transform.localPosition = newPosition;
    }

    private void ProcessShipRotation()
    {
        float pitch = transform.localPosition.y * positionPitchFactor + yThrow * controlPitchFactor;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll =  xThrow * controlRollFactor;

        // (x, y, z) = (pitch, yaw, roll) in degrees. Applies it in the order z : x : y (or roll : pitch : yaw).
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll); 
    }

    private void ProcessFiring()
    {
        if (Input.GetButton("Fire"))
        {
            ActivateGuns();
        }
        else
        {
            DeactivateGuns();
        }
    }
    
    private void ActivateGuns()
    {
        foreach(GameObject gun in guns)
        {
            gun.SetActive(true);
        }
    }

    private void DeactivateGuns()
    {
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
    }

    // Called by a string reference when player dies.
    public void OnPlayerDeath()
    {
        isDead = true;
    }
}
