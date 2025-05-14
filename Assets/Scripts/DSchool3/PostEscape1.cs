using UnityEngine;

public class EscapeRoutePanelManager1 : MonoBehaviour
{
    public GameObject panel1; // Panel to display
    public AudioClip audioClip1; // Audio clip for the panel
    private bool isActivated = false; // Tracks if the panel has already been shown

    private void Start()
    {
        // Ensure the panel is hidden at the start
        if (panel1 != null) panel1.SetActive(false);
    }

    public void ShowEscapeRoutePanel()
    {
        if (isActivated)
        {
            Debug.LogWarning("Escape route panel has already been shown.");
            return;
        }

        isActivated = true; // Mark as activated
        Debug.Log("Escape route panel activated!");

        // Start the sequence of showing the panel and playing audio
        StartCoroutine(ShowPanelAndPlayAudio());
    }

    private System.Collections.IEnumerator ShowPanelAndPlayAudio()
    {
        // Show the panel and play its audio
        if (panel1 != null)
        {
            panel1.SetActive(true);
            Debug.Log("Panel displayed.");

            if (audioClip1 != null)
            {
                AudioSource.PlayClipAtPoint(audioClip1, Camera.main.transform.position);
                Debug.Log("Audio for Panel played.");
            }

            yield return new WaitForSeconds(3f); // Wait for 3 seconds
            panel1.SetActive(false);
            Debug.Log("Panel hidden.");
        }
    }
}