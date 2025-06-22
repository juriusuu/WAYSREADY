using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public GameObject winPanel;

    public int requiredItemCount = 3; // ← Set this manually in Inspector (e.g. 3 correct items)

    private int score = 0;
    private int correctItemCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreText();

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void AddScore(int points, bool isCorrect)
    {
        score += points;
        UpdateScoreText();

        if (isCorrect)
        {
            correctItemCount++;
            CheckIfGameComplete();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void CheckIfGameComplete()
    {
        Debug.Log($"Correct items placed: {correctItemCount} / {requiredItemCount}");

        if (correctItemCount >= requiredItemCount)
        {
            Debug.Log("You win!");
            if (winPanel != null)
                winPanel.SetActive(true);
        }
    }
}
