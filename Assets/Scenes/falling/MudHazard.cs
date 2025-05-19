using Supercyan.FreeSample;
using UnityEngine;

public class MudHazard : MonoBehaviour
{
    [SerializeField] private float slowAmount = 0.5f; // Player speed multiplier (e.g., 0.5 = 50% speed)
    [SerializeField] private float slowDuration = 5f; // Duration in seconds

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if clothes are active (player is protected)
            if (InventorySlot.IsClothesActive)
            {
                Debug.Log("Player is protected by clothes! No slow effect.");
                Destroy(gameObject);
                return;
            }

            // Replace 'PlayerMovement' with the actual movement script attached to the player if different
            Joystickscript playerMovement = other.GetComponent<Joystickscript>();
            if (playerMovement != null)
            {
                playerMovement.ApplySlow(slowAmount, slowDuration);
                Debug.Log("Player slowed by mud!");
            }

            Destroy(gameObject); // Remove mud after triggering
        }
    }
}