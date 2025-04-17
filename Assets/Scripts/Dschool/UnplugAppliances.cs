using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Required for HashSet<>
public class UnplugAppliancesTask : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    private GameObject currentAppliance; // The appliance the player is near
    private int interactedApplianceCount = 0; // Tracks the number of interacted appliances
    private int totalAppliances = 0; // Total number of appliances in the scene
    private HashSet<GameObject> interactedAppliances = new HashSet<GameObject>(); // Tracks appliances already interacted with
    [SerializeField]
    private float detectionRadius = 0.5f; // Distance threshold for detecting appliances

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Count all appliances in the scene with the tag "Unplug"
        totalAppliances = GameObject.FindGameObjectsWithTag("Unplug").Length;
        Debug.Log($"Total appliances in the scene: {totalAppliances}");
    }

    private void Update()
    {
        DetectAppliances();
    }

    private void DetectAppliances()
    {
        GameObject nearestAppliance = null;
        float closestDistance = detectionRadius;

        // Find the nearest appliance within the detection radius
        foreach (GameObject appliance in GameObject.FindGameObjectsWithTag("Unplug"))
        {
            if (appliance != null && !interactedAppliances.Contains(appliance))
            {
                float distance = Vector3.Distance(appliance.transform.position, transform.position);
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    nearestAppliance = appliance;
                }
            }
        }

        // Update the current appliance and button visibility
        if (nearestAppliance != null)
        {
            currentAppliance = nearestAppliance;
            ShowInteractButton();
        }
        else
        {
            currentAppliance = null;
            HideInteractButton();
        }
    }

    private void ShowInteractButton()
    {
        if (interactButton != null && !interactButton.gameObject.activeSelf)
        {
            Debug.Log($"Showing interact button for {currentAppliance.name}");
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
        if (currentAppliance != null)
        {
            InteractWithAppliance(currentAppliance);
        }
        else
        {
            Debug.LogWarning("Interact button pressed, but no appliance is nearby!");
        }
    }

    private void InteractWithAppliance(GameObject appliance)
    {
        // Mark the appliance as interacted with
        if (!interactedAppliances.Contains(appliance))
        {
            interactedAppliances.Add(appliance);
            interactedApplianceCount++;
            Debug.Log($"Interacted with appliance: {appliance.name}. Progress: {interactedApplianceCount}/{totalAppliances}");
        }

        // Clear the currentAppliance reference
        currentAppliance = null;

        // Hide the button after interaction
        HideInteractButton();

        // Check if all appliances have been interacted with
        if (interactedApplianceCount >= totalAppliances)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(0); // Task index 0
            Debug.Log("All appliances interacted with. Quest 0 completed.");
        }
        else
        {
            Debug.LogError("QuestClipboardManager not found in the scene.");
        }
    }
}