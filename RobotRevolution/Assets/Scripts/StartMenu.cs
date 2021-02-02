using System.Collections;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Config Parameters
    [SerializeField] GameObject spaceShipObject = null;
    [SerializeField] float startGameDelayWhileCameraPans = 2f;
    [SerializeField] GameObject[] startMenuObjects = null;

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
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (GameObject child in startMenuObjects)
        {
            child.gameObject.SetActive(false);
        }

        cameraController.GameStarted();
        
        StartCoroutine(DelayStartWhilePanCamera());
    }

    public IEnumerator DelayStartWhilePanCamera()
    {
        yield return new WaitForSeconds(startGameDelayWhileCameraPans);

        spaceShipObject.SetActive(true);
        spaceShip.GetComponent<Animator>().SetBool("gameStarted", true);

        enemySpawner.GameStarted();

        gameObject.SetActive(false);
    }
}
