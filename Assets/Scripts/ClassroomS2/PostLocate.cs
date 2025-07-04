using UnityEngine;

public class PostTableInteractionManager : MonoBehaviour
{
    public GameObject panel1; // First panel to display
                              //  public GameObject panel2; // Second panel to display
    public AudioClip audioClip1; // Audio clip for the first panel
    //public AudioClip audioClip2; // Audio clip for the second panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure both panels are hidden at the start
        if (panel1 != null) panel1.SetActive(false);
        //if (panel2 != null) panel2.SetActive(false);
    }

    public void ActivatePostInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-table interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-table interaction activated!");

        // Start the sequence of showing the panels and playing audio
        StartCoroutine(ShowPanelsAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelsAndPlayAudio()
    {
        // Show the first panel and play its audio
        if (panel1 != null)
        {
            panel1.SetActive(true);
            Debug.Log("Panel 1 displayed.");

            if (audioClip1 != null)
            {
                AudioSource.PlayClipAtPoint(audioClip1, Camera.main.transform.position);
                Debug.Log("Audio for Panel 1 played.");
            }

            yield return new WaitForSeconds(3f); // Wait for 3 seconds (or the duration of the first panel)
            panel1.SetActive(false);
            Debug.Log("Panel 1 hidden.");
        }

    }
}
