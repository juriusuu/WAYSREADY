using UnityEngine;

public class PostFireSignBoardInteractionManager : MonoBehaviour
{
    public GameObject postInteractionPanel; // Panel to display after the interaction
    public AudioSource audioSource; // Audio source to play with the panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (postInteractionPanel != null)
        {
            postInteractionPanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ActivatePostInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-fire signboard interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-fire signboard interaction activated!");

        // Show the panel and play the audio
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
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audio for post-interaction panel started.");
        }

        // Wait for 2 seconds (or the duration of the panel display)
        yield return new WaitForSeconds(3f);

        // Hide the panel
        if (postInteractionPanel != null)
        {
            postInteractionPanel.SetActive(false);
            Debug.Log("Post-interaction panel hidden.");
        }

        // Stop the audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Audio for post-interaction panel stopped.");
        }
    }
}