using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;
public class RadioInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager1 phoneButtonManager; // Reference to PhoneButtonManager1

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Ensure all panels are hidden at the start
        if (firstPanel != null) firstPanel.SetActive(false);
        if (secondPanel != null) secondPanel.SetActive(false);

        // Find the AudioSource on the radio GameObject
        radioAudioSource = GetComponent<AudioSource>();
        if (radioAudioSource == null)
        {
            Debug.LogError("No AudioSource found on the radio GameObject!");
        }
    }

    private void Update()
    {
        // Handle button visibility based on the current interactable and interaction state
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Radio"))
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
        // Check if the player is near the radio
        Debug.Log($"Trigger entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject; // Set the current interactable object
            Debug.Log($"Player is near the radio. CurrentInteractable set to: {currentInteractable.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the interactable object when the player leaves the trigger
        Debug.Log($"Trigger exited by: {other.name}");
        if (currentInteractable == gameObject && other.CompareTag("Player"))
        {
            currentInteractable = null;
            Debug.Log("Player left the radio.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (isInteractionComplete)
        {
            Debug.Log("Interaction already completed. Button press ignored.");
            return;
        }

        if (radioAudioSource != null)
        {
            // Play the radio sound
            if (!radioAudioSource.isPlaying)
            {
                radioAudioSource.Play();
                Debug.Log("Radio is now playing!");
                StartCoroutine(ShowPanelsInSequence());
            }
            else
            {
                Debug.Log("Radio is already playing.");
                StartCoroutine(ShowPanelsInSequence());
            }

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
        }
        else
        {
            Debug.LogError("Radio AudioSource is not set!");
        }
    }

    private System.Collections.IEnumerator ShowPanelsInSequence()
    {
        // Show the first panel
        if (firstPanel != null)
        {
            firstPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            firstPanel.SetActive(false);
        }

        // Show the second panel
        if (secondPanel != null)
        {
            secondPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            secondPanel.SetActive(false);

            // Complete the task after the second panel
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Task index 1
            Debug.Log("Task 1 completed in QuestClipboardManagerS6.");

            // Activate the phone button
            if (phoneButtonManager != null)
            {
                phoneButtonManager.ActivatePhoneButton();
                Debug.Log("Phone button activated.");
            }
        }
    }
}