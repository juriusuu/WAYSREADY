using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS4 : MonoBehaviour
{
    public Button interactButton;
    public GameObject firstPanel;
    public GameObject secondPanel;
    public GameObject thirdPanel;
    public AudioClip audioClip1; // First panel audio
    public AudioClip audioClip2; // Second panel audio
    public AudioClip audioClip3; // Third panel audio
    private GameObject currentInteractable;
    private bool isInteractionComplete = false;
    public PostRadioInteractionManagerH postRadioInteractionManager;
    private bool isPhoneButtonPressed = false;

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed);
        }

        if (firstPanel != null) firstPanel.SetActive(false);
        if (secondPanel != null) secondPanel.SetActive(false);
        if (thirdPanel != null) thirdPanel.SetActive(false);
    }

    public void ActivateRadioButton()
    {
        if (interactButton != null && !isPhoneButtonPressed)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Radio button is now active.");
        }
        else if (isPhoneButtonPressed)
        {
            Debug.LogWarning("Radio button is already pressed. Cannot activate again.");
        }
        else
        {
            Debug.LogError("Radio button is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Radio"))
        {
            interactButton.gameObject.SetActive(true);
        }
        else
        {
            interactButton.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject;
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

        StartCoroutine(PlayAudioAndShowPanelsInSequence());

        isInteractionComplete = true;
        interactButton.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator PlayAudioAndShowPanelsInSequence()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        // First panel
        if (audioClip1 != null && firstPanel != null)
        {
            firstPanel.SetActive(true);
            audioSource.clip = audioClip1;
            audioSource.Play();
            Debug.Log($"Playing audio: {audioSource.clip.name}");
            yield return new WaitForSeconds(audioClip1.length);
            firstPanel.SetActive(false);
        }

        // Second panel
        if (audioClip2 != null && secondPanel != null)
        {
            secondPanel.SetActive(true);
            audioSource.clip = audioClip2;
            audioSource.Play();
            Debug.Log($"Playing audio: {audioSource.clip.name}");
            yield return new WaitForSeconds(audioClip2.length);
            secondPanel.SetActive(false);
        }

        // Third panel
        if (audioClip3 != null && thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            audioSource.clip = audioClip3;
            audioSource.Play();
            Debug.Log($"Playing audio: {audioSource.clip.name}");
            yield return new WaitForSeconds(audioClip3.length);
            thirdPanel.SetActive(false);
        }

        Destroy(audioSource);

        // Mark the "Use the radio" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1);
            Debug.Log("Quest task 'Use the radio' marked as completed.");
        }
        else
        {
            Debug.LogWarning("QuestClipboardManager not found in the scene.");
        }

        // Trigger the PostRadioInteractionManager
        if (postRadioInteractionManager != null)
        {
            postRadioInteractionManager.ActivatePostInteractionRad();
        }
        else
        {
            Debug.LogError("PostRadioInteractionManager is not assigned!");
        }
    }
}

/* using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS4 : MonoBehaviour
{
    public Button interactButton;
    public GameObject firstPanel;
    public GameObject secondPanel;
    public GameObject thirdPanel;
   public AudioSource audioSource; // Assign this in the Inspector (can be on this GameObject)
    public AudioClip ringAudioClip;
    public AudioClip firstPanelClip;
    public AudioClip secondPanelClip;
    public AudioClip thirdPanelClip;
    private GameObject currentInteractable;
    private bool isInteractionComplete = false;
    public PostRadioInteractionManagerH postRadioInteractionManager;
    private bool isPhoneButtonPressed = false;

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
        }

        if (firstPanel != null) firstPanel.SetActive(false);
        if (secondPanel != null) secondPanel.SetActive(false);
        if (thirdPanel != null) thirdPanel.SetActive(false);

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the Inspector!");
        }
        if (ringAudioClip == null)
        {
            Debug.LogError("Ring AudioClip is not assigned in the Inspector!");
        }
    }

    public void ActivateRadioButton()
    {
        if (interactButton != null && !isPhoneButtonPressed)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Radio button is now active.");
        }
        else if (isPhoneButtonPressed)
        {
            Debug.LogWarning("Radio button is already pressed. Cannot activate again.");
        }
        else
        {
            Debug.LogError("Radio button is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        if (!isInteractionComplete && currentInteractable != null && currentInteractable.CompareTag("Radio"))
        {
            if (!interactButton.gameObject.activeSelf)
            {
                Debug.Log("Showing interact button.");
            }
            interactButton.gameObject.SetActive(true);
        }
        else
        {
            if (interactButton.gameObject.activeSelf)
            {
                Debug.Log("Hiding interact button.");
            }
            interactButton.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            currentInteractable = gameObject;
            Debug.Log($"Player is near the radio. CurrentInteractable set to: {currentInteractable.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
        if (audioSource != null && ringAudioClip != null)
        {
            audioSource.clip = ringAudioClip;
            audioSource.Play();
            Debug.Log("Ring sound started!");
            StartCoroutine(StopAudioAfterDelay(1f));
        }
        else
        {
            Debug.LogError("AudioSource or Ring AudioClip is not set!");
        }

        // Start the panel and audio sequence
        StartCoroutine(ShowPanelsAndPlayClipsInSequence());

        isInteractionComplete = true;
        interactButton.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null)
        {
            audioSource.Stop();
            Debug.Log("Audio stopped after delay.");
        }
    }

    private System.Collections.IEnumerator ShowPanelsAndPlayClipsInSequence()
    {
        // First panel
        if (firstPanel != null)
        {
            firstPanel.SetActive(true);
            if (audioSource != null && firstPanelClip != null)
            {
                audioSource.clip = firstPanelClip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(11.5f);
            firstPanel.SetActive(false);
        }

        // Second panel
        if (secondPanel != null)
        {
            secondPanel.SetActive(true);
            if (audioSource != null && secondPanelClip != null)
            {
                audioSource.clip = secondPanelClip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(6.5f);
            secondPanel.SetActive(false);
        }

        // Third panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            if (audioSource != null && thirdPanelClip != null)
            {
                audioSource.clip = thirdPanelClip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(4f);
            thirdPanel.SetActive(false);
        }

        // Mark the "Use the radio" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1);
            Debug.Log("Quest task 'Use the radio' marked as completed.");
        }
        else
        {
            Debug.LogWarning("QuestClipboardManager not found in the scene.");
        }

        // Trigger the PostRadioInteractionManager
        if (postRadioInteractionManager != null)
        {
            postRadioInteractionManager.ActivatePostInteractionRad();
        }
        else
        {
            Debug.LogError("PostRadioInteractionManager is not assigned!");
        }
    }
} */

