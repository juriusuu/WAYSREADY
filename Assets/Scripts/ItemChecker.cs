using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    public GameObject winPanel;
    public TimeManager timeManager;

    private int correctItemCount = 0;
    private int requiredItemCount = 0;
    public GoBagQuizManager quizManager; // Add this reference
    void Start()
    {// Use the items array from the quiz manager
        /*  ItemData[] allItems = FindObjectsOfType<ItemData>();
         foreach (ItemData item in allItems)
         {
             if (item.isRequired)
                 requiredItemCount++;
         } */
        // Use the items array from the quiz manager
        if (quizManager != null && quizManager.items != null)
        {
            foreach (ItemData item in quizManager.items)
            {
                if (item != null && item.isRequired)
                    requiredItemCount++;
            }
        }
        else
        {
            Debug.LogError("QuizManager or its items array is not assigned in ItemChecker!");
        }
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void ItemDropped(bool isCorrect)
    {
        if (isCorrect)
        {
            correctItemCount++;
            CheckIfGameComplete();
        }
    }

    private void CheckIfGameComplete()
    {
        if (correctItemCount >= requiredItemCount)
        {
            Debug.Log("You win!");
            if (winPanel != null)
                winPanel.SetActive(true);

            /*  if (timeManager != null)
                 timeManager.StopTimer();
             timeManager.EndGame(true); // <-- Add this line */

            if (timeManager != null)
                timeManager.StopTimer();
            if (timeManager != null)
                timeManager.EndGame(true);

        }
    }
}

