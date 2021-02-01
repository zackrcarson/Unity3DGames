using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Config Params
    const int gridSize = 10;
    const float gapPercent = 0.1f;

    // State Variables. Public is OK because this is a data storage class
    Vector2Int gridPosition = new Vector2Int(0, 0);

    public bool isExplored = false;
    public Waypoint exploredFrom = null;

    public bool isPlaceable = true;

    // Cached References
    TowerSpawner towerSpawner = null;

    private void Start()
    {
        towerSpawner = FindObjectOfType<TowerSpawner>();
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
                towerSpawner.PlaceTower(transform);

                isPlaceable = false;
            }
            else
            {
                // Play "erhhh" sound
            }
        }
    }
}