/* using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS4 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public GameObject thirdPanel; // Reference to the third panel
    public AudioSource ringAudioSource; // Reference to the ring AudioSource
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed
    public PostRadioInteractionManagerH postRadioInteractionManager; // Reference to the PostRadioInteractionManager
    private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
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

    public void ActivateRadioButton()
    {
        // Activate the phone button
        if (interactButton != null && !isPhoneButtonPressed)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Phone button is now active.");
        }
        else if (isPhoneButtonPressed)
        {
            Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
        }
        else
        {
            Debug.LogError("Phone button is not assigned in the Inspector!");
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
            }
            else
            {
                Debug.Log("Radio is already playing.");
            }

            // Start the panel sequence
            StartCoroutine(ShowPanelsInSequence());

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

        // Show the third panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            thirdPanel.SetActive(false);
        }

        // Stop or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            radioAudioSource.Stop(); // Stop the audio
            radioAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("Radio AudioSource has been stopped and disabled.");
        }

        // Mark the "Use the radio" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1); // Assuming this is the first task
            Debug.Log("Quest task 'Use the radio' marked as completed.");
        }
        else
        {
            Debug.LogWarning("QuestClipboardManager not found in the scene.");
        }


        // Trigger the PostRadioInteractionManager
        if (postRadioInteractionManager != null)
        {
            postRadioInteractionManager.ActivatePostInteractionRad();
        }
        else
        {
            Debug.LogError("PostRadioInteractionManager is not assigned!");
        }
    }
}
 */
/* using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS4 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel
    public GameObject thirdPanel; // Reference to the third panel
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
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

    public void ActivateRadioButton()
    {
        // Activate the phone button
        if (interactButton != null && !isPhoneButtonPressed)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Phone button is now active.");
        }
        else if (isPhoneButtonPressed)
        {
            Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
        }
        else
        {
            Debug.LogError("Phone button is not assigned in the Inspector!");
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
            }
            else
            {
                Debug.Log("Radio is already playing.");
            }

            // Start the panel sequence
            StartCoroutine(ShowPanelsInSequence());

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

        // Show the third panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            thirdPanel.SetActive(false);
        }

        // Stop or disable the AudioSource after showing the panels
        if (radioAudioSource != null)
        {
            radioAudioSource.Stop(); // Stop the audio
            radioAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("Radio AudioSource has been stopped and disabled.");
        }

        // Mark the "Use the radio" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1); // Assuming this is the first task
            Debug.Log("Quest task 'Use the radio' marked as completed.");
        }
        else
        {
            Debug.LogWarning("QuestClipboardManager not found in the scene.");
        }
    }
}
 */

/* using UnityEngine;
using UnityEngine.UI;
using ClassroomS3;

public class RadioInteractionManagerS4 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject firstPanel; // Reference to the first panel
    public GameObject secondPanel; // Reference to the second panel

    public GameObject thirdPanel; // Reference to the third panel (not used in this script but can be added if needed)
    private GameObject currentInteractable; // The object the player is near
    private AudioSource radioAudioSource; // Reference to the radio's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed



    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {

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

    public void ActivateRadioButton()
    {
        // Activate the phone button
        if (interactButton != null && !isPhoneButtonPressed)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Phone button is now active.");
        }
        else if (isPhoneButtonPressed)
        {
            Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
        }
        else
        {
            Debug.LogError("Phone button is not assigned in the Inspector!");
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
            }
            else
            {
                Debug.Log("Radio is already playing.");
            }

            // Start the panel sequence
            StartCoroutine(ShowPanelsInSequence());

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

        // Show the second panel
        if (thirdPanel != null)
        {
            thirdPanel.SetActive(true);
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            thirdPanel.SetActive(false);
        }

        // Mark the "Use the radio" task as completed
        var questManager = FindObjectOfType<QuestClipboardManager>();
        if (questManager != null)
        {
            questManager.CompleteTask(1); // Assuming this is the first task
            Debug.Log("Quest task 'Use the radio' marked as completed.");
        }
        else
        {
            Debug.LogWarning("QuestClipboardManager not found in the scene.");
        }
    }


}

 */