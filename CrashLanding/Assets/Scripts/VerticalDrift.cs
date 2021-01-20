using UnityEngine;

public class VerticalDrift : MonoBehaviour
{
    // Config Parameters
    float speed = 10f;
    float radius = 10f;

    // Cached References
    CloudSpawner cloudSpawner;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x;

        cloudSpawner = FindObjectOfType<CloudSpawner>();

        if (cloudSpawner)
        {
            speed = cloudSpawner.GetVelocity();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetRadius(float newRadius)
    {
        transform.localScale = new Vector3(newRadius, newRadius, newRadius);
    }
}
