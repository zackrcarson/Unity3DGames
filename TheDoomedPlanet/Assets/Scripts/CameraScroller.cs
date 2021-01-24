using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    // Config Params
    [SerializeField] float startFieldOfView = 120;
    [SerializeField] float endFieldOfView = 80;
    [SerializeField] float scrollSpeed = .01f;
    [SerializeField] float zoomDelay = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.fieldOfView = startFieldOfView;

        StartCoroutine(PanInCamera());
    }

    private IEnumerator PanInCamera()
    {
        while (Camera.main.fieldOfView >= endFieldOfView)
        {
            yield return new WaitForSeconds(zoomDelay);

            Camera.main.fieldOfView -= scrollSpeed;
        }
    }
}
