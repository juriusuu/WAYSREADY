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

/* using UnityEngine;
using UnityEngine.UI;

namespace BedroomScriptS3
{
    public class PhoneButtonManager : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public AudioClip ringAudioClip; // Ring audio clip to play before the first panel
        public AudioClip[] phoneAudioClips; // Array of audio clips for each panel
        public float ringAudioDuration = 1.0f; // Duration of the ring audio
        public float[] panelDisplayTimes; // Array of display times for each panel

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed
        public PostPhoneInteractionManager postPhoneInteractionManager; // Reference to the PostPhoneInteractionManager

        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                phoneButton.onClick.AddListener(OnPhoneButtonPressed); // Add listener for phone button interaction
                Debug.Log("Phone button initialized and hidden.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }

            // Ensure all phone panels are hidden at the start
            if (phonePanels != null && phonePanels.Length > 0)
            {
                foreach (var panel in phonePanels)
                {
                    if (panel != null)
                    {
                        panel.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("One of the phone panels is null. Please check the Inspector.");
                    }
                }
                Debug.Log("All phone panels are initialized and hidden.");
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
        public void TriggerPhoneSequenceFromInventory()
        {
            // Optionally reset the pressed state if you want to allow multiple uses from inventory
            isPhoneButtonPressed = false;
            StartCoroutine(PlayRingAudioAndShowPanelsInSequence());
        }
        public void ActivatePhoneButton()
        {
            // Activate the phone button
            if (phoneButton != null && !isPhoneButtonPressed)
            {
                phoneButton.gameObject.SetActive(true);
                Debug.Log("Phone button is now active.");
            }
            else if (isPhoneButtonPressed)
            {
                Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }
        }

        private void OnPhoneButtonPressed()
        {
            if (isPhoneButtonPressed)
            {
                Debug.Log("Phone button already pressed. Ignoring further presses.");
                return;
            }

            // Handle the phone button interaction
            Debug.Log("Phone button pressed. Starting ring audio and panel sequence.");
            StartCoroutine(PlayRingAudioAndShowPanelsInSequence());

            // Mark the phone button as pressed
            isPhoneButtonPressed = true;
        }

        private System.Collections.IEnumerator PlayRingAudioAndShowPanelsInSequence()
        {
            // Add an AudioSource component dynamically
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0.0f; // Ensure the audio is 2D (local space)

            // Play the ring audio before showing the first panel
            if (ringAudioClip != null)
            {
                audioSource.clip = ringAudioClip;
                audioSource.Play();
                Debug.Log($"Playing ring audio: {ringAudioClip.name}");
                yield return new WaitForSeconds(ringAudioDuration); // Wait for the ring audio to finish
            }
            else
            {
                Debug.LogWarning("Ring audio clip is not assigned!");
            }

            // Start showing panels and playing their corresponding audio clips
            Debug.Log("Starting panel and audio sequence...");
            for (int i = 0; i < phonePanels.Length; i++)
            {
                if (phonePanels[i] != null && phoneAudioClips[i] != null)
                {
                    // Show the panel
                    Debug.Log($"Showing panel {i}: {phonePanels[i].name}");
                    phonePanels[i].SetActive(true);

                    // Play the corresponding audio clip
                    audioSource.clip = phoneAudioClips[i];
                    audioSource.Play();
                    Debug.Log($"Playing audio clip {i}: {phoneAudioClips[i].name}");

                    // Wait for the specified display time for this panel
                    yield return new WaitForSeconds(panelDisplayTimes[i]);

                    // Hide the panel
                    phonePanels[i].SetActive(false);
                    Debug.Log($"Hiding panel {i}: {phonePanels[i].name}");
                }
                else
                {
                    Debug.LogWarning($"Panel or audio clip at index {i} is null. Skipping.");
                }
            }

            // After showing all panels, complete the task and trigger the next interaction
            CompleteTaskAndTriggerNextInteraction();

            // Disable the phone button after all panels are shown
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                Debug.Log("Phone button is now disabled after showing all panels.");
            }

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
 */

