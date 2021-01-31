using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Config Params
    const int gridSize = 10;
    const float gapPercent = 0.1f;
    const string topCubeFaceName = "Top";

    // State Variables. Public is OK because this is a data storage class
    Vector2Int gridPosition = new Vector2Int(0, 0);
    public bool isExplored = false;
    public Waypoint exploredFrom = null;

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

    public void SetTopColor(Color color)
    {
        MeshRenderer topMeshRenderer = transform.Find(topCubeFaceName).GetComponent<MeshRenderer>();
        topMeshRenderer.material.color = color;
    }
}
