using UnityEngine;

public class PostWindowInteractionManager : MonoBehaviour
{
    public GameObject postInteractionPanel; // Panel to display after the window interaction
    public AudioSource postInteractionAudioSource; // Audio source to play with the panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (postInteractionPanel != null)
        {
            postInteractionPanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (postInteractionAudioSource != null && postInteractionAudioSource.isPlaying)
        {
            postInteractionAudioSource.Stop();
        }
    }

    public void ActivatePostInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-window interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-window interaction activated!");

        // Start the sequence of showing the panel and playing audio
        StartCoroutine(ShowPanelAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelAndPlayAudio()
    {
        // Show the panel
        if (postInteractionPanel != null)
        {
            postInteractionPanel.SetActive(true);
            Debug.Log("Post-interaction panel displayed.");
        }

        // Play the audio
        if (postInteractionAudioSource != null)
        {
            postInteractionAudioSource.Play();
            Debug.Log("Post-interaction audio started.");
        }

        // Wait for 3 seconds (or the duration of the panel display)
        yield return new WaitForSeconds(3f);

        // Hide the panel
        if (postInteractionPanel != null)
        {
            postInteractionPanel.SetActive(false);
            Debug.Log("Post-interaction panel hidden.");
        }

        // Stop the audio
        if (postInteractionAudioSource != null)
        {
            postInteractionAudioSource.Stop();
            Debug.Log("Post-interaction audio stopped.");
        }
    }
}