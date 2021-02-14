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

    [Header("Gun Audio")]
    [SerializeField] float errorToneVolume = 1f;
    [SerializeField] float shotAudioVolume = 0.5f;
    [SerializeField] float emptyAudioVolume = 0.5f;
    [SerializeField] AudioClip errorTone = null;
    [SerializeField] AudioClip shotAudio = null;
    [SerializeField] AudioClip emptyAudio = null;

    [SerializeField] float bulletHitMetalVolume = 1f;
    [SerializeField] float bulletHitBodyVolume = 1f;
    [SerializeField] AudioClip bulletHitMetal = null;
    [SerializeField] AudioClip bulletHitBody = null;

    // Cached References
    AudioSource audioSource = null;

    // State Variables
    public bool isDead = false;
    bool canShoot = true;
    bool isPaused = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        canShoot = true;
    }

    void Update()
    {
        DisplayAmmo();

        if (isDead) { return; }

        if (Input.GetButtonDown("Fire1") && !isPaused)
        {
            int ammoLeft = ammoSlot.GetAmmo(ammoType);
            if (canShoot && ammoLeft > 0)
            {
                StartCoroutine(Shoot());
            }
            else if (!canShoot && ammoLeft > 0)
            {
                PlayErrorTone();
            }
            
            if (ammoLeft <= 0)
            {
                PlayEmptyAudio();
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

        ammoSlot.ReduceCurrentAmmo(ammoType);
        PlayMuzzleFlash();
        ProcessRaycast();

        PlayFiringAudio();

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

    private void PlayFiringAudio()
    {
        audioSource.PlayOneShot(shotAudio, shotAudioVolume);
    }

    private void PlayEmptyAudio()
    {
        audioSource.PlayOneShot(emptyAudio, emptyAudioVolume);
    }

    private void PlayErrorTone()
    {
        audioSource.PlayOneShot(errorTone, errorToneVolume);
    }

    private void MetalHitImpactVFX(RaycastHit hit)
    {
        if (hit.transform.tag != "Pickup")
        {
            audioSource.PlayOneShot(bulletHitMetal, bulletHitMetalVolume);

            GameObject impact = Instantiate(metalHitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.parent = VFXParent;

            Destroy(impact, metalHitDestroyDelay);
        }
    }

    private void EnemyHitImpactVFX(RaycastHit hit)
    {
        audioSource.PlayOneShot(bulletHitBody, bulletHitBodyVolume);

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


    public void IsPaused(bool paused)
    {
        isPaused = paused;
    }
}
