using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int enemyHealth = 100;

    // Cached references
    int numDeathAnimations = 2;
    Animator animator = null;

    // State Variables
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void DamageEnemy(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            KillEnemy();
        }

        BroadcastMessage("OnDamageTaken");
    }

    private void KillEnemy()
    {
        Die();
    }

    private void Die()
    {
        if (isDead) { return; }

        // TODO Enemy death SFX

        isDead = true;

        animator.SetInteger("isDead", GetRandomDeathAnimation());
    }

    private int GetRandomDeathAnimation()
    {
        return Random.Range(1, numDeathAnimations + 1);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
