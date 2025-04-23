using UnityEngine;

public class SceneLoaderButtonHelper : MonoBehaviour
{
    // Load a specific scene by name
    public void LoadScene(string sceneName)
    {
        if (SceneLoader.Instance != null)
        {
            Debug.Log($"SceneLoaderButtonHelper: Requesting to load scene '{sceneName}'");
            SceneLoader.Instance.LoadSceneByName(sceneName);
            // Reset Time.timeScale before transitioning
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("SceneLoader instance is null! Unable to load the scene.");
        }
    }

    // Load the Main Menu scene
    public void LoadMainMenu()
    {
        if (SceneLoader.Instance != null)
        {
            Debug.Log("SceneLoaderButtonHelper: Requesting to load the Main Menu");
            // Reset Time.timeScale before transitioning
            Time.timeScale = 1f;
            SceneLoader.Instance.LoadMainMenu();
        }
        else
        {
            Debug.LogError("SceneLoader instance is null! Unable to load the Main Menu.");
        }
    }

    // Reload the current scene
    public void ReloadCurrentScene()
    {
        if (SceneLoader.Instance != null)
        {
            Debug.Log("SceneLoaderButtonHelper: Requesting to reload the current scene");
            SceneLoader.Instance.ReloadCurrentScene();
            // Reset Time.timeScale before transitioning
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("SceneLoader instance is null! Unable to reload the current scene.");
        }
    }

    // Quit the game
    public void QuitGame()
    {
        if (SceneLoader.Instance != null)
        {
            Debug.Log("SceneLoaderButtonHelper: Requesting to quit the game");
            SceneLoader.Instance.QuitGame();
            // Reset Time.timeScale before transitioning
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("SceneLoader instance is null! Unable to quit the game.");
        }
    }
}
/* using UnityEngine;

public class SceneLoaderButtonHelper : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadSceneByName(sceneName);
    }

    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void ReloadCurrentScene()
    {
        SceneLoader.Instance.ReloadCurrentScene();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
} */