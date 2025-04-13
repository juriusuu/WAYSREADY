using UnityEngine;
using UnityEngine.UI;

public class AshInteractionManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    private GameObject currentAsh; // The ash object the player is near
    private int destroyedAshCount = 0; // Tracks the number of destroyed ash objects
    private float detectionRadius = 0.5f; // Distance threshold for detecting ash objects

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }
    }

    private void Update()
    {
        DetectAshObjects();
    }

    private void DetectAshObjects()
    {
        // Find all objects with the "Ash" tag
        GameObject[] ashObjects = GameObject.FindGameObjectsWithTag("Ash");
        bool isNearAnyAsh = false;

        // Check for player proximity to each ash object
        foreach (GameObject ash in ashObjects)
        {
            if (ash != null && IsPlayerNearAsh(ash))
            {
                currentAsh = ash; // Set the current ash object
                ShowInteractButton();
                isNearAnyAsh = true;
                break; // Exit the loop once a nearby ash is found
            }
        }

        // Hide the button if no ash object is nearby
        if (!isNearAnyAsh)
        {
            currentAsh = null;
            HideInteractButton();
        }
    }

    private bool IsPlayerNearAsh(GameObject ash)
    {
        // Check if the player is near the ash object (using a distance threshold)
        float distance = Vector3.Distance(ash.transform.position, transform.position);
        return distance <= detectionRadius; // Adjust the distance threshold as needed
    }

    private void ShowInteractButton()
    {
        if (interactButton != null && !interactButton.gameObject.activeSelf)
        {
            interactButton.gameObject.SetActive(true); // Show the button
        }
    }

    private void HideInteractButton()
    {
        if (interactButton != null && interactButton.gameObject.activeSelf)
        {
            interactButton.gameObject.SetActive(false); // Hide the button
        }
    }

    private void OnInteractButtonPressed()
    {
        if (currentAsh != null)
        {
            DestroyAsh(currentAsh);
        }
    }

    private void DestroyAsh(GameObject ash)
    {
        // Destroy the ash object
        Destroy(ash);
        destroyedAshCount++;
        Debug.Log($"Ash destroyed. Total destroyed: {destroyedAshCount}");

        // Clear the currentAsh reference
        currentAsh = null;

        int remainingAsh = GameObject.FindGameObjectsWithTag("Ash").Length - 1;
        Debug.Log($"Remaining ash objects: {remainingAsh}");
        if (remainingAsh == 0)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        var questManager = FindObjectOfType<QuestClipboardManagerS6>();
        if (questManager != null)
        {
            questManager.CompleteTask(0); // Task index 0
            Debug.Log("All ash objects destroyed. Quest 0 completed.");
        }
        else
        {
            Debug.LogError("QuestClipboardManagerS6 not found in the scene.");
        }
    }
}
