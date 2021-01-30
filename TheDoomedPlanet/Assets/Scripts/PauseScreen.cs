using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public void UnpauseGame()
    {
        FindObjectOfType<PlayerController>().UnpauseGame();
    }

    public void RestartGame()
    {
        Destroy(GameObject.Find("Music Player"));
        Time.timeScale = 1;
        LevelLoader.LoadNextLevel();
    }
}
