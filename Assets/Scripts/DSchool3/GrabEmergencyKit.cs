using UnityEngine;

public class GrabEmergencyKit : MonoBehaviour
{
    private bool isTaskCompleted = false; // Tracks if the task is completed
    public GameObject interactButton; // Reference to the interact button UI

    public PostEmergencyKitInteractionManager postEmergencyKitInteractionManager; // Reference to the PostEmergencyKitInteractionManager

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
        // Check if the player enters the trigger zone and the object has the "MedKit" tag
        if (other.CompareTag("MedKit") && !isTaskCompleted)
        {
            // Show the interact button
            if (interactButton != null)
            {
                interactButton.SetActive(true);
            }

            Debug.Log("Player is near the Med Kit.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the interact button when the player leaves the trigger zone
        if (other.CompareTag("MedKit"))
        {
            if (interactButton != null)
            {
                interactButton.SetActive(false);
            }

            Debug.Log("Player left the Med Kit area.");
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
                questManager.CompleteTask(0); // Task index 0 for "Grab the Emergency Kit"
                Debug.Log("Task 0: Grab the Emergency Kit completed.");
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

            // Trigger the PostEmergencyKitInteractionManager
            if (postEmergencyKitInteractionManager != null)
            {
                postEmergencyKitInteractionManager.ActivatePostInteraction();
            }
            else
            {
                Debug.LogError("PostEmergencyKitInteractionManager is not assigned!");
            }
            // Optionally, destroy the emergency kit object
            Destroy(gameObject);
        }
    }
}