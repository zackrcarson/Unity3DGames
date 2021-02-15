using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int enemyHealth = 100;

    // Cached references
    int numDeathAnimations = 2;
    Animator animator = null;
    EnemyAudio enemyAudio = null;

    // State Variables
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyAudio = GetComponent<EnemyAudio>();
        animator = GetComponentInChildren<Animator>();
    }

    public void DamageEnemy(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            KillEnemy();
        }
        else
        {
            enemyAudio.PlayHurtSound();
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

        enemyAudio.PlayDeathSound();

        GetComponent<Collider>().enabled = false;

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
