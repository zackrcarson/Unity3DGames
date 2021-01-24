using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    // Config Variables
    [Header("Miscellaneous Properties")]
    [SerializeField] bool isMainMenu = false;

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

    // Update is called once per frame
    void Update()
    {
        if (isMainMenu) { return; }

        ProcessShipTranslation();

        ProcessShipRotation();
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
}
