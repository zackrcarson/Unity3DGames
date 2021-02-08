using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static IEnumerator ReloadCurrentScene(float delayTime)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(currentSceneIndex);
    }
}
