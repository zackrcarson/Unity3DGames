using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Config Params
    [SerializeField] Camera firstPersonCamera = null;
    [SerializeField] float range = 100f;
    [SerializeField] int damage = 10;

    [Header("Gun VFX")]
    [SerializeField] Transform VFXParent = null;
    [SerializeField] ParticleSystem muzzleFlash = null;
    [SerializeField] GameObject metalHitVFX = null;
    [SerializeField] float metalHitDestroyDelay = 30f;
    [SerializeField] GameObject enemyHitVFX = null;
    [SerializeField] float enemyHitDestroyDelay = 1f;

    void Update()
    {
       if (Input.GetButtonDown("Fire1"))
       {
            Shoot();
       }
    }

    private void Shoot()
    {
        PlayMuzzleFlash();
        // TODO:muzzle sound, gun kickback

        ProcessRaycast();
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
            Debug.Log("I hit " + hit.transform.name);
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
        GameObject impact = Instantiate(metalHitVFX, hit.point, Quaternion.LookRotation(hit.normal));
        impact.transform.parent = VFXParent;

        Destroy(impact, metalHitDestroyDelay);
    }

    private void EnemyHitImpactVFX(RaycastHit hit)
    {
        GameObject impact = Instantiate(enemyHitVFX, hit.point, Quaternion.LookRotation(hit.normal));
        impact.transform.parent = VFXParent;

        Destroy(impact, enemyHitDestroyDelay);
    }
}
