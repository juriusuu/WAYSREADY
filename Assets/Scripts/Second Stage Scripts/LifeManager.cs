

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Include this for TextMeshPro

using UnityEngine;
using TMPro; // Include this for TextMeshPro
using UnityEngine.SceneManagement; // Include this for scene management

public class LifeManager : MonoBehaviour
{
    public TMP_Text livesText; // Reference to the UI Text for lives

    void Start()
    {
        UpdateLivesUI(); // Update the UI to reflect the current lives
    }

    public void Die()
    {
        GameManager.Instance.Die(); // Call the Die method from GameManager
        UpdateLivesUI(); // Update the UI when the player dies

        if (GameManager.Instance.currentLives > 0)
        {
            Respawn(); // Respawn the player if lives are remaining
        }
        else
        {
            // Handle game over logic if no lives are left
            Debug.Log("No lives left. Game Over!");
            // You can load a game over scene or show a game over UI here
        }
    }

    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.currentLives; // Update the text to show current lives
        }
    }
}
/* public class LifeManager : MonoBehaviour
{
    public TMP_Text livesText; // Reference to the UI Text for lives
    public SandBagManager sandBagManager; // Reference to the SandBagManager

    void Start()
    {
        UpdateLivesUI(); // Update the UI to reflect the current lives
    }

    public void Die()
    {
        GameManager.Instance.Die(); // Call the Die method from GameManager
        UpdateLivesUI(); // Update the UI when the player dies

        if (GameManager.Instance.currentLives > 0)
        {
            Respawn(); // Respawn the player if lives are remaining
        }
        else
        {
            // Handle game over logic if no lives are left
            Debug.Log("No lives left. Game Over!");
            // You can load a game over scene or show a game over UI here
        }
    }


    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.currentLives; // Update the text to show current lives
        }
    }
} */

/*       // Reset the state of all pickup items in the scene
      foreach (PickupItem item in FindObjectsByType<PickupItem>(FindObjectsSortMode.None))
      {
          item.ResetPickup(); // Reset the pickup state
      }

      // Reset the state of the SandBagManager
      if (sandBagManager != null)
      {
          sandBagManager.ResetState(); // Reset the sandbag manager state
      }

      // Ensure InventoryManager is accessible and reset if needed
      InventoryManager inventoryManager = InventoryManager.Instance;
      if (inventoryManager == null)
      {
          Debug.LogError("InventoryManager is null during respawn!");
          // Optionally, you can reinitialize or handle the error
      }
      else
      {
          // If you want to reset the inventory state, you can call a reset method here
          inventoryManager.ResetInventory(); // Uncomment if you implement this method
      } */
/* 
    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
                                                                          // Reset the state of all pickup items in the scene
        foreach (PickupItem item in FindObjectsByType<PickupItem>(FindObjectsSortMode.None))
        {
            item.ResetPickup(); // Reset the pickup state
        }

        // Reset the state of the SandBagManager
        if (sandBagManager != null)
        {
            sandBagManager.ResetState(); // Reset the sandbag manager state
        }
    } */

/* Without Game State
public class LifeManager : MonoBehaviour
{
    public TMP_Text livesText; // Reference to the UI Text for lives

    void Start()
    {
        UpdateLivesUI(); // Update the UI to reflect the current lives
    }

    public void Die()
    {
        GameManager.Instance.Die(); // Call the Die method from GameManager
        UpdateLivesUI(); // Update the UI when the player dies

        if (GameManager.Instance.currentLives > 0)
        {
            Respawn(); // Respawn the player if lives are remaining
        }
    }

    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.currentLives; // Update the text to show current lives
        }
    }
} */
/*
public class LifeManager : MonoBehaviour
{
    public TMP_Text livesText; // Reference to the UI Text for lives

    void Start()
    {
        UpdateLivesUI(); // Update the UI to reflect the current lives
    }

    public void Die()
    {
        GameManager.Instance.Die(); // Call the Die method from GameManager
        UpdateLivesUI(); // Update the UI when the player dies

        if (GameManager.Instance.currentLives > 0)
        {
            Respawn(); // Respawn the player if lives are remaining
        }
    }

    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + GameManager.Instance.currentLives; // Update the text to show current lives
        }
    }
}

*/


// Compare this snippet from Assets/Scripts/Second%20Stage%20Scripts/LifeManager.cs:
// using UnityEngine;
/*
public class LifeManager : MonoBehaviour
{
    public int maxLives = 3; // Maximum number of lives
    private static int currentLives; // Current number of lives

    public TMP_Text livesText; // Reference to the UI Text for lives

    void Start()
    {
        ResetPlayerState(); // Initialize lives at the start
        UpdateLivesUI(); // Update the UI to reflect the current lives
    }

    public void Die()
    {
        currentLives--; // Decrease the current lives
        UpdateLivesUI(); // Update the UI when the player dies

        if (currentLives <= 0)
        {
            GameOver(); // If no lives left, trigger game over
        }
        else
        {
            Respawn(); // Respawn the player if lives are remaining
        }
    }

    public void Respawn()
    {
        // Logic to respawn the player, e.g., resetting position
        // You can add specific respawn logic here if needed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    private void GameOver()
    {
        // Logic for game over, e.g., load game over screen
        Debug.Log("Game Over!"); // Log game over message
        SceneManager.LoadScene("GameOverScene"); // Load the game over scene
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives; // Reset current lives to maximum
        UpdateLivesUI(); // Update the UI to reflect the reset lives
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

/*
using TMPro; // Include this for TextMeshPro
public class LifeManager : MonoBehaviour
{
    public int maxLives = 3;
    private static int currentLives;

    public TMP_Text livesText; // Reference to the UI Text for lives

    void Start()
    {
        ResetPlayerState();
        UpdateLivesUI();
    }

    public void Die()
    {
        currentLives--;
        UpdateLivesUI(); // Update the UI when the player dies
        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Respawn(); // Call the Respawn method
        }
    }

    public void Respawn() // Change this to public
    {
        // Logic to respawn the player, e.g., resetting position
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        // Logic for game over, e.g., load game over screen
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");
    }

    public void ResetPlayerState()
    {
        currentLives = maxLives;
        UpdateLivesUI();
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