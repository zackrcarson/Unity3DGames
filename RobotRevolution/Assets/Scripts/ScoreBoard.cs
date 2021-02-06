using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    // Config Params
    [SerializeField] Text healthText = null;
    [SerializeField] Text scoreText = null;

    // Cached References
    BaseHealth baseHealth = null;

    // State Variables
    int score = 0;

    private void Awake()
    {
        ScoreBoard[] scoreBoards = FindObjectsOfType<ScoreBoard>();
        int numberScoreBoards = scoreBoards.Length;

        if (numberScoreBoards == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            foreach (ScoreBoard scoreBoard in scoreBoards)
            {
                scoreBoard.ResetScoreBoard();
            }

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        baseHealth = FindObjectOfType<BaseHealth>();

        healthText.text = baseHealth.GetHealth().ToString();
        scoreText.text = score.ToString();
    }
    
    public void UpdateHealth(int health)
    {
        if (health < 0)
        {
            health = 0;
        }

        healthText.text = health.ToString();
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void ResetScoreBoard()
    {
        baseHealth = FindObjectOfType<BaseHealth>();

        healthText.text = baseHealth.GetHealth().ToString();
    }
}
