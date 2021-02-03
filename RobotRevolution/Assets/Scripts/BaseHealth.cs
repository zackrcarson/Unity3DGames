using UnityEngine;
using System.Collections;

public class BaseHealth : MonoBehaviour
{
    // config Parameters
    [SerializeField] int baseHealth = 3;
    [SerializeField] float baseDestroyWaitTime = 2f;
    [SerializeField] GameObject[] bombs = null;
    [SerializeField] AnimationClip sinkAnimationClipForLength = null;

    // Cached References
    Animator animator = null;
    ScoreBoard scoreBoard = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        scoreBoard = FindObjectOfType<ScoreBoard>();

        if (scoreBoard) { scoreBoard.UpdateHealth(baseHealth); }
    }

    private IEnumerator DestroyBase()
    {
        FindObjectOfType<MusicPlayer>().PlayDeathMusic();

        yield return new WaitForSeconds(baseDestroyWaitTime);

        FindObjectOfType<EnemySpawner>().isSpawning = false;
        FindObjectOfType<TowerSpawner>().isSpawning = false;
        EnemyDamage[] enemies = FindObjectsOfType<EnemyDamage>();

        foreach (EnemyDamage enemy in enemies)
        {
            if (!enemy.gameObject.GetComponent<EnemyMovement>().isDead)
            {
                StartCoroutine(enemy.BeginFailedSequence());
            }
        }

        animator.SetTrigger("isSinking");

        int numBombs = bombs.Length;
        float animationTime = sinkAnimationClipForLength.length;
        foreach (GameObject bomb in bombs)
        {
            yield return new WaitForSeconds(animationTime / numBombs);
            if (bomb)
            {
                bomb.SetActive(true);
            }
        }
    }

    public void ReduceHealth(int amount)
    {
        baseHealth--;

        if (!scoreBoard) { scoreBoard = FindObjectOfType<ScoreBoard>(); }

        scoreBoard.UpdateHealth(baseHealth);

        if (baseHealth <= 0)
        {
            StartCoroutine(DestroyBase());
        }
    }

    public int GetHealth()
    {
        return baseHealth;
    }
}
