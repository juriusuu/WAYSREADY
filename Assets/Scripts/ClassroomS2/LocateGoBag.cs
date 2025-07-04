using UnityEngine;
using UnityEngine.UI;

public class LocateGoBagInteraction : MonoBehaviour
{
    public Button locateButton; // Button to interact with the bag
    public GameObject goBagPanel; // Panel to display "You found the Go Bag"
    public GameObject inventoryOpenButton; // Reference to the InventoryOpenButton
    private bool isBagFound = false; // Tracks if the bag has already been found
    public PostGoBagInteractionManager postGoBagInteractionManager; // Reference to the PostGoBagInteractionManager
    private void Start()
    {
        // Ensure the locate button is hidden at the start
        if (locateButton != null)
        {
            locateButton.gameObject.SetActive(false);
            locateButton.onClick.AddListener(OnLocateButtonPressed); // Add listener for button click
        }

        // Ensure the panel is hidden at the start
        if (goBagPanel != null)
        {
            goBagPanel.SetActive(false);
        }

        // Ensure the InventoryOpenButton is hidden at the start
        if (inventoryOpenButton != null)
        {
            inventoryOpenButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the bag
        if (!isBagFound && other.CompareTag("Player"))
        {
            Debug.Log("Player is near the Go Bag. Showing locate button.");
            if (locateButton != null)
            {
                locateButton.gameObject.SetActive(true); // Show the locate button
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the locate button when the player leaves the trigger
        if (!isBagFound && other.CompareTag("Player"))
        {
            Debug.Log("Player left the Go Bag area. Hiding locate button.");
            if (locateButton != null)
            {
                locateButton.gameObject.SetActive(false); // Hide the locate button
            }
        }
    }

    private void OnLocateButtonPressed()
    {
        if (isBagFound)
        {
            Debug.Log("Go Bag already located. Button press ignored.");
            return;
        }

        Debug.Log("Locate button pressed. Showing Go Bag panel.");

        // Show the Go Bag panel
        if (goBagPanel != null)
        {
            goBagPanel.SetActive(true);
            StartCoroutine(HideGoBagPanelAfterDelay()); // Hide the panel after a delay
        }

        // Activate the InventoryOpenButton
        if (inventoryOpenButton != null)
        {
            inventoryOpenButton.SetActive(true); // Make the InventoryOpenButton active
            Debug.Log("InventoryOpenButton is now active.");
        }

        // Mark the bag as found
        isBagFound = true;

        // Hide the locate button permanently
        if (locateButton != null)
        {
            locateButton.gameObject.SetActive(false);
        }

        // Notify the quest manager
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(0); // Task index 0
        Debug.Log("Go Bag located. Task completed.");


        // Trigger the PostGoBagInteractionManager
        if (postGoBagInteractionManager != null)
        {
            postGoBagInteractionManager.ActivatePostInteraction();
        }
        else
        {
            Debug.LogError("PostGoBagInteractionManager is not assigned!");
        }
    }

    private System.Collections.IEnumerator HideGoBagPanelAfterDelay()
    {
        yield return new WaitForSeconds(1.4f); // Wait for 1.4 seconds
        if (goBagPanel != null)
        {
            goBagPanel.SetActive(false); // Hide the Go Bag panel
            Debug.Log("Go Bag panel hidden after 1.4 seconds.");
        }
    }
}