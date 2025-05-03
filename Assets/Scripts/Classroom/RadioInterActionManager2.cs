using UnityEngine;
using UnityEngine.UI;

public class RadioInteractionManagerS2 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public GameObject thirdPanel; // Reference to the third panel
    public AudioSource ringAudioSource; // Reference to the ring AudioSource
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
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
        if (firstPanel != null) firstPanel.SetActive(false);
        if (secondPanel != null) secondPanel.SetActive(false);
        if (thirdPanel != null) thirdPanel.SetActive(false);

        // Find the AudioSource on the radio GameObject
        radioAudioSource = GetComponent<AudioSource>();
        if (radioAudioSource == null)
        {
            Debug.LogError("No AudioSource found on the radio GameObject!");
        }

        // Ensure the ring AudioSource is assigned
        if (ringAudioSource == null)
        {
            Debug.LogError("Ring AudioSource is not assigned in the Inspector!");
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

        // Play the ring sound for 1 second
        if (ringAudioSource != null)
        {
            ringAudioSource.Play();
            Debug.Log("Ring sound started!");
            StartCoroutine(StopRingAudioAfterDelay(1f));
        }
        else
        {
            Debug.LogError("Ring AudioSource is not set!");
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

            // Mark the "Turn on the radio" task as completed
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Assuming this is the fourth task

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Interaction completed. Button is now inactive.");
        }
        else
        {
            Debug.LogError("Radio AudioSource is not set!");
        }
    }

    private System.Collections.IEnumerator StopRingAudioAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Stop the ring sound
        if (ringAudioSource != null)
        {
            ringAudioSource.Stop();
            Debug.Log("Ring sound stopped after 1 second.");
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
        }

        // Show the third panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            thirdPanel.SetActive(false);
        }

        // Remove or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            radioAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("AudioSource has been removed or disabled.");
        }
    }
}

/* using UnityEngine;
using UnityEngine.UI;

public class RadioInteractionManagerS2 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public GameObject thirdPanel; // Reference to the third panel
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
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
        if (firstPanel != null) firstPanel.SetActive(false);
        if (secondPanel != null) secondPanel.SetActive(false);
        if (thirdPanel != null) thirdPanel.SetActive(false);

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

            // Mark the "Turn on the radio" task as completed
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Assuming this is the fourth task

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Interaction completed. Button is now inactive.");
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
        }

        // Show the third panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            thirdPanel.SetActive(false);
        }
        // Remove or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            // Destroy(radioAudioSource); // Completely remove the AudioSource component
            // Alternatively, you can disable it instead:
            radioAudioSource.enabled = false;
            Debug.Log("AudioSource has been removed or disabled.");
        }
    }
} */