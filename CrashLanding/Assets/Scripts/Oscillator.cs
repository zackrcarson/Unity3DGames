using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    // Config Params
    [SerializeField] Vector3 movementVector = new Vector3(0, 0, 0);
    [SerializeField] float movementPeriod = 0f;

    // State Variables
    Vector3 startingPos;
    float startingTime;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;

        startingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementPeriod <= Mathf.Epsilon) { return; }

        float cycles = (Time.time - startingTime) / movementPeriod;

        float rawSinOutput = Mathf.Sin(cycles * 2f * Mathf.PI);
        
        float movementFactor = (rawSinOutput / 2f) + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
