using System.Collections;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Config Parameters
    [SerializeField] GameObject spaceShipObject = null;
    [SerializeField] float startGameDelayWhileCameraPans = 2f;

    // Cached References
    EnemySpawner enemySpawner = null;
    SpaceShip spaceShip = null;
    CameraController cameraController = null;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        spaceShip = FindObjectOfType<SpaceShip>();
        cameraController = FindObjectOfType<CameraController>();
    }

    public void StartGame()
    {
        cameraController.GameStarted();

        StartCoroutine(DelayStartWhilePanCamera());
    }

    public IEnumerator DelayStartWhilePanCamera()
    {
        yield return new WaitForSeconds(startGameDelayWhileCameraPans);

        gameObject.SetActive(false);

        spaceShipObject.SetActive(true);
        spaceShip.GetComponent<Animator>().SetBool("gameStarted", true);

        enemySpawner.GameStarted();
    }
}
