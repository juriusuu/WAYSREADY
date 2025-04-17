using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Required for HashSet<>

public class FireExtinguishersTask : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    private GameObject currentExtinguisher; // The fire extinguisher the player is near
    private int interactedExtinguisherCount = 0; // Tracks the number of interacted extinguishers
    private int totalExtinguishers = 0; // Total number of extinguishers in the scene
    private HashSet<GameObject> interactedExtinguishers = new HashSet<GameObject>(); // Tracks extinguishers already interacted with

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Count all fire extinguishers in the scene with the tag "FireExtinguisher"
        totalExtinguishers = GameObject.FindGameObjectsWithTag("FireExtinguisher").Length;
        Debug.Log($"Total fire extinguishers in the scene: {totalExtinguishers}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}");
        if (other.CompareTag("FireExtinguisher") && !interactedExtinguishers.Contains(other.gameObject))
        {
            currentExtinguisher = other.gameObject;
            ShowInteractButton();
            Debug.Log($"Player is near fire extinguisher: {currentExtinguisher.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited by: {other.gameObject.name}");
        if (currentExtinguisher == other.gameObject)
        {
            currentExtinguisher = null;
            HideInteractButton();
            Debug.Log($"Player left fire extinguisher: {other.gameObject.name}");
        }
    }

    private void ShowInteractButton()
    {
        if (interactButton != null && !interactButton.gameObject.activeSelf)
        {
            Debug.Log($"Showing interact button for {currentExtinguisher.name}");
            interactButton.gameObject.SetActive(true); // Show the button
        }
    }

    private void HideInteractButton()
    {
        if (interactButton != null && interactButton.gameObject.activeSelf)
        {
            Debug.Log("Hiding interact button");
            interactButton.gameObject.SetActive(false); // Hide the button
        }
    }

    private void OnInteractButtonPressed()
    {
        if (currentExtinguisher != null)
        {
            InteractWithExtinguisher(currentExtinguisher);
        }
        else
        {
            Debug.LogWarning("Interact button pressed, but no extinguisher is nearby!");
        }
    }

    private void InteractWithExtinguisher(GameObject extinguisher)
    {
        // Mark the extinguisher as interacted with
        if (!interactedExtinguishers.Contains(extinguisher))
        {
            interactedExtinguishers.Add(extinguisher);
            interactedExtinguisherCount++;
            Debug.Log($"Interacted with extinguisher: {extinguisher.name}. Progress: {interactedExtinguisherCount}/{totalExtinguishers}");
        }

        // Clear the currentExtinguisher reference
        currentExtinguisher = null;

        // Hide the button after interaction
        HideInteractButton();

        // Check if all extinguishers have been interacted with
        if (interactedExtinguisherCount >= totalExtinguishers)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1); // Task index 1
            Debug.Log("All fire extinguishers interacted with. Quest 1 completed.");
        }
        else
        {
            Debug.LogError("QuestClipboardManager not found in the scene.");
        }
    }
}