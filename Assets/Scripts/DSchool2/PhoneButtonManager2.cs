using UnityEngine;
using UnityEngine.UI;

namespace ClassroomS3
{
    public class PhoneButtonManager2 : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed
        public AudioSource phoneAudioSource; // Reference to the phone's AudioSource
        public AudioSource ringAudioSource; // Reference to the ring AudioSource

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

        private RadioInteractionManagerS4 radioInteractionManagerS4;

        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
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

        private void OnPhoneButtonPressed()
        {
            if (isPhoneButtonPressed)
            {
                Debug.Log("Phone button already pressed. Ignoring further presses.");
                return;
            }

            // Play the ring sound for 1 second
            if (ringAudioSource != null)
            {
                ringAudioSource.Play();
                Debug.Log("Ring sound started!");
                StartCoroutine(StopRingAudioAfterDelay(1f));
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
                Debug.Log("Ring sound stopped after 1 second.");
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

            // After showing all panels, complete the task and activate the radio button
            CompleteTaskAndActivateRadioButton();

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

        private void CompleteTaskAndActivateRadioButton()
        {
            // Mark the "Use the phone" task as completed
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(0); // Assuming this is the second task
                Debug.Log("Quest task 'Use the phone' marked as completed.");
            }
            else
            {
                Debug.LogWarning("QuestClipboardManagerS3 not found in the scene.");
            }

            // Activate the radio button
            if (radioInteractionManagerS4 != null)
            {
                radioInteractionManagerS4.ActivateRadioButton();
                Debug.Log("Radio button activated.");
            }
            else
            {
                Debug.LogWarning("RadioInteractionManagerS4 not found in the scene.");
            }
        }
    }
}



/* using UnityEngine;
using UnityEngine.UI;

namespace ClassroomS3
{
    public class PhoneButtonManager2 : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed
        public AudioSource phoneAudioSource; // Reference to the phone's AudioSource

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

        private RadioInteractionManagerS4 radioInteractionManagerS4;

        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
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

            // After showing all panels, complete the task and activate the radio button
            CompleteTaskAndActivateRadioButton();

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

        private void CompleteTaskAndActivateRadioButton()
        {
            // Mark the "Use the phone" task as completed
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(0); // Assuming this is the second task
                Debug.Log("Quest task 'Use the phone' marked as completed.");
            }
            else
            {
                Debug.LogWarning("QuestClipboardManagerS3 not found in the scene.");
            }

            // Activate the radio button
            if (radioInteractionManagerS4 != null)
            {
                radioInteractionManagerS4.ActivateRadioButton();
                Debug.Log("Radio button activated.");
            }
            else
            {
                Debug.LogWarning("RadioInteractionManagerS4 not found in the scene.");
            }
        }
    }
}

 */
/* using UnityEngine;
using UnityEngine.UI;

namespace ClassroomS3
{
    public class PhoneButtonManager2 : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed

        private bool isPhoneButtonPressed = false; // Tracks if the phone button has been pressed

        private RadioInteractionManagerS4 radioInteractionManagerS4;

        private void Start()
        {
            // Ensure the phone button is hidden at the start
            if (phoneButton != null)
            {
                //  phoneButton.gameObject.SetActive(false);
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
        }

        private void OnPhoneButtonPressed()
        {
            if (isPhoneButtonPressed)
            {
                Debug.Log("Phone button already pressed. Ignoring further presses.");
                return;
            }

            // Handle the phone button interaction
            Debug.Log("Phone button pressed. Starting panel sequence.");
            StartCoroutine(ShowPanelsInSequence());

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

            // After showing all panels, complete the task and activate the radio button
            CompleteTaskAndActivateRadioButton();

            // Disable the phone button after all panels are shown
            if (phoneButton != null)
            {
                phoneButton.gameObject.SetActive(false);
                Debug.Log("Phone button is now disabled after showing all panels.");
            }

            Debug.Log("Finished showing all panels.");
        }

        private void CompleteTaskAndActivateRadioButton()
        {
            // Mark the "Use the phone" task as completed
            var questManager = FindObjectOfType<QuestClipboardManager>();
            if (questManager != null)
            {
                questManager.CompleteTask(0); // Assuming this is the second task
                Debug.Log("Quest task 'Use the phone' marked as completed.");
            }
            else
            {
                Debug.LogWarning("QuestClipboardManagerS3 not found in the scene.");
            }

            // Activate the radio button
            if (radioInteractionManagerS4 != null)
            {
                radioInteractionManagerS4.ActivateRadioButton();
                Debug.Log("Radio button activated.");
            }
            else
            {
                Debug.LogWarning("RadioInteractionManagerS4 not found in the scene.");
            }
        }
    }
} */