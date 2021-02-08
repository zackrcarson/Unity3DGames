using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float bloodDripDelay = 2f;
    [SerializeField] float reloadLevelDelay = 0.5f;

    // Cached References
    Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(bloodDripDelay);

        GetComponent<Canvas>().enabled = true;

        if (!animator) { animator = GetComponent<Animator>(); }
        animator.SetTrigger("playerDead");
    }

    public void RestartGame()
    {
        StartCoroutine(SceneLoader.ReloadCurrentScene(reloadLevelDelay));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}