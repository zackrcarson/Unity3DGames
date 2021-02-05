using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // config Parameters
    [SerializeField] GameObject pauseMenu = null;
    //[SerializeField] float musicVolumeDimRatio = 0.5f;

    // State Variables
    bool isPaused = false;
    public bool canPause = false;
    //float baseMusicVolume = 1f;

    // Cached References
    TowerSpawner towerSpawner = null;
    Waypoint[] allWaypoints = null;
    //MusicPlayer musicPlayer = null;
    //Tower[] allTowers = null;
    //Tower[] towersPlayingWhenPaused = null;

    // Start is called before the first frame update
    void Start()
    {
        allWaypoints = FindObjectsOfType<Waypoint>();
        towerSpawner = FindObjectOfType<TowerSpawner>();

        //allTowers = FindObjectsOfType<Tower>();
        //musicPlayer = FindObjectOfType<MusicPlayer>();

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
        //if (!musicPlayer) { musicPlayer = FindObjectOfType<MusicPlayer>(); }

        //baseMusicVolume = musicPlayer.GetVolume();
        //musicPlayer.ChangeVolume(baseMusicVolume * musicVolumeDimRatio);

        foreach (Waypoint waypoint in allWaypoints)
        {
            waypoint.isSpawning = false;
        }
        towerSpawner.isSpawning = false;

        //foreach (Tower tower in allTowers)
        //{
        //    AudioSource towerAudio = tower.gameObject.GetComponentInChildren<AudioSource>();

        //    if (towerAudio.isPlaying)
        //    {
        //        towerAudio.Stop();

        //        //towersPlayingWhenPaused[]
        //    }
        //}

        AudioListener.pause = true;

        Time.timeScale = 0;

        pauseMenu.SetActive(true);

        isPaused = true;
    }

    public void ResumeGame()
    {
        AudioListener.pause = false;

        //musicPlayer.ChangeVolume(baseMusicVolume);

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
