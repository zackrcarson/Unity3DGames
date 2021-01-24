using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    // Cached References
    Text scoreText = null;

    // State Variables
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int points)
    {
        score += points;

        scoreText.text = score.ToString();
    }
}
