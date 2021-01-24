using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // Config Parameters
    [SerializeField] float levelLoadDelay = 30f;
    [SerializeField] float playerShipMovementResolution = 0.0001f;
    [SerializeField] float playerShipSpeed = 1f;

    // Cached References
    PlayerController playerShip = null;

    private void Start()
    {
        playerShip = FindObjectOfType<PlayerController>();
    }

    public void BeginGame()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        float startTime = Time.time;
        
        while (Time.time - startTime <= levelLoadDelay)
        {
            Vector3 newPlayerPos = new Vector3(playerShip.transform.position.x - playerShipSpeed * Time.deltaTime, playerShip.transform.position.y, playerShip.transform.position.z);
            playerShip.transform.position = newPlayerPos;

            yield return new WaitForSeconds(playerShipMovementResolution);
        }

        FindObjectOfType<MusicPlayer>().StartCountDownToFadeOut();
        LevelLoader.LoadNextLevel();
    }
}
