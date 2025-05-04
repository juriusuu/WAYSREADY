using UnityEngine;

public class PostRadioInteractionManagerH : MonoBehaviour
{
    public GameObject panel1; // First panel to display
    public GameObject panel2; // Second panel to display
    public AudioSource audioSource; // Single audio source for both panels
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure both panels are hidden at the start
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);

        // Ensure the audio source is not playing at the start
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void ActivatePostInteractionRad()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-radio interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-radio interaction activated!");

        // Start the sequence of showing the panels and playing audio
        StartCoroutine(ShowPanelsAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelsAndPlayAudio()
    {
        // Show the first panel and play the audio
        if (panel1 != null)
        {
            panel1.SetActive(true);
            Debug.Log("Panel 1 displayed.");

            if (audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Audio started for Panel 1.");
            }

            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            panel1.SetActive(false);
            Debug.Log("Panel 1 hidden.");
        }

        // Show the second panel
        if (panel2 != null)
        {
            panel2.SetActive(true);
            Debug.Log("Panel 2 displayed.");

            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
            panel2.SetActive(false);
            Debug.Log("Panel 2 hidden.");
        }

        // Stop the audio after both panels are shown
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Audio stopped after Panel 2.");
        }
    }
}