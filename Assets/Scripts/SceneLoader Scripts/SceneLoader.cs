using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance; // Singleton instance

    private void Awake()
    {
        // Ensure only one instance of SceneLoader exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate SceneLoader instances
        }
    }

    // Load a scene by its name
    /*  public void LoadSceneByName(string sceneName)
     {
         if (Application.CanStreamedLevelBeLoaded(sceneName))
         {
             Debug.Log($"Loading scene: {sceneName}");
             SceneManager.LoadScene(sceneName);
         }
         else
         {
             Debug.LogError($"Scene '{sceneName}' cannot be loaded. Ensure it is added to the Build Settings.");
         }
     } */
    public void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            // Save the current game state before transitioning
            if (GameSaveManager.Instance != null)
            {
                GameSaveManager.Instance.SaveGame();
                Debug.Log("Game state saved before transitioning to the next scene.");
            }
            else
            {
                Debug.LogError("GameSaveManager instance is null! Unable to save the game.");
            }

            // Load the next scene
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);

            // Load the saved game state in the new scene
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Ensure it is added to the Build Settings.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load the saved game state
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.LoadGame();
            Debug.Log("Game state loaded after transitioning to the new scene.");
        }
        else
        {
            Debug.LogError("GameSaveManager instance is null! Unable to load the game.");
        }

        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Reload the current scene
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Reloading current scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("Main Menu"); // Replace with the exact name of your Main Menu scene
    }

    // Load the game over scene
    public void LoadGameOver()
    {
        Debug.Log("Loading Game Over...");
        SceneManager.LoadScene("GameOverScene"); // Replace with the exact name of your Game Over scene
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}