using UnityEngine;
using UnityEngine.UI;

public class RadioInteractionManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public GameObject thirdPanel; // Reference to the third panel
    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public AudioClip audioClip1; // First audio clip
    public AudioClip audioClip2; // Second audio clip
    public AudioClip audioClip3; // Third audio clip

    public PostRadioInteractionManager postRadioInteractionManager; // Reference to the PostRadioInteractionManager

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
        if (thirdPanel != null) thirdPanel.SetActive(false);
    }

    private void Update()
    {
        // Handle button visibility based on the current interactable and interaction state
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Radio"))
        {
            interactButton.gameObject.SetActive(true); // Show the button
        }
        else
        {
            interactButton.gameObject.SetActive(false); // Hide the button
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject; // Set the current interactable object
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable == gameObject && other.CompareTag("Player"))
        {
            currentInteractable = null;
        }
    }

    private void OnInteractButtonPressed()
    {
        if (isInteractionComplete)
        {
            return;
        }

        // Play the audio clips and show panels in sequence
        StartCoroutine(PlayAudioAndShowPanelsInSequence());

        // Mark the "Turn on the radio" task as completed
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(2); // Assuming this is the fourth task

        // Mark interaction as complete and disable the button
        isInteractionComplete = true;
        interactButton.gameObject.SetActive(false); // Hide the button permanently
    }

    private System.Collections.IEnumerator PlayAudioAndShowPanelsInSequence()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource dynamically

        // Play the first audio clip and show the first panel
        if (audioClip1 != null && firstPanel != null)
        {
            firstPanel.SetActive(true);
            audioSource.clip = audioClip1;
            audioSource.Play();
            yield return new WaitForSeconds(audioClip1.length);
            firstPanel.SetActive(false);
        }

        // Play the second audio clip and show the second panel
        if (audioClip2 != null && secondPanel != null)
        {
            secondPanel.SetActive(true);
            audioSource.clip = audioClip2;
            audioSource.Play();
            yield return new WaitForSeconds(audioClip2.length);
            secondPanel.SetActive(false);
        }

        // Play the third audio clip and show the third panel
        if (audioClip3 != null && thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            audioSource.clip = audioClip3;
            audioSource.Play();
            yield return new WaitForSeconds(audioClip3.length);
            thirdPanel.SetActive(false);
        }

        Destroy(audioSource); // Remove the AudioSource after playing all clips

        // Trigger the PostRadioInteractionManager
        if (postRadioInteractionManager != null)
        {
            postRadioInteractionManager.ActivatePostRadioInteraction();
        }
        else
        {
            Debug.LogError("PostRadioInteractionManager is not assigned!");
        }
    }
}
