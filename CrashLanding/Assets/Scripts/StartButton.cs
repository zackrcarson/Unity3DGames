using System.Collections;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    // config Parameters
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float restartLoadDelay = 1f;

    public void StartGame()
    {
        FindObjectOfType<RocketShip>().SetMainMenuStart();

        StartCoroutine(DelayLoadNextLevel());
    }

    public void RestartGame()
    {
        FindObjectOfType<RocketShip>().SetWinScreenRestart();

        StartCoroutine(DelayStartOver());
    }

    private IEnumerator DelayLoadNextLevel()
    {
        yield return new WaitForSeconds(levelLoadDelay);

        LevelLoader.LoadNextLevel();
    }

    private IEnumerator DelayStartOver()
    {
        yield return new WaitForSeconds(restartLoadDelay);

        LevelLoader.LoadNextLevel();
    }
}