/* using UnityEngine;
using UnityEngine.UI;

namespace BedroomScriptS3
{
    public class PhoneButtonManager : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed
        public AudioSource phoneAudioSource; // Reference to the phone's AudioSource
        public AudioSource ringAudioSource; // Reference to the ring AudioSource

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed
        public PostPhoneInteractionManager postPhoneInteractionManager; // Reference to the PostPhoneInteractionManager
        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                phoneButton.onClick.AddListener(OnPhoneButtonPressed); // Add listener for phone button interaction
                Debug.Log("Phone button initialized and hidden.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }

            // Ensure all phone panels are hidden at the start
            if (phonePanels != null && phonePanels.Length > 0)
            {
                foreach (var panel in phonePanels)
                {
                    if (panel != null)
                    {
                        panel.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("One of the phone panels is null. Please check the Inspector.");
                    }
                }
                Debug.Log("All phone panels are initialized and hidden.");
            }
            else
            {
                Debug.LogError("Phone panels are not assigned in the Inspector!");
            }

            // Ensure the AudioSource is assigned
            if (phoneAudioSource == null)
            {
                phoneAudioSource = GetComponent<AudioSource>();
                if (phoneAudioSource == null)
                {
                    Debug.LogError("No AudioSource found on the PhoneButtonManager GameObject!");
                }
            }

            // Ensure the ring AudioSource is assigned
            if (ringAudioSource == null)
            {
                Debug.LogError("Ring AudioSource is not assigned in the Inspector!");
            }
        }

        public void ActivatePhoneButton()
        {
            // Activate the phone button
            if (phoneButton != null && !isPhoneButtonPressed)
            {
                phoneButton.gameObject.SetActive(true);
                Debug.Log("Phone button is now active.");
            }
            else if (isPhoneButtonPressed)
            {
                Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }
        }

        private void OnPhoneButtonPressed()
        {
            if (isPhoneButtonPressed)
            {
                Debug.Log("Phone button already pressed. Ignoring further presses.");
                return;
            }

            // Play the ring sound for 1.5 seconds
            if (ringAudioSource != null)
            {
                ringAudioSource.Play();
                Debug.Log("Ring sound started!");
                StartCoroutine(StopRingAudioAfterDelay(1.5f));
            }
            else
            {
                Debug.LogError("Ring AudioSource is not set!");
            }

            // Play the phone audio
            if (phoneAudioSource != null && !phoneAudioSource.isPlaying)
            {
                phoneAudioSource.Play();
                Debug.Log("Phone audio is now playing.");
            }

            // Handle the phone button interaction
            Debug.Log("Phone button pressed. Starting panel sequence.");
            StartCoroutine(ShowPanelsInSequence());

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

            // Mark the phone button as pressed
            isPhoneButtonPressed = true;
        }

        private System.Collections.IEnumerator StopRingAudioAfterDelay(float delay)
        {
            // Wait for the specified delay
            yield return new WaitForSeconds(delay);

            // Stop the ring sound
            if (ringAudioSource != null)
            {
                ringAudioSource.Stop();
                Debug.Log("Ring sound stopped after 1.5 seconds.");
            }
        }

        private System.Collections.IEnumerator ShowPanelsInSequence()
        {
            Debug.Log("Starting ShowPanelsInSequence coroutine...");
            int panelIndex = 0; // Track the index of the current panel

            foreach (var panel in phonePanels)
            {
                if (panel != null)
                {
                    Debug.Log($"Showing panel {panelIndex}: {panel.name}");
                    panel.SetActive(true); // Show the panel
                    yield return new WaitForSeconds(panelDisplayTime); // Wait for the specified time
                    panel.SetActive(false); // Hide the panel
                    Debug.Log($"Hiding panel {panelIndex}: {panel.name}");
                }
                else
                {
                    Debug.LogWarning($"Panel {panelIndex} is null. Skipping.");
                }
                panelIndex++;
            }

            // Disable the phone button after all panels are shown
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                Debug.Log("Phone button is now disabled after showing all panels.");
            }
            // Disable the AudioSource after all panels are shown
            if (phoneAudioSource != null)
            {
                phoneAudioSource.enabled = false;
                Debug.Log("Phone AudioSource has been disabled.");
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

            Debug.Log("Finished showing all panels.");
        }
    }
}
 */

