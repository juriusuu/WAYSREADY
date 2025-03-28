using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance; // Singleton instance

    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    public DifficultyLevel currentDifficulty;

    // Difficulty settings for each level
    private DifficultySettings easySettings = new DifficultySettings(5, 2, 300f); // 5 hints, 2 lives, 300 seconds
    private DifficultySettings normalSettings = new DifficultySettings(3, 3, 180f); // 3 hints, 3 lives, 180 seconds
    private DifficultySettings hardSettings = new DifficultySettings(0, 1, 60f); // 0 hints, 1 life, 60 seconds

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate DifficultyManager
        }
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        currentDifficulty = difficulty;
        InitializeGameSettings();
    }

    private void InitializeGameSettings()
    {
        DifficultySettings currentSettings;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                currentSettings = easySettings;
                break;
            case DifficultyLevel.Normal:
                currentSettings = normalSettings;
                break;
            case DifficultyLevel.Hard:
                currentSettings = hardSettings;
                break;
            default:
                currentSettings = normalSettings; // Default to normal if not set
                break;
        }

        // Apply settings (you can add more logic here if needed)
        GameManager.Instance.SetInitialLives(currentSettings.lives);
        // You can also manage hints and other settings here
        Debug.Log($"Current Difficulty: {currentDifficulty}, Hints: {currentSettings.hints}, Lives: {currentSettings.lives}, Time Limit: {currentSettings.timeLimit}");
    }
}

[System.Serializable]
public class DifficultySettings
{
    public int hints;       // Number of hints available
    public int lives;       // Number of lives available
    public float timeLimit; // Time limit in seconds

    public DifficultySettings(int hints, int lives, float timeLimit)
    {
        this.hints = hints;
        this.lives = lives;
        this.timeLimit = timeLimit;
    }
}