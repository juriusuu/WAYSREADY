using UnityEngine;
using UnityEngine.UI;

public class LocateGoBagInteraction : MonoBehaviour
{
    public Button locateButton; // Button to interact with the bag
    public GameObject goBagPanel; // Panel to display "You found the Go Bag"
    private bool isBagFound = false; // Tracks if the bag has already been found

    private void Start()
    {
        // Ensure the button is hidden at the start
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
        // Hide the button when the player leaves the trigger
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
        if (goBagPanel != null)
        {
            goBagPanel.SetActive(true); // Show the Go Bag panel
            StartCoroutine(HideGoBagPanelAfterDelay()); // Hide the panel after a delay
        }

        // Mark the bag as found
        isBagFound = true;

        if (locateButton != null)
        {
            locateButton.gameObject.SetActive(false); // Hide the locate button permanently
        }

        // Notify the quest manager
        FindObjectOfType<QuestClipboardManagerS5>()?.CompleteTask(0); // Task index 0
        Debug.Log("Go Bag located. Task completed.");
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