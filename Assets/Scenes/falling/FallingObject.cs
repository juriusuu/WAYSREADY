/* using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float timePenalty = 5f; // Time to decrease when hitting the player
    public float fallThreshold = -10f; // Y-axis threshold for detecting a fall

    private void Update()
    {
        // Check if the object has fallen below the threshold
        if (transform.position.y < fallThreshold)
        {
            // Debug.Log($"Falling object fell out of bounds and was destroyed. Position: {transform.position}");
            Destroy(gameObject); // Destroy the falling object
        }
    }

    // Trigger for player interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object hits the player
        {
            Debug.Log("Falling object hit the player!");

            // Access the TaymerManager and decrease the time
            TaymerManager timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(-timePenalty); // Decrease time
                Debug.Log($"Time decreased by {timePenalty} seconds. Remaining time: {timerManager.remainingTime}");
            }

            Destroy(gameObject); // Destroy the falling object after it hits the player
        }
    }

    // Collision for ground interaction
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) // Check if the object hits the ground
        {
            Debug.Log("Falling object hit the ground and was destroyed.");
            Destroy(gameObject); // Destroy the falling object
        }
    }
} */

using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float timePenalty = 5f; // Time to decrease when hitting the player
    public float fallThreshold = -10f; // Y-axis threshold for detecting a fall
    private bool canHitPlayer = true; // Flag to control continuous hits
    public float hitCooldown = 1f; // Cooldown time between hits

    private void Update()
    {
        // Check if the object has fallen below the threshold
        if (transform.position.y < fallThreshold)
        {
            Destroy(gameObject); // Destroy the falling object
        }
    }

    // Trigger for player interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHitPlayer) // Check if the object hits the player
        {
            Debug.Log("Falling object hit the player!");

            // Access the TaymerManager and decrease the time
            TaymerManager timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager != null)
            {
                timerManager.AddTime(-timePenalty); // Decrease time
                Debug.Log($"Time decreased by {timePenalty} seconds. Remaining time: {timerManager.remainingTime}");
            }

            StartCoroutine(HitCooldown()); // Start cooldown before the object can hit again
        }
    }

    // Cooldown coroutine to allow continuous hits
    private System.Collections.IEnumerator HitCooldown()
    {
        canHitPlayer = false; // Disable further hits
        yield return new WaitForSeconds(hitCooldown); // Wait for the cooldown duration
        canHitPlayer = true; // Re-enable hits
    }

    // Collision for ground interaction
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) // Check if the object hits the ground
        {
            Debug.Log("Falling object hit the ground and was destroyed.");
            Destroy(gameObject); // Destroy the falling object
        }
    }
}