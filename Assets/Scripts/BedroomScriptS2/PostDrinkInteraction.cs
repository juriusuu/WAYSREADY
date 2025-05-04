using UnityEngine;

public class PostDrinkInteractionManager : MonoBehaviour
{
    public GameObject panel1; // First panel to display
    public GameObject panel2; // Second panel to display
    public AudioSource postDrinkAudioSource; // Audio source to play with the panels
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure both panels are hidden at the start
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);

        // Ensure the audio source is not playing at the start
        if (postDrinkAudioSource != null && postDrinkAudioSource.isPlaying)
        {
            postDrinkAudioSource.Stop();
        }
    }

    public void ActivatePostDrinkInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-drink interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-drink interaction activated!");

        // Start the sequence of showing the panels and playing audio
        StartCoroutine(ShowPanelsAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelsAndPlayAudio()
    {
        // Start playing the audio source
        if (postDrinkAudioSource != null)
        {
            postDrinkAudioSource.Play();
            Debug.Log("Post-drink audio started.");
        }

        // Show the first panel
        if (panel1 != null)
        {
            panel1.SetActive(true);
            Debug.Log("Panel 1 displayed.");
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            panel1.SetActive(false);
            Debug.Log("Panel 1 hidden.");
        }

        // Show the second panel
        if (panel2 != null)
        {
            panel2.SetActive(true);
            Debug.Log("Panel 2 displayed.");
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            panel2.SetActive(false);
            Debug.Log("Panel 2 hidden.");
        }

        // Stop the audio source
        if (postDrinkAudioSource != null)
        {
            postDrinkAudioSource.Stop();
            Debug.Log("Post-drink audio stopped.");
        }
    }
}