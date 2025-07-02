using UnityEngine;
using UnityEngine.UI;

public class TelephoneInteractionManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels for the telephone interaction
    public AudioClip ringAudioClip; // Ring audio clip to play before the first panel
    public AudioClip[] telephoneAudioClips; // Array of audio clips for each panel
    public float ringAudioDuration = 2f; // Duration of the ring audio
    public float[] panelDisplayTimes; // Array of display times for each panel

    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed
    public PostTelephoneInteractionManager postTelephoneInteractionManager; // Reference to the PostTelephoneInteractionManager

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

        // Play the ring audio before starting the interaction
        if (ringAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(ringAudioClip, transform.position);
            Debug.Log("Playing ring audio.");
            StartCoroutine(PlayRingAudioAndShowPanelsInSequence());
        }
        else
        {
            Debug.LogError("Ring audio clip is not assigned!");
        }

        // Mark interaction as complete
        isInteractionComplete = true;
    }

    private System.Collections.IEnumerator PlayRingAudioAndShowPanelsInSequence()
    {
        // Wait for the ring audio to finish
        yield return new WaitForSeconds(ringAudioDuration);

        // Start showing panels and playing their corresponding audio clips
        Debug.Log("Starting panel and audio sequence...");
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null && telephoneAudioClips[i] != null)
            {
                // Show the panel
                Debug.Log($"Showing panel {i}: {panels[i].name}");
                panels[i].SetActive(true);

                // Play the corresponding audio clip
                AudioSource.PlayClipAtPoint(telephoneAudioClips[i], Camera.main.transform.position, 10f);
                Debug.Log($"Playing audio clip {i}: {telephoneAudioClips[i].name}");

                // Wait for the specified display time for this panel
                yield return new WaitForSeconds(panelDisplayTimes[i]);

                // Hide the panel
                panels[i].SetActive(false);
                Debug.Log($"Hiding panel {i}: {panels[i].name}");
            }
            else
            {
                Debug.LogWarning($"Panel or audio clip at index {i} is null. Skipping.");
            }
        }

        // Mark the "Answer the telephone" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(0); // Assuming this is the first task
            Debug.Log("Quest task 'Answer the telephone' marked as completed.");
        }
        else
        {
            Debug.LogError("QuestClipboardManager not found in the scene!");
        }

        // Trigger the PostTelephoneInteractionManager after the panels are done
        if (postTelephoneInteractionManager != null)
        {
            postTelephoneInteractionManager.ActivatePostInteraction();
            Debug.Log("PostTelephoneInteractionManager activated after telephone panels.");
        }
        else
        {
            Debug.LogError("PostTelephoneInteractionManager is not assigned!");
        }

        Debug.Log("Finished showing all panels and playing all audio clips.");

        // Mark interaction as complete and disable the button
        interactButton.gameObject.SetActive(false); // Hide the button permanently
    }
}