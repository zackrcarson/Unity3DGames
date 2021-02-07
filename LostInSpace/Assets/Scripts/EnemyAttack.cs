using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int damage = 20;

    // Cached References
    PlayerHealth playerHealth = null;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        if (!playerHealth) { return; }

        playerHealth.DamagePlayer(damage);
    }
}
