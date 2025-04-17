using UnityEngine;

public class GrabEmergencyKit : MonoBehaviour
{
    private bool isTaskCompleted = false; // Tracks if the task is completed
    public GameObject interactButton; // Reference to the interact button UI

    private void Start()
    {
        // Ensure the interact button is hidden at the start
        if (interactButton != null)
        {
            interactButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player") && !isTaskCompleted)
        {
            // Show the interact button
            if (interactButton != null)
            {
                interactButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the interact button when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }
        }
    }

    public void GrabKit()
    {
        if (!isTaskCompleted)
        {
            isTaskCompleted = true;

            // Notify the QuestClipboardManager to complete the task
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(2); // Task index 2 for "Grab the Emergency Kit"
                Debug.Log("Task 2: Grab the Emergency Kit completed.");
            }
            else
            {
                Debug.LogError("QuestClipboardManager not found in the scene.");
            }

            // Hide the interact button
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }

            // Optionally, destroy the emergency kit object
            Destroy(gameObject);
        }
    }
}