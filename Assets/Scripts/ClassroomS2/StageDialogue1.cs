/* using UnityEngine;

public class StageDialogueManager1 : MonoBehaviour
{
    public GameObject dialoguePanel; // Reference to the dialogue panel (image)
    public AudioSource voiceAudioSource; // Reference to the voice audio source
    public float dialogueDisplayDuration = 3f; // Duration to display the dialogue panel
 */
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
/*     void Start()
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
} */


using UnityEngine;

public class StageDialogueManager1 : MonoBehaviour
{
    public GameObject dialoguePanel; // Reference to the dialogue panel (image)
    public AudioSource voiceAudioSource; // Reference to the voice audio source
    public float dialogueDisplayDuration = 3f; // Duration to display the dialogue panel

    private bool canStartDialogue = false; // Control when dialogue can start

    void Start()
    {
        Debug.Log("StageDialogueManager1 Start method called.");

        // Don't start dialogue immediately - wait for instruction completion
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // Keep it hidden initially
            Debug.Log("Dialogue panel is hidden - waiting for instructions to complete.");
        }
    }

    // This method will be called from InstructionManagerSandBag when instructions are done
    public void StartDialogue()
    {
        if (!canStartDialogue)
        {
            canStartDialogue = true;
            Debug.Log("Starting dialogue after instructions completed.");

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
                Debug.Log("Dialogue panel activated.");
            }

            // Play your voice audio here
            if (voiceAudioSource != null)
            {
                voiceAudioSource.Play();
                Debug.Log("Voice audio started.");
            }

            Invoke(nameof(HideDialoguePanel), dialogueDisplayDuration);
        }
        else
        {
            Debug.Log("Dialogue already started - ignoring duplicate call.");
        }
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