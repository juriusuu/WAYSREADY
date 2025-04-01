using UnityEngine;
using UnityEngine.UI;

public class TVInteractionManager : MonoBehaviour
{
    public GameObject weatherUpdateUI; // Reference to the weather update UI Panel
    public Button interactButton; // Reference to the new interaction button
    private GameObject currentInteractable; // The object the player is near
    private AudioSource tvAudioSource; // Reference to the TV's AudioSource

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonPressed); // Add listener for button click
        }

        if (weatherUpdateUI != null)
        {
            weatherUpdateUI.SetActive(false); // Hide the weather update UI at the start
        }
    }

    private void Update()
    {
        // Check if the player is near an interactable object
        if (currentInteractable != null && currentInteractable.CompareTag("Television"))
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
        // Check if the player is near an interactable object
        if (other.CompareTag("Television"))
        {
            currentInteractable = other.gameObject; // Set the current interactable object
            tvAudioSource = currentInteractable.GetComponent<AudioSource>(); // Get the AudioSource from the TV
            Debug.Log($"Player is near: {currentInteractable.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the interactable object when the player leaves the trigger
        if (currentInteractable == other.gameObject)
        {
            currentInteractable = null;
            tvAudioSource = null; // Clear the AudioSource reference
            Debug.Log("Player left the interactable object.");
        }
    }

    private void OnInteractButtonPressed()
    {
        if (currentInteractable != null && currentInteractable.CompareTag("Television"))
        {
            // Show the weather update UI
            if (weatherUpdateUI != null)
            {
                weatherUpdateUI.SetActive(true);
                Debug.Log("Weather update displayed!");

                // Start a coroutine to hide the UI after 5 seconds
                StartCoroutine(HideWeatherUpdateUI());
            }

            // Play the TV sound
            if (tvAudioSource != null)
            {
                tvAudioSource.Play(); // Play the sound
                Debug.Log("TV sound played!");
            }
        }
    }

    private System.Collections.IEnumerator HideWeatherUpdateUI()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Hide the weather update UI
        if (weatherUpdateUI != null)
        {
            weatherUpdateUI.SetActive(false);
            Debug.Log("Weather update UI hidden after 5 seconds.");
        }
    }
}