using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int currentWeapon = 0;

    // State parameters
    bool weaponsDisabled = false;
    bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //SetWeaponActive();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) { return; }
        if (weaponsDisabled) { return; }

        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeapon)
        {
            SetWeaponActive();
        }
    }

    private void ProcessKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentWeapon = 3;
        }
    }

    private void ProcessScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }
            else
            {
                currentWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon == 0)
            {
                currentWeapon = transform.childCount - 1;
            }
            else
            {
                currentWeapon--;
            }
        }
    }

    private void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    public void SetAllWeaponInactive()
    {
        weaponsDisabled = true;

        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }

    public void ResetWeaponsActive()
    {
        weaponsDisabled = false;

        SetWeaponActive();
    }

    public void StartGame()
    {
        gameStarted = true;

        ResetWeaponsActive();
    }
}
