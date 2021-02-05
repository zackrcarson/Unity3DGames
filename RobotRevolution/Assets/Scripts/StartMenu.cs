﻿using System.Collections;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Config Parameters
    [SerializeField] GameObject spaceShipObject = null;
    [SerializeField] float startGameDelayWhileCameraPans = 2f;
    [SerializeField] GameObject[] startMenuObjects = null;
    [SerializeField] GameObject scoreBoardUI = null;
    [SerializeField] PauseMenu pauseMenu = null;

    // Cached References
    EnemySpawner enemySpawner = null;
    SpaceShip spaceShip = null;
    CameraController cameraController = null;
    MusicPlayer musicPlayer = null;

    private void Start()
    {
        scoreBoardUI.SetActive(false);

        enemySpawner = FindObjectOfType<EnemySpawner>();
        spaceShip = FindObjectOfType<SpaceShip>();
        cameraController = FindObjectOfType<CameraController>();
        
        pauseMenu.canPause = false;
    }

    public void StartGame()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        musicPlayer.StartCountDownToFadeOut();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (GameObject child in startMenuObjects)
        {
            child.gameObject.SetActive(false);
        }

        cameraController.GameStarted();
        
        StartCoroutine(DelayStartWhilePanCamera());

        scoreBoardUI.SetActive(true);

        pauseMenu.canPause = true;
    }

    public IEnumerator DelayStartWhilePanCamera()
    {
        yield return new WaitForSeconds(startGameDelayWhileCameraPans);

        spaceShipObject.SetActive(true);
        spaceShip.GetComponent<Animator>().SetTrigger("gameStarted");

        enemySpawner.GameStarted();

        gameObject.SetActive(false);
    }
}
