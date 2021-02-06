using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // config Parameters
    [SerializeField] GameObject pauseMenu = null;

    // State Variables
    bool isPaused = false;
    public bool canPause = false;

    // Cached References
    TowerSpawner towerSpawner = null;
    Waypoint[] allWaypoints = null;

    // Start is called before the first frame update
    void Start()
    {
        allWaypoints = FindObjectsOfType<Waypoint>();
        towerSpawner = FindObjectOfType<TowerSpawner>();

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    private void PauseGame()
    {
        foreach (Waypoint waypoint in allWaypoints)
        {
            waypoint.isSpawning = false;
        }
        towerSpawner.isSpawning = false;

        AudioListener.pause = true;

        Time.timeScale = 0;

        pauseMenu.SetActive(true);

        isPaused = true;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;

        foreach (Waypoint waypoint in allWaypoints)
        {
            waypoint.isSpawning = true;
        }
        towerSpawner.isSpawning = true;

        Time.timeScale = 1;

        pauseMenu.SetActive(false);

        isPaused = false;
    }

    public void RestartGame()
    {
        Destroy(GameObject.Find("Music Player"));
        Destroy(GameObject.Find("Score Board"));

        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
