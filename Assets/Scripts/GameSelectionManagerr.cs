using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelectionManagerr : MonoBehaviour
{
    [Header("Game Selection Buttons")]
    public Button crosswordButton;
    public Button goBagQuizButton;
    public Button firstStageButton;
    public Button secondStageButton;
    public Button exitButton;

    [Header("Scene Names")]
    public string crosswordSceneName = "CrosswordGame";
    public string goBagQuizSceneName = "GoBagQuiz";
    public string firstStageSceneName = "First Stage";
    public string secondStageSceneName = "Second Stage";

    void Start()
    {
        SetupButtons();
    }

    void SetupButtons()
    {
        if (crosswordButton != null)
            crosswordButton.onClick.AddListener(() => LoadGame(crosswordSceneName));

        if (goBagQuizButton != null)
            goBagQuizButton.onClick.AddListener(() => LoadGame(goBagQuizSceneName));

        if (firstStageButton != null)
            firstStageButton.onClick.AddListener(() => LoadGame(firstStageSceneName));

        if (secondStageButton != null)
            secondStageButton.onClick.AddListener(() => LoadGame(secondStageSceneName));

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }

    public void LoadGame(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log($"Loading {sceneName}");
        }
        else
        {
            Debug.LogWarning("Scene name is empty or null!");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Game Exited");
    }
}
