using UnityEngine;
using System.Collections;

using Supercyan.FreeSample;

public class WaveController : MonoBehaviour
{
    public float speed = 5f; // Speed of the leftward movement
    public float waveHeight = 0.5f; // Height of the wave oscillation
    public float waveFrequency = 1f; // Frequency of the wave oscillation
    public float startDelay = 1.5f; // Delay before the wave starts moving

    // Define the out-of-bounds position
    private float outOfBoundsX = 6.476028f; // X position to check against

    private void Start()
    {
        StartCoroutine(MoveWave());
    }

    private IEnumerator MoveWave()
    {
        // Wait for the specified delay before starting the movement
        yield return new WaitForSeconds(startDelay);

        while (true) // Infinite loop to keep the wave moving
        {
            // Move the object to the left
            transform.position += Vector3.left * speed * Time.deltaTime;

            // Create a wave effect (sine wave) for three overlapping waves
            float newY = Mathf.Sin(Time.time * waveFrequency) * waveHeight +
                         Mathf.Sin((Time.time * waveFrequency) + Mathf.PI / 3) * waveHeight + // First wave
                         Mathf.Sin((Time.time * waveFrequency) + 2 * Mathf.PI / 3) * waveHeight; // Second wave

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Check if the wave is out of bounds
            if (transform.position.x < outOfBoundsX)
            {
                Destroy(gameObject); // Destroy the wave object
                yield break; // Exit the coroutine
            }

            // Wait for the next frame
            yield return null; // This will pause the coroutine until the next frame
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Wave collided with: " + other.name);

        // Check if the wave collides with a sandbag
        if (other.CompareTag("Sandbag"))
        {
            // Check the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Current sandbag count before destruction: " + sandbagCount);

            // Only destroy the sandbag if there are fewer than 36 sandbags
            if (sandbagCount < 36)
            {
                Debug.Log("Collided with a sandbag: " + other.name);
                Destroy(other.gameObject); // Destroy the sandbag
                Debug.Log("Sandbag destroyed: " + other.name);
            }
            else
            {
                Debug.Log("Sufficient sandbags remain (36 or more). Sandbag will not be destroyed.");
            }
        }
        // Check if the wave collides with the player
        else if (other.CompareTag("Player"))
        {
            // Check the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Wave collided with player. Current sandbag count: " + sandbagCount);

            // If there are fewer than 36 sandbags, destroy the player
            if (sandbagCount < 36)
            {
                Debug.Log("Not enough sandbags. Destroying player: " + other.name);
                CharacterIt characterController = other.GetComponent<CharacterIt>(); // Get the CharacterIt component
                if (characterController != null)
                {
                    characterController.HandlePlayerDeath(); // Call the HandlePlayerDeath method
                }
                Destroy(other.gameObject); // Destroy the player
            }
            else
            {
                Debug.Log("Sufficient sandbags remain (36 or more). Player will not be destroyed.");
            }
        }
        else
        {
            Debug.Log("Wave collided with a non-sandbag and non-player object: " + other.name);
        }
    }
}


/* public class WaveController : MonoBehaviour
{
    public float speed = 5f; // Speed of the leftward movement
    public float waveHeight = 0.5f; // Height of the wave oscillation
    public float waveFrequency = 1f; // Frequency of the wave oscillation
    public float startDelay = 1.5f; // Delay before the wave starts moving

    // Define the out-of-bounds position
    private float outOfBoundsX = 6.476028f; // X position to check against

    private void Start()
    {
        StartCoroutine(MoveWave());
    }

    private IEnumerator MoveWave()
    {
        // Wait for the specified delay before starting the movement
        yield return new WaitForSeconds(startDelay);

        while (true) // Infinite loop to keep the wave moving
        {
            // Move the object to the left
            transform.position += Vector3.left * speed * Time.deltaTime;

            // Create a wave effect (sine wave) for three overlapping waves
            float newY = Mathf.Sin(Time.time * waveFrequency) * waveHeight +
                         Mathf.Sin((Time.time * waveFrequency) + Mathf.PI / 3) * waveHeight + // First wave
                         Mathf.Sin((Time.time * waveFrequency) + 2 * Mathf.PI / 3) * waveHeight; // Second wave

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Check if the wave is out of bounds
            if (transform.position.x < outOfBoundsX)
            {
                Destroy(gameObject); // Destroy the wave object
                yield break; // Exit the coroutine
            }

            // Wait for the next frame
            yield return null; // This will pause the coroutine until the next frame
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Wave collided with: " + other.name);

        // Check if the wave collides with a sandbag
        if (other.CompareTag("Sandbag"))
        {
            // Check the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Current sandbag count before destruction: " + sandbagCount);

            // Only destroy the sandbag if there are fewer than 36 sandbags
            if (sandbagCount < 36)
            {
                Debug.Log("Collided with a sandbag: " + other.name);
                Destroy(other.gameObject); // Destroy the sandbag
                Debug.Log("Sandbag destroyed: " + other.name);
            }
            else
            {
                Debug.Log("Sufficient sandbags remain (36 or more). Sandbag will not be destroyed.");
            }
        }
        // Check if the wave collides with the player
        else if (other.CompareTag("Player"))
        {
            // Check the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Wave collided with player. Current sandbag count: " + sandbagCount);

            // If there are fewer than 36 sandbags, destroy the player
            if (sandbagCount < 36)
            {
                Debug.Log("Not enough sandbags. Destroying player: " + other.name);
                Destroy(other.gameObject); // Destroy the player
            }
            else
            {
                Debug.Log("Sufficient sandbags remain (36 or more). Player will not be destroyed.");
            }
        }
        else
        {
            Debug.Log("Wave collided with a non-sandbag and non-player object: " + other.name);
        }
    }
} */