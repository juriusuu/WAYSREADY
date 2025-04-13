using UnityEngine;
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
        FindObjectOfType<QuestClipboardManagerS3>()?.CompleteTask(0);

        // Hide the button after interaction
        interactButton.gameObject.SetActive(false);
        Debug.Log("Window interaction completed. Task 0 marked as complete.");
    }
}