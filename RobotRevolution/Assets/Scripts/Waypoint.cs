using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Config Params
    [SerializeField] AudioClip successSound = null;
    [SerializeField] AudioClip failureSound = null;
    const int gridSize = 10;
    const float gapPercent = 0.1f;

    // State Variables. Public is OK because this is a data storage class
    Vector2Int gridPosition = new Vector2Int(0, 0);

    public bool isExplored = false;
    public Waypoint exploredFrom = null;

    public bool isPlaceable = true;

    // Cached References
    TowerSpawner towerSpawner = null;
    AudioSource audioSource = null;

    private void Start()
    {
        towerSpawner = FindObjectOfType<TowerSpawner>();
        audioSource = GetComponent<AudioSource>();
    }

    public void GetGridSizeAndGap(out int outGridSize, out float outGapPercent)
    {
        outGridSize = gridSize;
        outGapPercent = gapPercent;
    }

    public Vector2Int GetGridPosition()
    {
        Vector2Int cubePosition = new Vector2Int(
                                 Mathf.RoundToInt(transform.position.x / gridSize),
                                 Mathf.RoundToInt(transform.position.z / gridSize)
                               );

        return cubePosition;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaceable)
            {
                audioSource.PlayOneShot(successSound);
                towerSpawner.PlaceTower(this);
            }
            else
            {
                audioSource.PlayOneShot(failureSound);
            }
        }
    }
}
