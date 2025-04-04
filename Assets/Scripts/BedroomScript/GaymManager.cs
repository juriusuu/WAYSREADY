using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

// Define the GameState enum outside the GameManager class
public enum GameState
{
    MainMenu, // Add MainMenu state
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

    public GameObject collectionsPanel;
    public GameObject shopPanel;
    public GameObject libraryPanel;

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
        currentState = GameState.MainMenu; // Start with the Main Menu
        StartStage();
    }

    public void StartStage()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                Debug.Log("Loading Main Menu...");
                SceneManager.LoadScene("Main Menu"); // Replace with the actual name of your Main Menu scene
                break;

            case GameState.Stage1_Easy:
                Debug.Log("Starting Stage 1 - Easy");
                SceneManager.LoadScene("Stage 1 Bedroom");
                break;

            case GameState.Stage1_Medium:
                Debug.Log("Starting Stage 1 - Medium");
                SceneManager.LoadScene("Stage1_Medium");
                break;

            case GameState.Stage1_Hard:
                Debug.Log("Starting Stage 1 - Hard");
                SceneManager.LoadScene("Stage1_Hard");
                break;

            case GameState.Stage1_MiniGame:
                Debug.Log("Starting Stage 1 - MiniGame");
                SceneManager.LoadScene("Stage1_MiniGame");
                break;

            case GameState.Stage2_Easy:
                Debug.Log("Starting Stage 2 - Easy");
                SceneManager.LoadScene("Stage2_Easy");
                break;

            case GameState.Stage2_Medium:
                Debug.Log("Starting Stage 2 - Medium");
                SceneManager.LoadScene("Stage2_Medium");
                break;

            case GameState.Stage2_Hard:
                Debug.Log("Starting Stage 2 - Hard");
                SceneManager.LoadScene("Stage2_Hard");
                break;

            case GameState.Stage2_MiniGame:
                Debug.Log("Starting Stage 2 - MiniGame");
                SceneManager.LoadScene("Stage2_MiniGame");
                break;

            case GameState.Stage3_Easy:
                Debug.Log("Starting Stage 3 - Easy");
                SceneManager.LoadScene("Stage3_Easy");
                break;

            case GameState.Stage3_Medium:
                Debug.Log("Starting Stage 3 - Medium");
                SceneManager.LoadScene("Stage3_Medium");
                break;

            case GameState.Stage3_Hard:
                Debug.Log("Starting Stage 3 - Hard");
                SceneManager.LoadScene("Stage3_Hard");
                break;

            case GameState.Stage3_MiniGame:
                Debug.Log("Starting Stage 3 - MiniGame");
                SceneManager.LoadScene("Stage3_MiniGame");
                break;

            case GameState.GameOver:
                Debug.Log("Game Over!");
                SceneManager.LoadScene("GameOverScene");
                break;

            default:
                Debug.LogError("Unknown game state!");
                break;
        }
    }

    public void TransitionToNextState()
    {
        // Transition to the next state
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

    public void StartGame()
    {
        Debug.Log("Starting the game...");
        currentState = GameState.Stage1_Easy; // Set the state to the first stage
        StartStage(); // Load the first stage
    }

    public void GoToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");
        currentState = GameState.MainMenu; // Set the state to MainMenu
        StartStage(); // Load the Main Menu
    }

    public void HandleGameOver()
    {
        Debug.Log("Game Over!");
        currentState = GameState.GameOver;
        StartStage(); // Transition to the Game Over scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    public void ShowCollections()
    {
        Debug.Log("Opening Collections Panel...");
        collectionsPanel.SetActive(true);
        shopPanel.SetActive(false);
        libraryPanel.SetActive(false);
    }

    public void ShowShop()
    {
        Debug.Log("Opening Shop Panel...");
        collectionsPanel.SetActive(false);
        shopPanel.SetActive(true);
        libraryPanel.SetActive(false);
    }

    public void ShowLibrary()
    {
        Debug.Log("Opening Library Panel...");
        collectionsPanel.SetActive(false);
        shopPanel.SetActive(false);
        libraryPanel.SetActive(true);
    }

    public void CloseAllPanels()
    {
        Debug.Log("Closing all panels...");
        collectionsPanel.SetActive(false);
        shopPanel.SetActive(false);
        libraryPanel.SetActive(false);
    }
}