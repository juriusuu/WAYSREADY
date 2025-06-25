using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public GameObject winPanel;
    public TimeManager timeManager;
    private GoBagQuizManager goBagQuizManager; // Add reference to GoBagQuizManager

    private int correctItemCount = 0;
    private int requiredItemCount = 0;

    void Start()
    {
        // Check if GoBagQuizManager is present
        goBagQuizManager = FindFirstObjectByType<GoBagQuizManager>();

        if (goBagQuizManager != null)
        {
            Debug.Log("GoBagQuizManager found - ItemChecker will work in compatibility mode");
            // GoBagQuizManager will handle win conditions
            return;
        }

        // Original ItemChecker logic for scenes without GoBagQuizManager
        ItemData[] allItems = FindObjectsByType<ItemData>(FindObjectsSortMode.None);
        foreach (ItemData item in allItems)
        {
            if (item.isRequired)
                requiredItemCount++;
        }

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void ItemDropped(bool isCorrect)
    {
        // If GoBagQuizManager is present, let it handle the logic
        if (goBagQuizManager != null)
        {
            Debug.Log("GoBagQuizManager is handling item drops - ItemChecker skipping");
            return;
        }

        // Original ItemChecker logic for backward compatibility
        if (isCorrect)
        {
            correctItemCount++;
            CheckIfGameComplete();
        }
    }

    private void CheckIfGameComplete()
    {
        // Skip if GoBagQuizManager is handling game completion
        if (goBagQuizManager != null)
            return;

        if (requiredItemCount == 0)
        {
            Debug.LogWarning("No required items found! Check your scene setup.");
            return;
        }

        if (correctItemCount >= requiredItemCount)
        {
            Debug.Log("You win!");
            if (winPanel != null)
                winPanel.SetActive(true);

            if (timeManager != null)
                timeManager.StopTimer();
        }
    }

}

