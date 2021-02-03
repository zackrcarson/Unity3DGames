using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Config Parameters
    [SerializeField] Transform startCameraTransform = null;
    [SerializeField] int numPanFrames = 100;

    // State Parameters
    Vector3 mainCameraPosition = new Vector3(0f, 0f, 0f);
    Quaternion mainCameraRotation = new Quaternion(0f, 0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        mainCameraPosition = transform.position;
        mainCameraRotation = transform.rotation;

        transform.position = startCameraTransform.position;
        transform.rotation = startCameraTransform.rotation;

        startCameraTransform.gameObject.SetActive(false);
    }

    public void GameStarted()
    {
        StartCoroutine(PanCamera());
    }

    private IEnumerator PanCamera()
    {
        float distanceToMove = Vector3.Distance(transform.position, mainCameraPosition);
        float angleToMove = Quaternion.Angle(transform.rotation, mainCameraRotation);

        for (int i = 0; i < numPanFrames; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, mainCameraPosition, distanceToMove / numPanFrames);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mainCameraRotation, angleToMove / numPanFrames);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = mainCameraPosition;
        transform.rotation = mainCameraRotation;
    }
}
