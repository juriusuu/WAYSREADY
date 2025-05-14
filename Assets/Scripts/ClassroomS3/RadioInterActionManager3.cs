/* using UnityEngine;
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

            // Remove or disable the AudioSource after showing the panels
            if (radioAudioSource != null)
            {
                // Destroy(radioAudioSource); // Completely remove the AudioSource component
                // Alternatively, you can disable it instead:
                radioAudioSource.enabled = false;
                Debug.Log("AudioSource has been removed or disabled.");
            }
            // Activate the phone button
            if (phoneButtonManager != null)
            {
                phoneButtonManager.ActivatePhoneButton();
                Debug.Log("Phone button activated.");
            }
        }
    }
} */
/* 
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
        }

        // Complete the task after the second panel
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Task index 1
        Debug.Log("Task 1 completed in QuestClipboardManagerS6.");

        // Remove or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            radioAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("AudioSource has been removed or disabled.");
        }

        // Activate the phone button after all panels are shown
        if (phoneButtonManager != null)
        {
            phoneButtonManager.ActivatePhoneButton();
            Debug.Log("Phone button activated after panels are shown.");
        }
    }
} */
/* 
using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public AudioSource ringAudioSource; // Reference to the ring AudioSource
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager1 phoneButtonManager; // Reference to PhoneButtonManager1

    public PostRadio1 postRadio1; // Reference to the PostWhiteboardInteractionManager


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

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
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

        // Complete the task after the second panel
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Task index 1
        Debug.Log("Task 1 completed in QuestClipboardManagerS6.");

        // Remove or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            radioAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("AudioSource has been removed or disabled.");
        }
        // Add a 1-second delay before triggering the PostWhiteboardInteractionManager
        yield return new WaitForSeconds(0.5f);


        // Trigger the PostWhiteboardInteractionManager
        if (postRadio1 != null)
        {
            postRadio1.ActivatePostInteractionRadio1();
        }
        else
        {
            Debug.LogError("PostWhiteboardInteractionManager is not assigned!");
        }
        // Activate the phone button after all panels are shown
        if (phoneButtonManager != null)
        {
            phoneButtonManager.ActivatePhoneButton();
            Debug.Log("Phone button activated after panels are shown.");
        }
    }
} */




using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;
public class RadioInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public AudioClip audioClip1; // First audio clip
    public AudioClip audioClip2; // Second audio clip

    public PostRadio1 postRadio1; // Reference to the PostRadioInteractionManager
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
            Debug.Log($"Playing audio: {audioSource.clip.name}");
            yield return new WaitForSeconds(audioClip1.length); // Wait for the duration of the audio clip
            firstPanel.SetActive(false);
        }

        // Play the second audio clip and show the second panel
        if (audioClip2 != null && secondPanel != null)
        {
            secondPanel.SetActive(true);
            audioSource.clip = audioClip2;
            audioSource.Play();
            Debug.Log($"Playing audio: {audioSource.clip.name}");
            yield return new WaitForSeconds(audioClip2.length); // Wait for the duration of the audio clip
            secondPanel.SetActive(false);


            // Mark the task as completed
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Task index 1
            Debug.Log("Task 1 completed in QuestClipboardManager.");
        }

        Destroy(audioSource); // Remove the AudioSource after playing all clips

        // Trigger the PostRadioInteractionManager
        if (postRadio1 != null)
        {
            postRadio1.ActivatePostInteractionRadio1();
        }
        else
        {
            Debug.LogError("PostRadioInteractionManager is not assigned!");
        }

        // Activate the phone button after all panels are shown
        if (phoneButtonManager != null)
        {
            phoneButtonManager.ActivatePhoneButton();
            Debug.Log("Phone button activated after panels are shown.");
        }
    }
}