/* using UnityEngine;
using UnityEngine.UI;

namespace BedroomScriptS3
{
    public class PhoneButtonManager : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed
        public AudioSource phoneAudioSource; // Reference to the phone's AudioSource

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                phoneButton.onClick.AddListener(OnPhoneButtonPressed); // Add listener for phone button interaction
                Debug.Log("Phone button initialized and hidden.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }

            // Ensure all phone panels are hidden at the start
            if (phonePanels != null && phonePanels.Length > 0)
            {
                foreach (var panel in phonePanels)
                {
                    if (panel != null)
                    {
                        panel.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("One of the phone panels is null. Please check the Inspector.");
                    }
                }
                Debug.Log("All phone panels are initialized and hidden.");
            }
            else
            {
                Debug.LogError("Phone panels are not assigned in the Inspector!");
            }

            // Ensure the AudioSource is assigned
            if (phoneAudioSource == null)
            {
                phoneAudioSource = GetComponent<AudioSource>();
                if (phoneAudioSource == null)
                {
                    Debug.LogError("No AudioSource found on the PhoneButtonManager GameObject!");
                }
            }
        }

        public void ActivatePhoneButton()
        {
            // Activate the phone button
            if (phoneButton != null && !isPhoneButtonPressed)
            {
                phoneButton.gameObject.SetActive(true);
                Debug.Log("Phone button is now active.");
            }
            else if (isPhoneButtonPressed)
            {
                Debug.LogWarning("Phone button is already pressed. Cannot activate again.");
            }
            else
            {
                Debug.LogError("Phone button is not assigned in the Inspector!");
            }
        }

        private void OnPhoneButtonPressed()
        {
            if (isPhoneButtonPressed)
            {
                Debug.Log("Phone button already pressed. Ignoring further presses.");
                return;
            }


            // Play the phone audio
            if (phoneAudioSource != null && !phoneAudioSource.isPlaying)
            {
                phoneAudioSource.Play();
                Debug.Log("Phone audio is now playing.");
            }

            // Handle the phone button interaction
            Debug.Log("Phone button pressed. Starting panel sequence.");
            StartCoroutine(ShowPanelsInSequence());

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

            // Mark the phone button as pressed
            isPhoneButtonPressed = true;
        }

        private System.Collections.IEnumerator ShowPanelsInSequence()
        {
            Debug.Log("Starting ShowPanelsInSequence coroutine...");
            int panelIndex = 0; // Track the index of the current panel

            foreach (var panel in phonePanels)
            {
                if (panel != null)
                {
                    Debug.Log($"Showing panel {panelIndex}: {panel.name}");
                    panel.SetActive(true); // Show the panel
                    yield return new WaitForSeconds(panelDisplayTime); // Wait for the specified time
                    panel.SetActive(false); // Hide the panel
                    Debug.Log($"Hiding panel {panelIndex}: {panel.name}");
                }
                else
                {
                    Debug.LogWarning($"Panel {panelIndex} is null. Skipping.");
                }
                panelIndex++;
            }

            // Disable the phone button after all panels are shown
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                Debug.Log("Phone button is now disabled after showing all panels.");
            }
            // Disable the AudioSource after all panels are shown
            if (phoneAudioSource != null)
            {
                phoneAudioSource.enabled = false;
                Debug.Log("Phone AudioSource has been disabled.");
            }

            Debug.Log("Finished showing all panels.");


        }
    }
} */