using UnityEngine;
using UnityEngine.UI;
using BedroomScriptS3;

public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels for the telephone interaction
    public AudioClip ringAudioClip; // Ring audio clip assigned in the Inspector
    public AudioClip[] telephoneAudioClips; // Array of telephone audio clips assigned in the Inspector
    public float ringAudioDuration = 2f; // Duration of the ring audio
    public float[] panelDisplayTimes; // Array of display times for each panel

    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager phoneButtonManager; // Reference to the PhoneButtonManager
    public PostTelephoneInteractionHard postTelephoneInteractionManager; // Reference to the PostTelephoneInteractionManager

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
                AudioSource.PlayClipAtPoint(telephoneAudioClips[i], transform.position);
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
            questManager.CompleteTask(1); // Assuming this is the first task
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

        // Activate the phone button after all panels are shown
        if (phoneButtonManager != null)
        {
            phoneButtonManager.ActivatePhoneButton();
            Debug.Log("Phone button activated after showing all panels.");
        }

        Debug.Log("Finished showing all panels and playing all audio clips.");

        // Mark interaction as complete and disable the button
        interactButton.gameObject.SetActive(false); // Hide the button permanently
    }
}

/* using UnityEngine;
using UnityEngine.UI;
using BedroomScriptS3;

public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels for the telephone interaction
    public string ringAudioPath; // Path to the ring audio clip (e.g., "Audio/Ring")
    public string[] telephoneAudioPaths; // Paths to the telephone audio clips (e.g., "Audio/TelephoneClip1")
    public float ringAudioDuration = 2f; // Duration of the ring audio
    public float[] panelDisplayTimes; // Array of display times for each panel

    private AudioClip ringAudioClip; // Ring audio clip loaded at runtime
    private AudioClip[] telephoneAudioClips; // Telephone audio clips loaded at runtime
    private GameObject currentInteractable; // The object the player is near
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager phoneButtonManager; // Reference to the PhoneButtonManager
    public PostTelephoneInteractionHard postTelephoneInteractionManager; // Reference to the PostTelephoneInteractionManager

    private void Start()
    {
        // Load the ring audio clip
        ringAudioClip = Resources.Load<AudioClip>(ringAudioPath);
        if (ringAudioClip == null)
        {
            Debug.LogError($"Failed to load ring audio clip from path: {ringAudioPath}");
        }

        // Load the telephone audio clips
        telephoneAudioClips = new AudioClip[telephoneAudioPaths.Length];
        for (int i = 0; i < telephoneAudioPaths.Length; i++)
        {
            telephoneAudioClips[i] = Resources.Load<AudioClip>(telephoneAudioPaths[i]);
            if (telephoneAudioClips[i] == null)
            {
                Debug.LogError($"Failed to load telephone audio clip from path: {telephoneAudioPaths[i]}");
            }
        }

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
            Debug.LogError("Ring audio clip is not assigned or failed to load!");
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
                AudioSource.PlayClipAtPoint(telephoneAudioClips[i], transform.position);
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
            questManager.CompleteTask(1); // Assuming this is the first task
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

        // Activate the phone button after all panels are shown
        if (phoneButtonManager != null)
        {
            phoneButtonManager.ActivatePhoneButton();
            Debug.Log("Phone button activated after showing all panels.");
        }

        Debug.Log("Finished showing all panels and playing all audio clips.");

        // Mark interaction as complete and disable the button
        interactButton.gameObject.SetActive(false); // Hide the button permanently
    }
} */
/* using UnityEngine;
using UnityEngine.UI;
using BedroomScriptS3;
public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels (9 panels for the telephone interaction)
    public AudioSource ringAudioSource; // Reference to the ring AudioSource
    private GameObject currentInteractable; // The object the player is near
    private AudioSource telephoneAudioSource; // Reference to the telephone's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager phoneButtonManager; // Drag and drop the PhoneButtonManager GameObject in the Inspector

    public PostTelephoneInteractionHard postTelephoneInteractionManager; // Reference to the PostTelephoneInteractionManagerS3
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

        // Ensure the ring AudioSource is assigned
        if (ringAudioSource == null)
        {
            Debug.LogError("Ring AudioSource is not assigned in the Inspector!");
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

        // Play the ring sound for 1.5 seconds
        if (ringAudioSource != null)
        {
            ringAudioSource.Play();
            Debug.Log("Ring sound started!");
            StartCoroutine(StopRingAudioAfterDelay(1.5f));
        }
        else
        {
            Debug.LogError("Ring AudioSource is not set!");
        }

        if (telephoneAudioSource != null)
        {
            // Play the telephone sound
            if (!telephoneAudioSource.isPlaying)
            {
                telephoneAudioSource.Play();
                StartCoroutine(ShowPanelsInSequence());
            }
            else
            {
                StartCoroutine(ShowPanelsInSequence());
            }

            // Mark the "Answer the telephone" task as completed
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Assuming this is the first task

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Telephone interaction completed.");
        }
        else
        {
            Debug.LogError("Telephone AudioSource is not set!");
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
            Debug.Log("Ring sound stopped after 1.5 seconds.");
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

        // Make the telephoneAudioSource inactive after showing the panels
        if (telephoneAudioSource != null)
        {
            telephoneAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("Telephone AudioSource has been disabled.");
        }


        // Trigger the PostTelephoneInteractionManagerS3
        if (postTelephoneInteractionManager != null)
        {
            postTelephoneInteractionManager.ActivatePostInteraction();
        }
        else
        {
            Debug.LogError("PostTelephoneInteractionManagerS3 is not assigned!");
        }

        // Activate the phone button after all panels are shown
        phoneButtonManager?.ActivatePhoneButton();
        Debug.Log("Phone button activated after showing all panels.");
    }
}
 */

