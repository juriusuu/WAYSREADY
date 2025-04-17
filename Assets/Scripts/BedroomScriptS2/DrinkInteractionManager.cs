using UnityEngine;
using UnityEngine.UI;

public class DrinkInteractionManager : MonoBehaviour
{
    public Button drinkButton; // Reference to the drink button
    public Animator playerAnimator; // Reference to the player's Animator
    private GameObject currentInteractable; // The object the player is near
    private bool isDrinking = false; // Tracks if the player is currently drinking
    private bool isTaskComplete = false; // Tracks if the task is already completed

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (drinkButton != null)
        {
            drinkButton.gameObject.SetActive(false);
            drinkButton.onClick.AddListener(OnDrinkButtonPressed); // Add listener for button click
        }
    }

    private void Update()
    {
        // Handle button visibility based on the current interactable and task completion state
        if (!isTaskComplete && !isDrinking && currentInteractable != null && currentInteractable.CompareTag("Table"))
        {
            if (!drinkButton.gameObject.activeSelf)
            {
                Debug.Log("Showing drink button.");
            }
            drinkButton.gameObject.SetActive(true); // Show the button
        }
        else
        {
            if (drinkButton.gameObject.activeSelf)
            {
                Debug.Log("Hiding drink button.");
            }
            drinkButton.gameObject.SetActive(false); // Hide the button
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the table
        Debug.Log($"Trigger entered by: {other.name}");
        if (other.CompareTag("Table"))
        {
            currentInteractable = other.gameObject; // Set the current interactable object
            Debug.Log($"Player is near the table. CurrentInteractable set to: {currentInteractable.name}");
        }
        else
        {
            Debug.Log($"Trigger entered by non-table object: {other.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the interactable object when the player leaves the trigger
        Debug.Log($"Trigger exited by: {other.name}");
        if (currentInteractable == other.gameObject && other.CompareTag("Table"))
        {
            currentInteractable = null;
            Debug.Log("Player left the table.");
        }
    }

    private void OnDrinkButtonPressed()
    {
        if (isDrinking || isTaskComplete)
        {
            Debug.Log("Player is already drinking or task is completed. Button press ignored.");
            return;
        }

        if (playerAnimator != null)
        {
            // Trigger the drinking animation
            playerAnimator.SetTrigger("Drink");
            Debug.Log("Player is drinking.");
            isDrinking = true;

            // Disable the button while drinking
            drinkButton.gameObject.SetActive(false);

            // Mark the task as completed in QuestClipboardManagerS2
            QuestClipboardManager questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(1); // Mark the second task (index 1) as completed
                Debug.Log("Second task completed in QuestClipboardManagerS2.");
            }
            else
            {
                Debug.LogError("QuestClipboardManagerS2 not found!");
            }

            // Mark the task as complete and reset the drinking state after the animation finishes
            isTaskComplete = true;
            StartCoroutine(ResetDrinkingState());
        }
        else
        {
            Debug.LogError("Player Animator is not set!");
        }
    }

    private System.Collections.IEnumerator ResetDrinkingState()
    {
        // Wait for the duration of the animation (adjust the time as needed)
        yield return new WaitForSeconds(2.0f); // Assuming the drinking animation is 2 seconds long
        isDrinking = false;
        Debug.Log("Player finished drinking.");

        // The button will remain inactive since the task is complete
        Debug.Log("Drink button will remain inactive as the task is completed.");
    }
}