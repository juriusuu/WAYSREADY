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
    public void LoadSceneByName(string sceneName)
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