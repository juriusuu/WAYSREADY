using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene transitions

public class SandbagInteraction : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public string sceneToLoad; // Name of the scene to load
    private bool isPlayerNear = false; // Tracks if the player is near the sandbag

    private void Start()
    {
        // Ensure the interact button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is near the sandbag
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            // Show the interact button
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
            }

            Debug.Log("Player is near the sandbag.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the interact button when the player leaves the sandbag area
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
            }

            Debug.Log("Player left the sandbag area.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (isPlayerNear)
        {
            Debug.Log("Interact button pressed. Loading scene: " + sceneToLoad);

            // Load the specified scene
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("Scene name is not set!");
            }
        }
    }
}