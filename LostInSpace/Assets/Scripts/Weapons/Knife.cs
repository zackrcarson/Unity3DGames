using UnityEngine;

public class Knife : MonoBehaviour
{
    // Config Params
    [SerializeField] int knifeDamage = 40;
    [SerializeField] GameObject enemyHitVFX = null;
    [SerializeField] float enemyHitDestroyDelay = 1f;
    [SerializeField] Transform VFXParent = null;
    [SerializeField] Transform hitVFXLocation = null;

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (!enemyHealth) { return; }

        enemyHealth.DamageEnemy(knifeDamage);

        GameObject impact = Instantiate(enemyHitVFX, hitVFXLocation.position, Quaternion.LookRotation(transform.position - enemyHealth.gameObject.transform.position));
        impact.transform.parent = VFXParent;

        Destroy(impact, enemyHitDestroyDelay);
    }
}
