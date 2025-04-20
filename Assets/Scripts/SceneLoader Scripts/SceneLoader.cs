using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

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
    public void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            // Save the current game state before transitioning
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveGame(sceneName);
                Debug.Log("Game state saved before transitioning to the next scene.");
            }
            else
            {
                Debug.LogError("GameManager instance is null! Unable to save the game.");
            }

            // Load the next scene
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);

            // Restore the saved game state in the new scene
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Ensure it is added to the Build Settings.");
        }
    }

    // Reload the current scene
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Save the current game state before reloading
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Game state saved before reloading the current scene.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to save the game.");
        }

        Debug.Log($"Reloading current scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);

        // Restore the saved game state after reloading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        // Save the current game state before transitioning
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Game state saved before transitioning to the Main Menu.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to save the game.");
        }

        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("Main Menu"); // Replace with the exact name of your Main Menu scene

        // Restore the saved game state in the Main Menu
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Load the game over scene
    public void LoadGameOver()
    {
        // Save the current game state before transitioning
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Game state saved before transitioning to the Game Over scene.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to save the game.");
        }

        Debug.Log("Loading Game Over...");
        SceneManager.LoadScene("GameOverScene"); // Replace with the exact name of your Game Over scene

        // Restore the saved game state in the Game Over scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Quit the game
    public void QuitGame()
    {
        // Save the current game state before quitting
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            Debug.Log("Game state saved before quitting the game.");
        }
        else
        {
            Debug.LogError("GameManager instance is null! Unable to save the game.");
        }

        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // Event handler for when a scene is loaded
    /*   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
      {
          // Load the saved game state
          if (GameManager.Instance != null)
          {
              GameManager.Instance.LoadGame();
              Debug.Log("Game state loaded after transitioning to the new scene.");
          }
          else
          {
              Debug.LogError("GameManager instance is null! Unable to load the game.");
          }

          // Unsubscribe from the event to avoid duplicate calls
          SceneManager.sceneLoaded -= OnSceneLoaded;
      } */

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Path to the save file
        string saveFilePath = Path.Combine(Application.persistentDataPath, "SavedGameWR.json");

        // Check if the save file exists
        if (File.Exists(saveFilePath))
        {
            try
            {
                // Read and deserialize the JSON save file
                string json = File.ReadAllText(saveFilePath);
                GameManager.SaveData saveData = JsonConvert.DeserializeObject<GameManager.SaveData>(json);

                // Update the CoinUIManager with the saved coin count
                if (CoinUIManager.Instance != null)
                {
                    CoinUIManager.Instance.UpdateCoinUI(saveData.coinCount);
                    Debug.Log($"Coin UI updated after loading scene: {scene.name}. Coin count: {saveData.coinCount}");
                }
                else
                {
                    Debug.LogWarning("CoinUIManager instance is null. Unable to update coin UI.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load and update coin count from save file: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Save file not found. Coin UI will not be updated.");
        }

        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


}

/* using UnityEngine;
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
    public void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            // Save the current scene state before transitioning
            SaveSceneState();

            // Load the next scene
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);

            // Restore the saved game state in the new scene
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Ensure it is added to the Build Settings.");
        }
    }

    // Reload the current scene
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Save the current scene state before reloading
        SaveSceneState();

        Debug.Log($"Reloading current scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);

        // Restore the saved game state after reloading
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        // Save the current scene state before transitioning
        SaveSceneState();

        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("Main Menu"); // Replace with the exact name of your Main Menu scene

        // Restore the saved game state in the Main Menu
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Load the game over scene
    public void LoadGameOver()
    {
        // Save the current scene state before transitioning
        SaveSceneState();

        Debug.Log("Loading Game Over...");
        SceneManager.LoadScene("GameOverScene"); // Replace with the exact name of your Game Over scene

        // Restore the saved game state in the Game Over scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Quit the game
    public void QuitGame()
    {
        // Save the current scene state before quitting
        SaveSceneState();

        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // Event handler for when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Restore the saved game state
        RestoreSceneState();

        // Unsubscribe from the event to avoid duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Save the current scene state
    private void SaveSceneState()
    {
        // Save the game state using GameSaveManager
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.SaveGame();
            Debug.Log("Game state saved before transitioning to the next scene.");
        }
        else
        {
            Debug.LogError("GameSaveManager instance is null! Unable to save the game.");
        }

        // Save the state of all SavableObjects in the scene
        var savableObjects = FindObjectsOfType<SavableObject>();
        foreach (var savableObject in savableObjects)
        {
            savableObject.SaveState();
        }

        // Save the scene state to a JSON file
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SaveSceneState();
            Debug.Log("Scene state saved.");
        }
        else
        {
            Debug.LogError("GameStateManager instance is null! Unable to save the scene state.");
        }
    }

    // Restore the saved scene state
    private void RestoreSceneState()
    {
        // Load the game state using GameSaveManager
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.LoadGame();
            Debug.Log("Game state loaded after transitioning to the new scene.");
        }
        else
        {
            Debug.LogError("GameSaveManager instance is null! Unable to load the game.");
        }

        // Restore the state of all SavableObjects in the scene
        var savableObjects = FindObjectsOfType<SavableObject>();
        foreach (var savableObject in savableObjects)
        {
            savableObject.RestoreState();
        }

        // Load the scene state from a JSON file
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.LoadSceneState();
            Debug.Log("Scene state restored.");
        }
        else
        {
            Debug.LogError("GameStateManager instance is null! Unable to restore the scene state.");
        }
    }
} */
//APril 15
/* using UnityEngine;
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
} */