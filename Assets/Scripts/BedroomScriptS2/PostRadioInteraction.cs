using UnityEngine;

public class PostRadioInteractionManager : MonoBehaviour
{
    public GameObject postRadioPanel; // Panel to display after the radio interaction
    public AudioSource postRadioAudioSource; // Audio source to play with the panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (postRadioPanel != null)
        {
            postRadioPanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (postRadioAudioSource != null && postRadioAudioSource.isPlaying)
        {
            postRadioAudioSource.Stop();
        }
    }

    public void ActivatePostRadioInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-radio interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-radio interaction activated!");

        // Start the sequence of showing the panel and playing audio
        StartCoroutine(ShowPanelAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelAndPlayAudio()
    {
        // Show the panel
        if (postRadioPanel != null)
        {
            postRadioPanel.SetActive(true);
            Debug.Log("Post-radio panel displayed.");
        }

        // Play the audio
        if (postRadioAudioSource != null)
        {
            postRadioAudioSource.Play();
            Debug.Log("Post-radio audio started.");
        }

        // Wait for 3 seconds (or the duration of the panel display)
        yield return new WaitForSeconds(3f);

        // Hide the panel
        if (postRadioPanel != null)
        {
            postRadioPanel.SetActive(false);
            Debug.Log("Post-radio panel hidden.");
        }

        // Stop the audio
        if (postRadioAudioSource != null)
        {
            postRadioAudioSource.Stop();
            Debug.Log("Post-radio audio stopped.");
        }
    }
}