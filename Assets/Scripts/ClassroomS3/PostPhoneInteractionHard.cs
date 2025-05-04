using UnityEngine;

namespace ClassroomS3
{
    public class PostPhoneInteractionManagerHard : MonoBehaviour
    {
        public GameObject postPhonePanel; // Panel to display after the phone interaction
        public AudioSource audioSource; // Audio source to play with the panel
        private bool isActivated = false; // Tracks if the script has been activated

        private void Start()
        {
            // Ensure the panel is hidden at the start
            if (postPhonePanel != null)
            {
                postPhonePanel.SetActive(false);
            }

            // Ensure the audio source is not playing at the start
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        public void ActivatePostInteraction()
        {
            if (isActivated)
            {
                Debug.LogWarning("Post-phone interaction has already been activated.");
                return;
            }

            isActivated = true; // Mark as activated
            Debug.Log("Post-phone interaction activated!");

            // Show the panel and play the audio
            StartCoroutine(ShowPanelAndPlayAudio());
        }

        private System.Collections.IEnumerator ShowPanelAndPlayAudio()
        {
            // Show the panel
            if (postPhonePanel != null)
            {
                postPhonePanel.SetActive(true);
                Debug.Log("Post-phone panel displayed.");
            }

            // Play the audio
            if (audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Audio for post-phone panel started.");
            }

            // Wait for 2 seconds (or the duration of the panel display)
            yield return new WaitForSeconds(2f);

            // Hide the panel
            if (postPhonePanel != null)
            {
                postPhonePanel.SetActive(false);
                Debug.Log("Post-phone panel hidden.");
            }

            // Stop the audio
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Audio for post-phone panel stopped.");
            }
        }
    }
}