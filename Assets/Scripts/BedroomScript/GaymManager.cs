using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

// Define the GameState enum outside the GameManager class
public enum GameState
{
    Stage1_Easy,
    Stage1_Medium,
    Stage1_Hard,
    Stage1_MiniGame,
    Stage2_Easy,
    Stage2_Medium,
    Stage2_Hard,
    Stage2_MiniGame,
    Stage3_Easy,
    Stage3_Medium,
    Stage3_Hard,
    Stage3_MiniGame,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public GameState currentState; // Current game state

    private string currentSceneName; // Store the name of the current scene

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
            Destroy(gameObject); // Destroy duplicate GameManager
        }
    }

    private void Start()
    {
        // Initialize the game state
        currentSceneName = SceneManager.GetActiveScene().name; // Get the name of the current scene
        currentState = GameState.Stage1_Easy;
        StartStage();
    }

    public void StartStage()
    {
        switch (currentState)
        {
            case GameState.Stage1_Easy:
                if (currentSceneName != "Stage 1 Bedroom")
                {
                    Debug.Log("Starting Stage 1 - Easy");
                    SceneManager.LoadScene("Stage 1 Bedroom");
                    currentSceneName = "Stage 1 Bedroom"; // Update the current scene name
                }
                break;

            case GameState.Stage1_Medium:
                Debug.Log("Starting Stage 1 - Medium");
                SceneManager.LoadScene("Stage1_Medium"); // Load the scene for Stage 1 Medium
                break;

            case GameState.Stage1_Hard:
                Debug.Log("Starting Stage 1 - Hard");
                SceneManager.LoadScene("Stage1_Hard"); // Load the scene for Stage 1 Hard
                break;

            case GameState.Stage1_MiniGame:
                Debug.Log("Starting Stage 1 - MiniGame");
                SceneManager.LoadScene("Stage1_MiniGame"); // Load the scene for Stage 1 MiniGame
                break;

            case GameState.Stage2_Easy:
                Debug.Log("Starting Stage 2 - Easy");
                SceneManager.LoadScene("Stage2_Easy"); // Load the scene for Stage 2 Easy
                break;

            case GameState.Stage2_Medium:
                Debug.Log("Starting Stage 2 - Medium");
                SceneManager.LoadScene("Stage2_Medium"); // Load the scene for Stage 2 Medium
                break;

            case GameState.Stage2_Hard:
                Debug.Log("Starting Stage 2 - Hard");
                SceneManager.LoadScene("Stage2_Hard"); // Load the scene for Stage 2 Hard
                break;

            case GameState.Stage2_MiniGame:
                Debug.Log("Starting Stage 2 - MiniGame");
                SceneManager.LoadScene("Stage2_MiniGame"); // Load the scene for Stage 2 MiniGame
                break;

            case GameState.Stage3_Easy:
                Debug.Log("Starting Stage 3 - Easy");
                SceneManager.LoadScene("Stage3_Easy"); // Load the scene for Stage 3 Easy
                break;

            case GameState.Stage3_Medium:
                Debug.Log("Starting Stage 3 - Medium");
                SceneManager.LoadScene("Stage3_Medium"); // Load the scene for Stage 3 Medium
                break;

            case GameState.Stage3_Hard:
                Debug.Log("Starting Stage 3 - Hard");
                SceneManager.LoadScene("Stage3_Hard"); // Load the scene for Stage 3 Hard
                break;

            case GameState.Stage3_MiniGame:
                Debug.Log("Starting Stage 3 - MiniGame");
                SceneManager.LoadScene("Stage3_MiniGame"); // Load the scene for Stage 3 MiniGame
                break;

            case GameState.GameOver:
                Debug.Log("Game Over!");
                SceneManager.LoadScene("GameOverScene"); // Load the Game Over scene
                break;
        }
    }

    public void TransitionToNextState()
    {
        // Example: Transition to the next state
        if (currentState == GameState.Stage1_Easy)
        {
            currentState = GameState.Stage1_Medium;
        }
        else if (currentState == GameState.Stage1_Medium)
        {
            currentState = GameState.Stage1_Hard;
        }
        else if (currentState == GameState.Stage1_Hard)
        {
            currentState = GameState.Stage1_MiniGame;
        }
        else if (currentState == GameState.Stage1_MiniGame)
        {
            currentState = GameState.Stage2_Easy;
        }
        else if (currentState == GameState.GameOver)
        {
            Debug.Log("Game Over!");
        }

        StartStage(); // Start the next stage
    }

    public void HandleGameOver()
    {
        Debug.Log("Game Over!");
        currentState = GameState.GameOver;
        StartStage(); // Transition to the Game Over scene
    }
}