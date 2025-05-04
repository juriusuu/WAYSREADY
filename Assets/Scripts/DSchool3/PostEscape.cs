using UnityEngine;

public class EscapeRoutePanelManager : MonoBehaviour
{
    public GameObject escapeRoutePanel; // Panel to display when the player enters the trigger
    public AudioSource audioSource; // Audio source to play with the panel
    private bool isActivated = false; // Tracks if the panel has already been shown

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (escapeRoutePanel != null)
        {
            escapeRoutePanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ShowEscapeRoutePanel()
    {
        if (isActivated)
        {
            Debug.LogWarning("Escape route panel has already been shown.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Escape route panel activated!");

        // Show the panel and play the audio
        if (escapeRoutePanel != null)
        {
            escapeRoutePanel.SetActive(true);
            Debug.Log("Escape route panel displayed.");
        }

        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audio for escape route panel started.");
        }

        // Hide the panel after 3 seconds
        StartCoroutine(HideEscapeRoutePanelAfterDelay(3f));
    }

    private System.Collections.IEnumerator HideEscapeRoutePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hide the panel
        if (escapeRoutePanel != null)
        {
            escapeRoutePanel.SetActive(false);
            Debug.Log("Escape route panel hidden.");
        }

        // Stop the audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Audio for escape route panel stopped.");
        }
    }
}