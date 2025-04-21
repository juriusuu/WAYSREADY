using UnityEngine;

public class FindEscapeRouteTask1 : MonoBehaviour
{
    private bool isTaskCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player") && !isTaskCompleted)
        {
            isTaskCompleted = true;

            // Notify the QuestClipboardManager to complete the tasks
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(2); // Task index 2
                Debug.Log("Tasks 1 and 2 completed.");
            }
            else
            {
                Debug.LogError("QuestClipboardManager not found in the scene.");
            }
        }
    }
}