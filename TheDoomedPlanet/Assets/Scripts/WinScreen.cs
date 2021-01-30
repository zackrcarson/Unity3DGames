using UnityEngine;

public class WinScreen : MonoBehaviour
{
    // State Variables
    int numFramesActive = 0;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy && numFramesActive == 0)
        {
            numFramesActive++;
        }

        if (numFramesActive == 1)
        {
            FindObjectOfType<MusicPlayer>().PlayWinMusic();
            FindObjectOfType<PlayerController>().PlayerWon();

            numFramesActive++;
            
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        Destroy(GameObject.Find("Music Player"));
        Time.timeScale = 1;
        LevelLoader.LoadNextLevel();
    }
}
