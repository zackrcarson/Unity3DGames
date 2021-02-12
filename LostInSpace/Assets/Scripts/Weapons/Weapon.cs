using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    // Config Params
    [Header("Ammo")]
    [SerializeField] Ammo ammoSlot = null;
    [SerializeField] AmmoType ammoType = AmmoType.pistol;

    [Header("Gun Parameters")]
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 10;
    [SerializeField] float shootDelay = 1f;

    [Header("Miscellaneous")]
    [SerializeField] Camera firstPersonCamera = null;
    [SerializeField] Text ammoDisplay = null;
    [SerializeField] Color ammoDisplayColor = Color.white;

    [Header("Gun VFX")]
    [SerializeField] Transform VFXParent = null;
    [SerializeField] ParticleSystem muzzleFlash = null;
    [SerializeField] GameObject metalHitVFX = null;
    [SerializeField] float metalHitDestroyDelay = 30f;
    [SerializeField] GameObject enemyHitVFX = null;
    [SerializeField] float enemyHitDestroyDelay = 1f;

    // State Variables
    public bool isDead = false;
    bool canShoot = true;

    private void OnEnable()
    {
        canShoot = true;
    }

    void Update()
    {
        DisplayAmmo();

        if (isDead) { return; }

        if (Input.GetButtonDown("Fire1"))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetAmmo(ammoType);

        ammoDisplay.color = ammoDisplayColor;
        ammoDisplay.text = currentAmmo.ToString();
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        if (ammoSlot.GetAmmo(ammoType) > 0)
        {
            ammoSlot.ReduceCurrentAmmo(ammoType);
            PlayMuzzleFlash();
            ProcessRaycast();

            // TODO: gun kickback animation
            // TODO: gun firing sound
        }
        else
        {
            yield return null;
            // TODO: Add empty magazine click sound, maybe small kickback
        }

        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out hit, range))
        {
            // TODO: Also make sound effects on fire and hit
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (!target)
            {
                MetalHitImpactVFX(hit);
            }
            else
            {
                EnemyHitImpactVFX(hit);

                target.DamageEnemy(damage);
            }
        }
        else
        {
            return;
        }
    }

    private void MetalHitImpactVFX(RaycastHit hit)
    {
        if (hit.transform.tag != "Pickup")
        {
            GameObject impact = Instantiate(metalHitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.parent = VFXParent;

            Destroy(impact, metalHitDestroyDelay);
        }
    }

    private void EnemyHitImpactVFX(RaycastHit hit)
    {
        GameObject impact = Instantiate(enemyHitVFX, hit.point, Quaternion.LookRotation(hit.normal));
        impact.transform.parent = VFXParent;

        Destroy(impact, enemyHitDestroyDelay);
    }

    public void DenyShooting()
    {
        canShoot = false;
    }

    public void AllowShooting()
    {
        canShoot = true;
    }
}
