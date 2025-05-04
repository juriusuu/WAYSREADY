using UnityEngine;

public class FindEscapeRouteTask : MonoBehaviour
{
    private bool isTaskCompleted = false;
    public EscapeRoutePanelManager escapeRoutePanelManager; // Reference to the EscapeRoutePanelManager
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
                questManager.CompleteTask(1); // Task index 1
                questManager.CompleteTask(2); // Task index 2
                Debug.Log("Tasks 1 and 2 completed.");
            }
            else
            {
                Debug.LogError("QuestClipboardManager not found in the scene.");
            }

            // Trigger the EscapeRoutePanelManager
            if (escapeRoutePanelManager != null)
            {
                escapeRoutePanelManager.ShowEscapeRoutePanel();
            }
            else
            {
                Debug.LogError("EscapeRoutePanelManager is not assigned!");
            }
        }
    }
}