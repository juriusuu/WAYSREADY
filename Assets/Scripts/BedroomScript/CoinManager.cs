using UnityEngine;
using UnityEngine.UI;

public class CoinUIManager : MonoBehaviour
{
    public static CoinUIManager Instance; // Singleton instance
    public TMPro.TextMeshProUGUI coinText; // Reference to the Text component for displaying coins

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    public void UpdateCoinUI(int coinCount)
    {
        if (coinText != null)
        {
            coinText.text = $"{coinCount}"; // Update the coin count text
        }
        else
        {
            Debug.LogWarning("Coin Text is not assigned in the CoinUIManager.");
        }
    }
}