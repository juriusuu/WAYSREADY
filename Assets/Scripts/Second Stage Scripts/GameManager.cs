using UnityEngine;
using TMPro; // Include this for TextMeshPro

using UnityEngine;
using TMPro; // Include this for TextMeshPro
using UnityEngine;
using TMPro; // Include this for TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public int maxLives = 3; // Maximum number of lives
    public int currentLives; // Current number of lives

    public TMP_Text livesText; // Reference to the UI Text for lives
    public GameState currentGameState; // Current game state

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

        ResetPlayerState(); // Initialize lives at the start
        currentGameState = GameState.Playing; // Set initial game state
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives; // Reset current lives to maximum
        UpdateLivesUI(); // Update the UI to reflect the reset lives
    }

    public void SetInitialLives(int lives)
    {
        maxLives = lives; // Set the maximum lives based on difficulty
        ResetPlayerState(); // Reset player state to initialize lives
    }

    public void Die()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the current lives
            UpdateLivesUI(); // Update the UI when the player dies

            if (currentLives <= 0)
            {
                currentGameState = GameState.PlayerDead; // Set game state to PlayerDead
                GameOver(); // Trigger game over
            }
            else
            {
                Debug.Log($"Player has {currentLives} life(s) left.");
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!"); // Log game over message
        // Load the game over scene or handle game over logic here
        // SceneManager.LoadScene("GameOverScene"); // Uncomment and replace with your actual game over scene name
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Update the text to show current lives
        }
    }
}
// Implement Singleton pattern
/* 
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public int maxLives = 3; // Maximum number of lives
    public int currentLives; // Current number of lives

    public TMP_Text livesText; // Reference to the UI Text for lives
    public GameState currentGameState; // Current game state

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

        ResetPlayerState(); // Initialize lives at the start
        currentGameState = GameState.Playing; // Set initial game state
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives; // Reset current lives to maximum
        UpdateLivesUI(); // Update the UI to reflect the reset lives
    }

    public void Die()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the current lives
            UpdateLivesUI(); // Update the UI when the player dies

            if (currentLives <= 0)
            {
                currentGameState = GameState.PlayerDead; // Set game state to PlayerDead
                GameOver(); // Trigger game over
            }
            else
            {
                Debug.Log($"Player has {currentLives} life(s) left.");
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!"); // Log game over message
        // Load the game over scene or handle game over logic here
        // SceneManager.LoadScene("GameOverScene"); // Uncomment and replace with your actual game over scene name
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Update the text to show current lives
        }
    }
} */

// Without GameState
/* public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public int maxLives = 3; // Maximum number of lives
    public int currentLives; // Current number of lives

    public TMP_Text livesText; // Reference to the UI Text for lives

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

        ResetPlayerState(); // Initialize lives at the start
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives; // Reset current lives to maximum
        UpdateLivesUI(); // Update the UI to reflect the reset lives
    }

    public void Die()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the current lives
            UpdateLivesUI(); // Update the UI when the player dies

            if (currentLives <= 0)
            {
                GameOver(); // If current lives are 0, trigger game over
            }
            else
            {
                Debug.Log($"Player has {currentLives} life(s) left.");
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!"); // Log game over message
                                 // Load the game over scene or handle game over logic here
                                 // SceneManager.LoadScene("GameOverScene"); // Replace with your actual game over scene name
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Update the text to show current lives
        }
    }
} */

/*
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public int maxLives = 3; // Maximum number of lives
    public int currentLives; // Current number of lives

    public TMP_Text livesText; // Reference to the UI Text for lives

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

        ResetPlayerState(); // Initialize lives at the start
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives; // Reset current lives to maximum
        UpdateLivesUI(); // Update the UI to reflect the reset lives
    }

    public void Die()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the current lives
            UpdateLivesUI(); // Update the UI when the player dies

            if (currentLives <= 0)
            {
                GameOver(); // If current lives are 0, trigger game over
            }
            else
            {
                // Optionally, you can add any logic here for when the player has lives left
                Debug.Log($"Player has {currentLives} life(s) left.");
            }
        }
    }

    private void GameOver()
    {
        // Logic for game over, e.g., load game over screen
        Debug.Log("Game Over!"); // Log game over message
        // Load the game over scene or handle game over logic here
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives; // Update the text to show current lives
        }
    }
}

*/