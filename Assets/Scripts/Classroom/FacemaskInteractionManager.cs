using UnityEngine;
using UnityEngine.UI;

public class FacemaskInteractionManager : MonoBehaviour
{
    public Button facemaskButton; // Reference to the facemask button
    public GameObject facemaskPanel; // Reference to the panel that shows "You found the facemask"
    private bool isFacemaskFound = false; // Tracks if the facemask has already been found

    private void Start()
    {
        // Ensure the facemask button is hidden at the start
        if (facemaskButton != null)
        {
            facemaskButton.gameObject.SetActive(false);
            facemaskButton.onClick.AddListener(OnFacemaskButtonPressed); // Add listener for button click
        }

        // Ensure the facemask panel is hidden at the start
        if (facemaskPanel != null)
        {
            facemaskPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the facemask
        if (!isFacemaskFound && other.CompareTag("Player"))
        {
            Debug.Log("Player is near the facemask. Showing facemask button.");
            if (facemaskButton != null)
            {
                facemaskButton.gameObject.SetActive(true); // Show the facemask button
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the facemask button when the player leaves the trigger
        if (!isFacemaskFound && other.CompareTag("Player"))
        {
            Debug.Log("Player left the facemask area. Hiding facemask button.");
            if (facemaskButton != null)
            {
                facemaskButton.gameObject.SetActive(false); // Hide the facemask button
            }
        }
    }

    private void OnFacemaskButtonPressed()
    {
        if (isFacemaskFound)
        {
            Debug.Log("Facemask already found. Button press ignored.");
            return;
        }

        Debug.Log("Facemask button pressed. Showing facemask panel.");
        if (facemaskPanel != null)
        {
            facemaskPanel.SetActive(true); // Show the facemask panel
            StartCoroutine(HideFacemaskPanelAfterDelay()); // Start coroutine to hide the panel after 1.4 seconds
        }

        // Mark the facemask as found and disable the button
        isFacemaskFound = true;
        if (facemaskButton != null)
        {
            facemaskButton.gameObject.SetActive(false); // Hide the facemask button permanently
        }

        // Complete the task in QuestClipboardManagerS4
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(2);
        Debug.Log("Task 2 completed in QuestClipboardManagerS4.");
    }

    private System.Collections.IEnumerator HideFacemaskPanelAfterDelay()
    {
        yield return new WaitForSeconds(1.4f); // Wait for 1.4 seconds
        if (facemaskPanel != null)
        {
            facemaskPanel.SetActive(false); // Hide the facemask panel
            Debug.Log("Facemask panel hidden after 1.4 seconds.");
        }
    }
}