using UnityEngine;

public class WeaponSwapAnimator : MonoBehaviour
{
    // Cached References
    WeaponSwitcher weaponSwitcher = null;

    private void Start()
    {
        weaponSwitcher = GetComponentInChildren<WeaponSwitcher>();
    }

    public void SwapWeapons()
    {
        weaponSwitcher.SetWeaponActive();
    }
}
