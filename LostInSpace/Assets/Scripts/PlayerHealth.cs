using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int playerHealth = 200;
    [SerializeField] float reloadLevelDelayTime = 4f;

    // Cached References
    Rigidbody rigidBody = null;
    Animator animator = null;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void DamagePlayer(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        animator.enabled = true;

        animator.SetTrigger("playerDead");

        StartCoroutine(SceneLoader.ReloadCurrentScene(reloadLevelDelayTime));
    }

    public void TurnOffGravity()
    {
        rigidBody.useGravity = false;
    }
}
