using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int finalSceneIndex = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex == finalSceneIndex - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public static void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
