using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{
    // Cached References
    TextMesh cubeLabel = null;
    Waypoint waypoint = null;
    PathFinder pathFinder = null;

    // State Variables
    int gridSize;
    float gapPercent;
    Vector2 gridPosition;

    private void Awake()
    {
        cubeLabel = GetComponentInChildren<TextMesh>();
        pathFinder = FindObjectOfType<PathFinder>();
        waypoint = GetComponent<Waypoint>();

        waypoint.GetGridSizeAndGap(out gridSize, out gapPercent);
    }

    private void Update()
    {
        SnapPosition();

        ResizeScale();

        UpdateLabel();
    }

    private void SnapPosition()
    {
        gridPosition = waypoint.GetGridPosition();

        transform.position = new Vector3(gridPosition.x * gridSize, 0f, gridPosition.y * gridSize);
    }

    private void ResizeScale()
    {
        Vector3 cubeScale;
        cubeScale.x = gridSize * (1 - gapPercent);
        cubeScale.y = gridSize * (1 - gapPercent);
        cubeScale.z = gridSize * (1 - gapPercent);

        transform.localScale = cubeScale;
    }

    private void UpdateLabel()
    {
        string labelText = gridPosition.x + "," + gridPosition.y;

        if (cubeLabel)
        {
            cubeLabel.text = labelText;
        }

        gameObject.name = "Cube " + labelText;
    }
}
