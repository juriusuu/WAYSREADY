using UnityEngine;

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
}