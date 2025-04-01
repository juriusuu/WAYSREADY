using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button buttonEasy;
    public Button buttonMedium;
    public Button buttonHard;

    public DifficultyManager difficultyManager;
    public TimerManager timerManager; // Reference to TimerManager

    void Start()
    {
        buttonEasy.onClick.AddListener(() => StartGame(DifficultyManager.DifficultyLevel.Easy));
        buttonMedium.onClick.AddListener(() => StartGame(DifficultyManager.DifficultyLevel.Normal));
        buttonHard.onClick.AddListener(() => StartGame(DifficultyManager.DifficultyLevel.Hard));
    }

    void StartGame(DifficultyManager.DifficultyLevel difficulty)
    {
        difficultyManager.SetDifficulty(difficulty);

        // Set initial lives and timer based on difficulty
        switch (difficulty)
        {
            case DifficultyManager.DifficultyLevel.Easy:
                GameeManager.Instance.SetInitialLives(2); // Set lives for Easy
                timerManager.SetCountdownTime(300f); // 5 minutes for Easy
                break;
            case DifficultyManager.DifficultyLevel.Normal:
                GameeManager.Instance.SetInitialLives(3); // Set lives for Normal
                timerManager.SetCountdownTime(180f); // 3 minutes for Normal
                break;
            case DifficultyManager.DifficultyLevel.Hard:
                GameeManager.Instance.SetInitialLives(1); // Set lives for Hard
                timerManager.SetCountdownTime(60f); // 1 minute for Hard
                break;
        }

        // Load the game scene or start the gameplay
        Debug.Log("Starting game with difficulty: " + difficulty);
        // SceneManager.LoadScene("YourGameScene"); // Uncomment and replace with your actual game scene name
    }
}