using UnityEngine;
using UnityEngine.UI;

namespace ClassroomS3
{
    public class PhoneButtonManager1 : MonoBehaviour
    {
        public Button phoneButton; // Reference to the phone button
        public GameObject[] phonePanels; // Array of panels for the phone button interaction
        public float panelDisplayTime = 1.8f; // Time each panel is displayed

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

            Debug.Log("Finished showing all panels.");
        }
    }
}