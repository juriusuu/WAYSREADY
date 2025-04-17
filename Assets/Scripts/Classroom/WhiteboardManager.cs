using UnityEngine;
using UnityEngine.UI;

public class WhiteboardManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels (9 panels for the whiteboard interaction)
    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Ensure all panels are hidden at the start
        if (panels != null)
        {
            foreach (var panel in panels)
            {
                if (panel != null) panel.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Handle button visibility based on the current interactable and interaction state
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Whiteboard"))
        {
            if (!interactButton.gameObject.activeSelf)
            {
                Debug.Log("Showing interact button.");
            }
            interactButton.gameObject.SetActive(true); // Show the button
        }
        else
        {
            if (interactButton.gameObject.activeSelf)
            {
                Debug.Log("Hiding interact button.");
            }
            interactButton.gameObject.SetActive(false); // Hide the button
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the whiteboard
        Debug.Log($"Trigger entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject; // Set the current interactable object
            Debug.Log($"Player is near the whiteboard. CurrentInteractable set to: {currentInteractable.name}");
        }
        else if (!other.CompareTag("Whiteboard"))
        {
            // Log only non-whiteboard and non-player objects
            Debug.Log($"Trigger entered by non-whiteboard and non-player object: {other.name}, Tag: {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the interactable object when the player leaves the trigger
        Debug.Log($"Trigger exited by: {other.name}");
        if (currentInteractable == gameObject && other.CompareTag("Player"))
        {
            currentInteractable = null;
            Debug.Log("Player left the whiteboard.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (isInteractionComplete)
        {
            Debug.Log("Interaction already completed. Button press ignored.");
            return;
        }

        StartCoroutine(ShowPanelsInSequence());

        // Mark interaction as complete and disable the button
        isInteractionComplete = true;
        interactButton.gameObject.SetActive(false); // Hide the button permanently
        Debug.Log("Whiteboard interaction completed.");
    }

    private System.Collections.IEnumerator ShowPanelsInSequence()
    {
        // Show each panel in sequence
        foreach (var panel in panels)
        {
            if (panel != null)
            {
                panel.SetActive(true);
                yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
                panel.SetActive(false);
            }
        }

        // Mark the task as completed after showing all panels
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(0);
        Debug.Log("Task 0 completed in QuestClipboardManagerS4.");
    }
}