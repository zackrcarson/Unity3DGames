using UnityEngine;
using System.Collections;

public class BaseHealth : MonoBehaviour
{
    // config Parameters
    [SerializeField] int baseHealth = 3;
    [SerializeField] float baseDestroyWaitTime = 2f;
    [SerializeField] GameObject[] bombs = null;
    [SerializeField] GameObject[] fires = null;
    [SerializeField] AnimationClip sinkAnimationClipForLength = null;
    [SerializeField] DeathMenu deathMenu = null;
    [SerializeField] float fireDestroyDelay = 0.2f;
    [SerializeField] float deathScreenLoadDelay = 3f;

    // Cached References
    Animator animator = null;
    ScoreBoard scoreBoard = null;
    PauseMenu pauseMenu = null;

    // State variables
    int currentFire = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        scoreBoard = FindObjectOfType<ScoreBoard>();

        if (scoreBoard) { scoreBoard.UpdateHealth(baseHealth); }

        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private IEnumerator DestroyBase()
    {
        FindObjectOfType<TowerSpawner>().isSpawning = false;
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in allWaypoints)
        {
            waypoint.isSpawning = false;
        }

        FindObjectOfType<MusicPlayer>().PlayDeathMusic();

        yield return new WaitForSeconds(baseDestroyWaitTime);

        FindObjectOfType<EnemySpawner>().isSpawning = false;
        FindObjectOfType<TowerSpawner>().isSpawning = false;
        EnemyDamage[] enemies = FindObjectsOfType<EnemyDamage>();

        foreach (EnemyDamage enemy in enemies)
        {
            if (!enemy.gameObject.GetComponent<EnemyMovement>().isDead || enemy.gameObject.GetComponent<EnemyMovement>().isGettingOffShip)
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

        foreach (GameObject fire in fires)
        {
            fire.SetActive(false);
            yield return new WaitForSeconds(fireDestroyDelay);
        }

        yield return new WaitForSeconds(deathScreenLoadDelay);

        pauseMenu.canPause = false;
        deathMenu.gameObject.SetActive(true);
    }

    public void ReduceHealth(int amount)
    {
        baseHealth--;
        
        fires[currentFire].SetActive(true);
        currentFire++;
        if (currentFire >= fires.Length - 1)
        {
            currentFire = fires.Length - 1;
        }

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
