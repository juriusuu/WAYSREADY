using UnityEngine;
using UnityEngine.UI;

public class WindowInteractionManager : MonoBehaviour
{
    public Button interactButton;
    public AudioSource ringAudioSource;
    public PostWindowInteractionManager postWindowInteractionManager;

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed);
        }

        if (ringAudioSource == null)
        {
            Debug.LogError("Ring AudioSource is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactButton != null)
        {
            interactButton.gameObject.SetActive(true);
            Debug.Log("Player entered window trigger. Interaction enabled.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            Debug.Log("Player exited window trigger. Interaction disabled.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (ringAudioSource != null)
        {
            ringAudioSource.Play();
            Debug.Log("Ring sound started!");
            StartCoroutine(StopRingAudioAfterDelay(2f));
        }
        else
        {
            Debug.LogError("Ring AudioSource is not set!");
        }

        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(0);

        interactButton.gameObject.SetActive(false);
        Debug.Log("Window interaction completed. Task 0 marked as complete.");

        StartCoroutine(ActivatePostInteractionWithDelay(1f));
    }

    private System.Collections.IEnumerator ActivatePostInteractionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (postWindowInteractionManager != null)
        {
            postWindowInteractionManager.ActivatePostInteraction();
        }
        else
        {
            Debug.LogError("PostWindowInteractionManager is not assigned!");
        }
    }

    private System.Collections.IEnumerator StopRingAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (ringAudioSource != null)
        {
            ringAudioSource.Stop();
            Debug.Log("Ring sound stopped after 2 seconds.");
        }
    }
}

/* using UnityEngine;
using UnityEngine.UI;

public class WindowInteractionManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject player; // Reference to the player GameObject
    public float interactionDistance = 3f; // Distance within which the player can interact
    public AudioSource ringAudioSource; // Reference to the ring AudioSource
    private bool isPlayerNear = false; // Tracks if the player is near the object
    public PostWindowInteractionManager postWindowInteractionManager; // Reference to the PostWindowInteractionManager
    void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        // Ensure the ring AudioSource is assigned
        if (ringAudioSource == null)
        {
            Debug.LogError("Ring AudioSource is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Check the distance between the player and this object
        if (Vector3.Distance(player.transform.position, transform.position) <= interactionDistance)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                interactButton.gameObject.SetActive(true); // Show the button
                Debug.Log("Player is near the window. Interaction enabled.");
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                interactButton.gameObject.SetActive(false); // Hide the button
                Debug.Log("Player moved away from the window. Interaction disabled.");
            }
        }
    }

    private void OnInteractButtonPressed()
    {
        // Play the ring sound for 2 seconds
        if (ringAudioSource != null)
        {
            ringAudioSource.Play();
            Debug.Log("Ring sound started!");
            StartCoroutine(StopRingAudioAfterDelay(2f));
        }
        else
        {
            Debug.LogError("Ring AudioSource is not set!");
        }

        // Mark the first quest task (task 0) as completed
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(0);

        // Hide the button after interaction
        interactButton.gameObject.SetActive(false);
        Debug.Log("Window interaction completed. Task 0 marked as complete.");


        // Delay the activation of the PostWindowInteractionManager by 1 second
        StartCoroutine(ActivatePostInteractionWithDelay(1f));
    }
    private System.Collections.IEnumerator ActivatePostInteractionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Trigger the PostWindowInteractionManager
        if (postWindowInteractionManager != null)
        {
            postWindowInteractionManager.ActivatePostInteraction();
        }
        else
        {
            Debug.LogError("PostWindowInteractionManager is not assigned!");
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
            Debug.Log("Ring sound stopped after 2 seconds.");
        }
    }
}
 */

/* using UnityEngine;
using UnityEngine.UI;

public class WindowInteractionManager : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject player; // Reference to the player GameObject
    public float interactionDistance = 3f; // Distance within which the player can interact
    private bool isPlayerNear = false; // Tracks if the player is near the object

    void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }
    }

    void Update()
    {
        // Check the distance between the player and this object
        if (Vector3.Distance(player.transform.position, transform.position) <= interactionDistance)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                interactButton.gameObject.SetActive(true); // Show the button
                Debug.Log("Player is near the window. Interaction enabled.");
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                interactButton.gameObject.SetActive(false); // Hide the button
                Debug.Log("Player moved away from the window. Interaction disabled.");
            }
        }
    }

    private void OnInteractButtonPressed()
    {
        // Mark the first quest task (task 0) as completed
        FindObjectOfType<QuestClipboardManager>()?.CompleteTask(0);

        // Hide the button after interaction
        interactButton.gameObject.SetActive(false);
        Debug.Log("Window interaction completed. Task 0 marked as complete.");
    }
} */