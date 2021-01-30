using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public void RestartGame()
    {
        Destroy(GameObject.Find("Music Player"));
        Time.timeScale = 1;
        LevelLoader.LoadNextLevel();
    }
}
