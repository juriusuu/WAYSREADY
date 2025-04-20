using UnityEngine;
using UnityEngine.UI;

public class LayfManager : MonoBehaviour
{
    public static LayfManager Instance; // Singleton instance
    public Image[] hearts; // Array of heart images
    public Sprite fullHeart; // Sprite for a full heart
    public Sprite emptyHeart; // Sprite for an empty heart
    public int currentLives; // Current number of lives

    /* 
        private void Awake()
        {
            // Singleton pattern to persist LayfManager across scenes
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances
            }
        }

     */
    private void Start()
    {
        if (currentLives <= 0)
        {
            currentLives = hearts.Length; // Initialize lives to the number of hearts
        }

        UpdateHeartsUI(); // Initialize the heart UI
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the player's life
            Debug.Log($"Life lost. Remaining lives: {currentLives}");

            UpdateHeartsUI(); // Update the heart UI
        }

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            NotifyGameManagerGameOver(); // Notify the GameManager when lives are out
        }
    }

    private void UpdateHeartsUI()
    {
        Debug.Log($"Updating hearts UI. Current lives: {currentLives}");
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite = fullHeart; // Show full heart
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Show empty heart
            }
        }
    }

    public int GetRemainingLives()
    {
        Debug.Log($"GetRemainingLives called. Current lives: {currentLives}");
        return currentLives; // Return the number of remaining lives
    }


    private void NotifyGameManagerGameOver()
    {
        Debug.Log("Notifying GameManager of GameOver...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver); // Transition to GameOver state
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void AddLife()
    {
        if (currentLives < hearts.Length)
        {
            currentLives++;
            UpdateHeartsUI(); // Update the heart UI
            Debug.Log("Life added. Current lives: " + currentLives);
        }
        else
        {
            Debug.LogWarning("Maximum lives reached. Cannot add more lives.");
        }
    }
    /* 
        private void NotifyGameManagerGameOver()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEnterGameOverState(); // Call the GameManager's game over method
            }
            else
            {
                Debug.LogError("GameManager instance not found!");
            }
        } */
}