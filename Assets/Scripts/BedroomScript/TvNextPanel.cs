/* using UnityEngine;

public class TVNextPanelManager : MonoBehaviour
{
    public GameObject firstNextPanel; // Reference to the first panel to display
    public GameObject secondNextPanel; // Reference to the second panel to display
    public float firstPanelDisplayDuration = 2f; // Duration to display the first panel
    public float secondPanelDisplayDuration = 2f; // Duration to display the second panel
    public AudioSource voiceAudioSource; // Reference to the voice audio source for the panels

    private bool isNextPanelsTriggered = false; // Ensures the panels are triggered only once

    public void TriggerNextPanels()
    {
        if (!isNextPanelsTriggered)
        {
            isNextPanelsTriggered = true; // Prevent multiple triggers
            ShowFirstNextPanel();
        }
    }

    private void ShowFirstNextPanel()
    {
        if (firstNextPanel != null)
        {
            firstNextPanel.SetActive(true); // Show the first panel
            Debug.Log("First next panel displayed!");

            // Play the voice audio if assigned
            if (voiceAudioSource != null)
            {
                voiceAudioSource.Play();
                Debug.Log("Voice audio for the first panel started!");
            }

            // Hide the first panel and show the second panel after the specified duration
            Invoke(nameof(ShowSecondNextPanel), firstPanelDisplayDuration);
        }
        else
        {
            Debug.LogError("First next panel is not assigned in the Inspector!");
        }
    }

    private void ShowSecondNextPanel()
    {
        if (firstNextPanel != null)
        {
            firstNextPanel.SetActive(false); // Hide the first panel
            Debug.Log("First next panel hidden.");
        }

        if (secondNextPanel != null)
        {
            secondNextPanel.SetActive(true); // Show the second panel
            Debug.Log("Second next panel displayed!");

            // Hide the second panel after the specified duration
            Invoke(nameof(HideSecondNextPanel), secondPanelDisplayDuration);
        }
        else
        {
            Debug.LogError("Second next panel is not assigned in the Inspector!");
        }
    }

    private void HideSecondNextPanel()
    {
        if (secondNextPanel != null)
        {
            secondNextPanel.SetActive(false); // Hide the second panel
            Debug.Log("Second next panel hidden.");
        }

        // Stop the voice audio if it is playing
        if (voiceAudioSource != null && voiceAudioSource.isPlaying)
        {
            voiceAudioSource.Stop();
            Debug.Log("Voice audio for the panels stopped.");
        }
    }
} */

using UnityEngine;
using System;
public class TVNextPanelManager : MonoBehaviour
{
    public GameObject firstNextPanel; // Reference to the first panel to display
    public GameObject secondNextPanel; // Reference to the second panel to display
    public float firstPanelDisplayDuration = 5f; // Duration to display the first panel
    public float secondPanelDisplayDuration = 6f; // Duration to display the second panel
    public AudioClip firstPanelAudioClip; // Audio clip for the first panel
    public AudioClip secondPanelAudioClip; // Audio clip for the second panel
    public Action OnPanelsFinished; // <-- Add this line
    private bool isNextPanelsTriggered = false; // Ensures the panels are triggered only once

    public void TriggerNextPanels()
    {
        if (!isNextPanelsTriggered)
        {
            isNextPanelsTriggered = true; // Prevent multiple triggers
            ShowFirstNextPanel();
        }
    }

    private void ShowFirstNextPanel()
    {
        if (firstNextPanel != null)
        {
            firstNextPanel.SetActive(true); // Show the first panel
            Debug.Log("First next panel displayed!");

            // Play the audio for the first panel if assigned
            if (firstPanelAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(firstPanelAudioClip, Camera.main.transform.position);
                Debug.Log("Audio for the first panel started!");
            }

            // Hide the first panel and show the second panel after the specified duration
            Invoke(nameof(ShowSecondNextPanel), firstPanelDisplayDuration);
        }
        else
        {
            Debug.LogError("First next panel is not assigned in the Inspector!");
        }
    }

    private void ShowSecondNextPanel()
    {
        if (firstNextPanel != null)
        {
            firstNextPanel.SetActive(false); // Hide the first panel
            Debug.Log("First next panel hidden.");
        }

        if (secondNextPanel != null)
        {
            secondNextPanel.SetActive(true); // Show the second panel
            Debug.Log("Second next panel displayed!");

            // Play the audio for the second panel if assigned
            if (secondPanelAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(secondPanelAudioClip, Camera.main.transform.position);
                Debug.Log("Audio for the second panel started!");
            }

            // Hide the second panel after the specified duration
            Invoke(nameof(HideSecondNextPanel), secondPanelDisplayDuration);
        }
        else
        {
            Debug.LogError("Second next panel is not assigned in the Inspector!");
        }
    }

    private void HideSecondNextPanel()
    {
        if (secondNextPanel != null)
        {
            secondNextPanel.SetActive(false); // Hide the second panel
            Debug.Log("Second next panel hidden.");
        }
        OnPanelsFinished?.Invoke(); // <-- Add this line
    }
}