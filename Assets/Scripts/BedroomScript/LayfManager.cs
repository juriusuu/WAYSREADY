/* using UnityEngine;
using UnityEngine.UI;

public class LayfManager : MonoBehaviour
{
    public static LayfManager Instance; // Singleton instance
    public Image[] hearts; // Array of heart images
    public Sprite fullHeart; // Sprite for a full heart
    public Sprite emptyHeart; // Sprite for an empty heart
    public int currentLives; // Current number of lives

    private void Start()
    {
        // Access StageDataSO from SolidGameManager
        if (GameManager.Instance != null && GameManager.Instance.currentStageData != null)
        {
            StageDataSO stageData = GameManager.Instance.currentStageData;
            currentLives = stageData.initialLives; // Initialize lives from StageDataSO
        }
        else
        {
            Debug.LogError("StageDataSO is not assigned in SolidGameManager!");
            currentLives = hearts.Length; // Fallback to the number of hearts
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

    private void NotifyGameManagerGameOver()
    {
        Debug.Log("Notifying GameManager of GameOver...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver); // Transition to GameOver state
        }
        else
        {
            Debug.LogError("SolidGameManager instance not found!");
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
    


}
 */

/* 
//Withoush Using SO
using UnityEngine;
using UnityEngine.UI;

public class LayfManager : MonoBehaviour
{
    public static LayfManager Instance; // Singleton instance
    public Image[] hearts; // Array of heart images
    public Sprite fullHeart; // Sprite for a full heart
    public Sprite emptyHeart; // Sprite for an empty heart
    public int currentLives; // Current number of lives


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


} */

using UnityEngine;
using UnityEngine.UI;

public class LayfManager : MonoBehaviour
{
    public Image[] hearts; // Array of heart images
    public Sprite fullHeart; // Sprite for a full heart
    public Sprite emptyHeart; // Sprite for an empty heart
    public int currentLives; // Current number of lives

    private void Start()
    {
        // Initialize lives to the number of hearts if not already set
        if (currentLives <= 0)
        {
            currentLives = hearts.Length;
        }

        // Apply additional lives purchased from the shop
        if (GameManager.Instance != null)
        {
            Debug.Log($"Applying {GameManager.Instance.additionalLives} stored lives from GameManager.");
            currentLives += GameManager.Instance.additionalLives; // Add purchased lives
            if (currentLives > hearts.Length)
            {
                currentLives = hearts.Length; // Cap lives at the maximum
            }
            GameManager.Instance.additionalLives = 0; // Reset additional lives after applying
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
                Debug.Log($"Heart {i}: Full");
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Show empty heart
                Debug.Log($"Heart {i}: Empty");
            }

            //  Debug.Log($"Heart {i}: {(i < currentLives ? "Full" : "Empty")}");
        }
        Canvas.ForceUpdateCanvases();
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
        Debug.Log($"AddLife called. Current lives after increment: {currentLives}");
    }
}