/* using UnityEngine;
using UnityEngine.UI; // Import the namespace where PhoneButtonManager is located
using BedroomScriptS3;
using UnityEngine.SceneManagement; // Required for SceneManager
using TMPro; // Required for TextMeshPro
using static UnityEngine.SceneManagement.SceneManager; // Required for SceneManager

public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels (9 panels for the telephone interaction)
    private GameObject currentInteractable; // The object the player is near
    private AudioSource telephoneAudioSource; // Reference to the telephone's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager phoneButtonManager; // Drag and drop the PhoneButtonManager GameObject in the Inspector

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
                StartCoroutine(ShowPanelsInSequence());
            }
            else
            {
                StartCoroutine(ShowPanelsInSequence());
            }

            // Mark the "Answer the telephone" task as completed
            FindObjectOfType<QuestClipboardManager>()?.CompleteTask(1); // Assuming this is the first task

            // Activate the phone button
            //  phoneButtonManager?.ActivatePhoneButton();

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Telephone interaction completed.");
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

        // Make the telephoneAudioSource inactive after showing the panels
        if (telephoneAudioSource != null)
        {
            telephoneAudioSource.enabled = false; // Disable the AudioSource
            Debug.Log("Telephone AudioSource has been disabled.");
        }


        // Activate the phone button after all panels are shown
        phoneButtonManager?.ActivatePhoneButton();
        Debug.Log("Phone button activated after showing all panels.");
    }
}
 */

/* public class TelephoneInteractionManagerS3 : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject[] panels; // Array of panels (9 panels for the telephone interaction)
    private GameObject currentInteractable; // The object the player is near
    private AudioSource telephoneAudioSource; // Reference to the telephone's AudioSource
    private bool isInteractionComplete = false; // Tracks if the interaction is already completed

    public PhoneButtonManager phoneButtonManager; // Drag and drop the PhoneButtonManager GameObject in the Inspector
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
 */
/*   private void OnInteractButtonPressed()
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
  } */

/*     private void OnInteractButtonPressed()
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
                StartCoroutine(ShowPanelsInSequence());
            }
            else
            {
                StartCoroutine(ShowPanelsInSequence());
            }

            // Mark the "Answer the telephone" task as completed
            FindObjectOfType<QuestClipboardManagerS2>()?.CompleteTask(1); // Assuming this is the first task

            // Activate the phone button
            // Find the PhoneButtonManager and activate the phone button
            // This assumes that the PhoneButtonManager is attached to the same GameObject or is accessible in the scene
            FindFirstObjectByType<PhoneButtonManager>()?.ActivatePhoneButton();

            // Mark interaction as complete and disable the button
            isInteractionComplete = true;
            interactButton.gameObject.SetActive(false); // Hide the button permanently
            Debug.Log("Telephone interaction completed.");
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
} */