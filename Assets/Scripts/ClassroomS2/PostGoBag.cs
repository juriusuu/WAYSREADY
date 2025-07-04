using UnityEngine;

public class PostGoBagInteractionManager : MonoBehaviour
{
    public GameObject panel1; // First panel to display
    public GameObject panel2; // Second panel to display
    public AudioClip panel1AudioClip; // Audio clip for the first panel
    public AudioClip panel2AudioClip; // Audio clip for the second panel
    private bool isActivated = false; // Tracks if the script has been activated

    private void Start()
    {
        // Ensure both panels are hidden at the start
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);
    }

    public void ActivatePostInteraction()
    {
        if (isActivated)
        {
            Debug.LogWarning("Post-Go Bag interaction has already been activated.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Post-Go Bag interaction activated!");

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

            if (panel1AudioClip != null)
            {
                AudioSource.PlayClipAtPoint(panel1AudioClip, Camera.main.transform.position);
                Debug.Log("Panel 1 audio played.");
            }

            yield return new WaitForSeconds(4f); // Wait for 3 seconds
            panel1.SetActive(false);
            Debug.Log("Panel 1 hidden.");
        }

        // Show the second panel and play its audio
        if (panel2 != null)
        {
            panel2.SetActive(true);
            Debug.Log("Panel 2 displayed.");

            if (panel2AudioClip != null)
            {
                AudioSource.PlayClipAtPoint(panel2AudioClip, Camera.main.transform.position);
                Debug.Log("Panel 2 audio played.");
            }

            yield return new WaitForSeconds(3f); // Wait for 3 seconds
            panel2.SetActive(false);
            Debug.Log("Panel 2 hidden.");
        }
    }
}