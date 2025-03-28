using UnityEngine;

public class DestructionSystem
 : MonoBehaviour
{
    public float timeToDestroy = 4f; // Time in seconds before destruction
    private float timer = 0f; // Timer to track time
    private bool playerOnPlatform = false; // Check if player is on the platform

    private void Update()
    {
        if (playerOnPlatform)
        {
            timer += Time.deltaTime; // Increment timer by the time since last frame
            if (timer >= timeToDestroy)
            {
                Destroy(gameObject); // Destroy the platform
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true; // Player is on the platform
            timer = 0f; // Reset timer
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false; // Player is no longer on the platform
            timer = 0f; // Reset timer
        }
    }
}