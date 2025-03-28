using UnityEngine;

public class Hexatile : MonoBehaviour
{
    private bool isPlayerAbove = false;
    private float timeAbove = 0f;
    private const float timeToDestroy = 3f; // Time in seconds to destroy the tile

    private void Update()
    {
        // Check if the player is above the hexatile
        if (isPlayerAbove)
        {
            timeAbove += Time.deltaTime; // Increment the timer

            if (timeAbove >= timeToDestroy)
            {
                DestroyHexatile();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            isPlayerAbove = true;
            Debug.Log($"Player is above the hexatile: {gameObject.name}");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerAbove = false;
            timeAbove = 0f; // Reset the timer when the player leaves
            Debug.Log($"Player exited the hexatile: {gameObject.name}");
        }
    }

    private void DestroyHexatile()
    {
        Debug.Log($"Destroying hexatile: {gameObject.name} after {timeToDestroy} seconds above.");
        Destroy(gameObject); // Destroy the hexatile
    }
}