using UnityEngine;

public class StageDialogueManager2 : MonoBehaviour
{
    public GameObject dialoguePanel; // Reference to the dialogue panel (image)
    public AudioSource voiceAudioSource; // Reference to the voice audio source
    public float dialogueDisplayDuration = 3f; // Duration to display the dialogue panel

    /* void Start()
    {
        // Show the dialogue panel and play the voice audio at the start of the stage
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);

            if (voiceAudioSource != null)
            {
                voiceAudioSource.Play();
                Debug.Log("Voice audio started!");
            }

            Invoke(nameof(HideDialoguePanel), dialogueDisplayDuration);
        }
        else
        {
            Debug.LogError("Dialogue panel is not assigned in the Inspector!");
        }
    } */
    void Start()
    {
        Debug.Log("StageDialogueManager Start method called.");
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            Debug.Log("Dialogue panel activated.");
        }

        if (voiceAudioSource != null)
        {
            voiceAudioSource.Play();
            Debug.Log("Voice audio started.");
        }

        Invoke(nameof(HideDialoguePanel), dialogueDisplayDuration);
    }
    private void HideDialoguePanel()
    {
        // Hide the dialogue panel and stop the voice audio
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
            Debug.Log("Dialogue panel hidden after display duration.");
        }

        if (voiceAudioSource != null && voiceAudioSource.isPlaying)
        {
            voiceAudioSource.Stop();
            Debug.Log("Voice audio stopped.");
        }
    }
}


