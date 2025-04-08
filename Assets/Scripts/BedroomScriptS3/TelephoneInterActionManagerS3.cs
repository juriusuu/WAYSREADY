using UnityEngine;
using UnityEngine.UI;

public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels (9 panels for the telephone interaction)
    private GameObject currentInteractable; // The object the player is near
    private AudioSource telephoneAudioSource; // Reference to the telephone's AudioSource
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

        // Find the AudioSource on the telephone GameObject
        telephoneAudioSource = GetComponent<AudioSource>();
        if (telephoneAudioSource == null)
        {
            Debug.LogError("No AudioSource found on the telephone GameObject!");
        }
    }

    private void Update()
    {
        // Handle button visibility based on the current interactable and interaction state
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Telephone"))
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
        // Check if the player is near the telephone
        Debug.Log($"Trigger entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject; // Set the current interactable object
            Debug.Log($"Player is near the telephone. CurrentInteractable set to: {currentInteractable.name}");
        }
        else if (!other.CompareTag("Telephone"))
        {
            // Log only non-telephone and non-player objects
            Debug.Log($"Trigger entered by non-telephone and non-player object: {other.name}, Tag: {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the interactable object when the player leaves the trigger
        Debug.Log($"Trigger exited by: {other.name}");
        if (currentInteractable == gameObject && other.CompareTag("Player"))
        {
            currentInteractable = null;
            Debug.Log("Player left the telephone.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (isInteractionComplete)
        {
            Debug.Log("Interaction already completed. Button press ignored.");
            return;
        }

        if (telephoneAudioSource != null)
        {
            // Play the telephone sound
            if (!telephoneAudioSource.isPlaying)
            {
                telephoneAudioSource.Play();
                Debug.Log("Telephone is now playing!");
                StartCoroutine(ShowPanelsInSequence());
            }
            else
            {
                Debug.Log("Telephone is already playing.");
                StartCoroutine(ShowPanelsInSequence());
            }

            // Mark the "Answer the telephone" task as completed
            FindObjectOfType<QuestClipboardManagerS2>()?.CompleteTask(1); // Assuming this is the first task

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Interaction completed. Button is now inactive.");
        }
        else
        {
            Debug.LogError("Telephone AudioSource is not set!");
        }
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
    }
}