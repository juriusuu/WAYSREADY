using UnityEngine;
using UnityEngine.UI;

public class LocateTableInteraction : MonoBehaviour
{
    public Button locateButton; // Button to interact with the table
    public GameObject tablePanel; // Panel to display "You found the Table"
    private bool isTableFound = false; // Tracks if the table has already been found

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (locateButton != null)
        {
            locateButton.gameObject.SetActive(false);
            locateButton.onClick.AddListener(OnLocateButtonPressed); // Add listener for button click
        }

        // Ensure the panel is hidden at the start
        if (tablePanel != null)
        {
            tablePanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the table
        if (!isTableFound && other.CompareTag("Player"))
        {
            Debug.Log("Player is near the Table. Showing locate button.");
            if (locateButton != null)
            {
                locateButton.gameObject.SetActive(true); // Show the locate button
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the button when the player leaves the trigger
        if (!isTableFound && other.CompareTag("Player"))
        {
            Debug.Log("Player left the Table area. Hiding locate button.");
            if (locateButton != null)
            {
                locateButton.gameObject.SetActive(false); // Hide the locate button
            }
        }
    }

    private void OnLocateButtonPressed()
    {
        if (isTableFound)
        {
            Debug.Log("Table already located. Button press ignored.");
            return;
        }

        Debug.Log("Locate button pressed. Showing Table panel.");
        if (tablePanel != null)
        {
            tablePanel.SetActive(true); // Show the Table panel
            StartCoroutine(HideTablePanelAfterDelay()); // Hide the panel after a delay
        }

        // Mark the table as found
        isTableFound = true;

        if (locateButton != null)
        {
            locateButton.gameObject.SetActive(false); // Hide the locate button permanently
        }

        // Notify the quest manager
        FindObjectOfType<QuestClipboardManagerS5>()?.CompleteTask(2); // Task index 1
        Debug.Log("Table located. Task completed.");
    }

    private System.Collections.IEnumerator HideTablePanelAfterDelay()
    {
        yield return new WaitForSeconds(1.4f); // Wait for 1.4 seconds
        if (tablePanel != null)
        {
            tablePanel.SetActive(false); // Hide the Table panel
            Debug.Log("Table panel hidden after 1.4 seconds.");
        }
    }
}