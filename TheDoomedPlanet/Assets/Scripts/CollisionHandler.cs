using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Config Parameters
    [SerializeField] int playerHealth = 3;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] GameObject explosionVFX = null;

    private void Start()
    {
        explosionVFX.SetActive(false);
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        playerHealth--;

        if (playerHealth <= 0)
        {
            StartDeathSequence(otherCollider);
        }
    }

    private void StartDeathSequence(Collider otherCollider)
    {
        SendMessage("OnPlayerDeath");

        GetComponent<MeshRenderer>().enabled = false;

        explosionVFX.SetActive(true);

        StartCoroutine(LevelLoader.ReloadLevel(levelLoadDelay));
    }
}
