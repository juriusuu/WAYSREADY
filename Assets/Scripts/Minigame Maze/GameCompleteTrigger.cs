using UnityEngine;

public class GameCompleteTrigger : MonoBehaviour
{
    public PanelManager panelManager; // Reference to the PanelManager script
    public GameObject additionalPanel; // Reference to the additional panel
    public AudioSource audioSource; // Reference to the audio source

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            Debug.Log("Player entered the trigger. Triggering Complete Panel.");

            // Show the main finish panel
            if (panelManager != null)
            {
                panelManager.ShowFinishPanel(); // Call the method to show the Complete Panel
            }
            else
            {
                Debug.LogError("PanelManager is not assigned in the Inspector!");
            }

            // Show the additional panel
            if (additionalPanel != null)
            {
                additionalPanel.SetActive(true);
                Debug.Log("Additional panel displayed.");
            }
            else
            {
                Debug.LogError("Additional panel is not assigned in the Inspector!");
            }

            // Play the audio source
            if (audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Audio source started playing.");
            }
            else
            {
                Debug.LogError("Audio source is not assigned in the Inspector!");
            }

            Time.timeScale = 0f; // Pause the game
        }
    }
}