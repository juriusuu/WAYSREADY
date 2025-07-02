using UnityEngine;

namespace BedroomScriptS3
{
    public class PhoneButtonManager : MonoBehaviour
    {
        public GameObject[] phonePanels; // Array of panels for the phone interaction
        public AudioClip ringAudioClip; // Ring audio clip to play before the first panel
        public AudioClip[] phoneAudioClips; // Array of audio clips for each panel
        public float ringAudioDuration = 1.0f; // Duration of the ring audio
        public float[] panelDisplayTimes; // Array of display times for each panel

        public PostPhoneInteractionManager postPhoneInteractionManager; // Reference to the PostPhoneInteractionManager

        private void Start()
        {
            // Hide all phone panels at the start
            if (phonePanels != null && phonePanels.Length > 0)
            {
                foreach (var panel in phonePanels)
                {
                    if (panel != null)
                        panel.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("Phone panels are not assigned in the Inspector!");
            }

            // Ensure the audio clips and display times match the number of panels
            if (phoneAudioClips == null || phoneAudioClips.Length != phonePanels.Length)
            {
                Debug.LogError("The number of phone audio clips must match the number of phone panels!");
            }

            if (panelDisplayTimes == null || panelDisplayTimes.Length != phonePanels.Length)
            {
                Debug.LogError("The number of panel display times must match the number of phone panels!");
            }
        }

        // Call this from inventory slot
        public void TriggerPhoneSequenceFromInventory()
        {
            Debug.Log("TriggerPhoneSequenceFromInventory called!");
            StartCoroutine(PlayRingAudioAndShowPanelsInSequence());
        }

        private System.Collections.IEnumerator PlayRingAudioAndShowPanelsInSequence()
        {
            // Add an AudioSource component dynamically
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0.0f; // Ensure the audio is 2D

            // Play the ring audio before showing the first panel
            if (ringAudioClip != null)
            {
                audioSource.clip = ringAudioClip;
                audioSource.Play();
                Debug.Log($"Playing ring audio: {ringAudioClip.name}");
                yield return new WaitForSeconds(ringAudioDuration);
            }
            else
            {
                Debug.LogWarning("Ring audio clip is not assigned!");
            }

            // Show panels and play their corresponding audio clips
            for (int i = 0; i < phonePanels.Length; i++)
            {
                if (phonePanels[i] != null && phoneAudioClips[i] != null)
                {
                    phonePanels[i].SetActive(true);
                    audioSource.clip = phoneAudioClips[i];
                    audioSource.Play();
                    Debug.Log($"Showing panel {i}: {phonePanels[i].name}, playing audio: {phoneAudioClips[i].name}");
                    yield return new WaitForSeconds(panelDisplayTimes[i]);
                    phonePanels[i].SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"Panel or audio clip at index {i} is null. Skipping.");
                }
            }

            // Complete the task and trigger the next interaction
            CompleteTaskAndTriggerNextInteraction();

            // Remove the dynamically added AudioSource
            Destroy(audioSource);

            Debug.Log("Finished showing all panels and playing all audio clips.");
        }

        private void CompleteTaskAndTriggerNextInteraction()
        {
            // Mark the "Use the phone" task as completed
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(2); // Assuming this is the second task
                Debug.Log("Quest task 'Use the phone' marked as completed.");
            }
            else
            {
                Debug.LogWarning("QuestClipboardManagerS3 not found in the scene.");
            }

            // Trigger the PostPhoneInteractionManager
            if (postPhoneInteractionManager != null)
            {
                postPhoneInteractionManager.ActivatePostInteraction();
            }
            else
            {
                Debug.LogError("PostPhoneInteractionManager is not assigned!");
            }
        }
    }
}