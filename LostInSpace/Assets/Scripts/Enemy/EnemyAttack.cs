using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int damage = 20;

    // Cached References
    PlayerHealth playerHealth = null;
    DisplayDamage displayDamage = null;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        displayDamage = FindObjectOfType<DisplayDamage>();
    }

    public void AttackHitEvent()
    {
        if (!playerHealth) { return; }

        playerHealth.DamagePlayer(damage);

        displayDamage.ShowDamage();
    }
}
