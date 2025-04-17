using UnityEngine;

public class FindEscapeRouteTask : MonoBehaviour
{
    private bool isTaskCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player") && !isTaskCompleted)
        {
            isTaskCompleted = true;

            // Notify the QuestClipboardManager to complete the task
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(0); // Task index 0 for "Find the Escape Route"
                Debug.Log("Task 0: Find the Escape Route completed.");
            }
            else
            {
                Debug.LogError("QuestClipboardManager not found in the scene.");
            }
        }
    }
}