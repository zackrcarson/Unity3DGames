using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Animator alienShipAnimator = null;
    [SerializeField] AnimationClip shipTakeoffAnimationForLength = null;

    // Cached References
    Animator animator = null;
    MusicPlayer musicPlayer = null;
    TowerSpawner towerSpawner = null;
    Waypoint[] allWaypoints = null;

    // State Variables
    bool buttonClickable = false;

    private void Start()
    {
        allWaypoints = FindObjectsOfType<Waypoint>();
        towerSpawner = FindObjectOfType<TowerSpawner>();
        animator = GetComponent<Animator>();
        musicPlayer = FindObjectOfType<MusicPlayer>();
    }

    public void WonGame()
    {
        animator.SetTrigger("isWon");
        musicPlayer.PlayWinMusic();

        foreach (Waypoint waypoint in allWaypoints)
        {
            waypoint.isSpawning = false;
        }
        towerSpawner.isSpawning = false;
    }

    public void NextLevel()
    {
        if (buttonClickable)
        {
            StartCoroutine(AlienShipTakeoffAndLoadNextLevel());
        }
    }

    private IEnumerator AlienShipTakeoffAndLoadNextLevel()
    {
        alienShipAnimator.SetTrigger("levelEnded");
        yield return new WaitForSeconds(shipTakeoffAnimationForLength.length);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalNumberOfScenes = SceneManager.sceneCountInBuildSettings - 1;

        if (currentScene == totalNumberOfScenes)
        {
            Time.timeScale = 1;

            Destroy(GameObject.Find("Music Player"));
            Destroy(GameObject.Find("Score Board"));

            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
            FindObjectOfType<MusicPlayer>().PlayPlayMusic();
        }
    }

    public void CanClickButton()
    {
        buttonClickable = true;
    }
}
