/* using UnityEngine;

public class WaterPuddleHazard : MonoBehaviour
{
    [SerializeField] private float timePenalty = 5f; // Amount of time to subtract
    private void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("Player"))

            // Check if towel is active
            if (InventorySlot.IsTowelActive)
            {
                Debug.Log("Player is protected by towel! No time penalty.");
                Destroy(gameObject);
                return;
            }
        {
            // Make the player slip
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Slip");
                Debug.Log("Player slipped on puddle!");
            }

            TaymerManager timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(-timePenalty); // Decrease time
                Debug.Log($"Time decreased by {timePenalty} seconds. Remaining time: {timerManager.remainingTime}");
            }
            Destroy(gameObject); // Destroy the puddle after triggering
        }
    }
} */using UnityEngine;

public class WaterPuddleHazard : MonoBehaviour
{
    [SerializeField] private float timePenalty = 5f; // Amount of time to subtract
    [SerializeField] private AudioClip slipSound; // Assign your slip sound in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if towel is active
            if (InventorySlot.IsTowelActive)
            {
                Debug.Log("Player is protected by towel! No time penalty.");
                Destroy(gameObject);
                return;
            }

            // Play slip sound
            if (slipSound != null)
            {
                AudioSource.PlayClipAtPoint(slipSound, transform.position);
            }

            // Make the player slip
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Slip");
                Debug.Log("Player slipped on puddle!");
            }

            TaymerManager timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(-timePenalty); // Decrease time
                Debug.Log($"Time decreased by {timePenalty} seconds. Remaining time: {timerManager.remainingTime}");
            }
            Destroy(gameObject); // Destroy the puddle after triggering
        }
    }
}