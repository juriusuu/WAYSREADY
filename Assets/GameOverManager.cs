using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign GameOverPanel in the Inspector

    void Start()
    {
        gameOverPanel.SetActive(false); // Hide Game Over screen initially
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true); // Show Game Over screen
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume game speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
