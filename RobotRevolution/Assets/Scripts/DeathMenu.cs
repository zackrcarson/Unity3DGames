using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1;
        Destroy(GameObject.Find("Music Player"));
        Destroy(GameObject.Find("Score Board"));

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }
}
