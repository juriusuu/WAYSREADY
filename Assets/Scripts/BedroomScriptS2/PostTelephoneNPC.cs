using UnityEngine;

public class PostTelephoneInteractionManager : MonoBehaviour
{
    public GameObject panel1; // First panel to display
    public GameObject panel2; // Second panel to display
    public AudioSource postInteractionAudio; // Audio source to play during interaction
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure both panels are hidden at the start
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);

        // Ensure the audio source is not playing at the start
        if (postInteractionAudio != null && postInteractionAudio.isPlaying)
        {
            postInteractionAudio.Stop();
        }
    }

    public void ActivatePostInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-telephone interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-telephone interaction activated!");

        // Start the sequence of showing panels and playing audio
        StartCoroutine(ShowPanelsAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelsAndPlayAudio()
    {
        // Start playing the audio source
        if (postInteractionAudio != null)
        {
            postInteractionAudio.Play();
            Debug.Log("Post-interaction audio started.");
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

        // Stop and disable the audio source
        if (postInteractionAudio != null)
        {
            postInteractionAudio.Stop();
            postInteractionAudio.enabled = false;
            Debug.Log("Post-interaction audio stopped and disabled.");
        }
    }
}