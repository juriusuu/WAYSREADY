/* using UnityEngine;

public class AdditionalPanelManager : MonoBehaviour
{
    public GameObject additionalPanel; // Reference to the additional panel
    public AudioSource audioSource; // Audio source for the additional panel

    private bool isPanelShown = false; // Tracks if the panel has already been shown

    private void Start()
    {
        // Ensure the additional panel is hidden at the start
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ShowAdditionalPanel()
    {
        if (isPanelShown)
        {
            Debug.LogWarning("Additional panel has already been shown.");
            return;
        }

        isPanelShown = true; // Mark the panel as shown
        Debug.Log("Showing additional panel.");

        // Show the additional panel
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(true);
        }

        // Play the audio source
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Playing audio for additional panel.");
        }

        // Hide the panel and stop the audio after 1.5 seconds
        Invoke(nameof(HideAdditionalPanel), 0.5f);
    }

    public void HideAdditionalPanel()
    {
        Debug.Log("Hiding additional panel.");

        // Hide the additional panel
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(false);
        }

        // Stop the audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Reset the panel state for potential reuse
        isPanelShown = false;
    }
} */

using UnityEngine;

public class AdditionalPanelManager : MonoBehaviour
{
    public GameObject additionalPanel; // Reference to the additional panel
    public AudioSource audioSource; // Audio source for the additional panel

    private bool isPanelShown = false; // Tracks if the panel has already been shown

    private void Start()
    {
        // Ensure the additional panel is hidden at the start
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(false);
        }

        // Ensure the audio source is not playing at the start
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ShowAdditionalPanel()
    {
        if (isPanelShown)
        {
            Debug.LogWarning("Additional panel has already been shown.");
            return;
        }

        isPanelShown = true; // Mark the panel as shown
        Debug.Log("Showing additional panel.");

        // Show the additional panel
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(true);
        }

        // Play the audio source
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Playing audio for additional panel.");
        }

        // Automatically hide the panel after 1 second
        Invoke(nameof(HideAdditionalPanel), 1f);
    }

    public void HideAdditionalPanel()
    {
        Debug.Log("Hiding additional panel.");

        // Hide the additional panel
        if (additionalPanel != null)
        {
            additionalPanel.SetActive(false);
        }

        // Stop the audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Reset the panel state for potential reuse
        isPanelShown = false;
    }
}