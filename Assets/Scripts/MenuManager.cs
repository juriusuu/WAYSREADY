using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Menu Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button exitButton;
    public Button backToMainButton;
    public Button backFromCreditsButton;

    void Start()
    {
        SetupMenuButtons();
        ShowMainMenu();
    }

    void SetupMenuButtons()
    {
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(ShowSettings);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(ShowCredits);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        if (backToMainButton != null)
            backToMainButton.onClick.AddListener(ShowMainMenu);

        if (backFromCreditsButton != null)
            backFromCreditsButton.onClick.AddListener(ShowMainMenu);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameSelection");
    }

    public void ShowMainMenu()
    {
        HideAllPanels();
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllPanels();
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        HideAllPanels();
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
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
