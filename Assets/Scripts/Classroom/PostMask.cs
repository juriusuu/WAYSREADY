using UnityEngine;

public class PostFacemaskInteractionManager : MonoBehaviour
{
    public GameObject postFacemaskPanel; // Panel to display after the facemask interaction
    public AudioSource audioSource; // Audio source to play with the panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (postFacemaskPanel != null)
        {
            postFacemaskPanel.SetActive(false);
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
            Debug.LogWarning("Post-facemask interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-facemask interaction activated!");

        // Show the panel and play the audio
        StartCoroutine(ShowPanelAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelAndPlayAudio()
    {
        // Show the panel
        if (postFacemaskPanel != null)
        {
            postFacemaskPanel.SetActive(true);
            Debug.Log("Post-facemask panel displayed.");
        }

        // Play the audio
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audio for post-facemask panel started.");
        }

        // Wait for 2 seconds (or the duration of the panel display)
        yield return new WaitForSeconds(3f);

        // Hide the panel
        if (postFacemaskPanel != null)
        {
            postFacemaskPanel.SetActive(false);
            Debug.Log("Post-facemask panel hidden.");
        }

        // Stop the audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Audio for post-facemask panel stopped.");
        }
    }
}