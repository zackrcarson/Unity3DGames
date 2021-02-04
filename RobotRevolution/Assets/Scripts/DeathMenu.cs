using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void RestartGame()
    {
        GameObject.Destroy(GameObject.Find("Music Player"));
        GameObject.Destroy(GameObject.Find("Score Board"));

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentScene);
    }
}
