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
        UpdateHeartsUI(); // Initialize the heart UI
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--; // Decrease the player's life
            UpdateHeartsUI(); // Update the heart UI
        }

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            GameManager.Instance.HandleGameOver(); // Notify the GameManager
        }
    }

    private void UpdateHeartsUI()
    {
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
        return currentLives; // Replace with your actual variable for remaining lives
    }
}