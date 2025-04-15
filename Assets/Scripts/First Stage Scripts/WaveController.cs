using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour
{
    public float speed = 5f; // Speed of the leftward movement
    public float waveHeight = 0.5f; // Height of the wave oscillation
    public float waveFrequency = 1f; // Frequency of the wave oscillation
    public float startDelay = 1.5f; // Delay before the wave starts moving

    private float outOfBoundsX = 6.476028f; // X position to check against
    private PanelManager panelManager; // Reference to the PanelManager

    private void Start()
    {
        // Find the PanelManager in the scene
        panelManager = FindObjectOfType<PanelManager>();

        if (panelManager == null)
        {
            Debug.LogError("PanelManager not found in the scene!");
        }

        StartCoroutine(MoveWave());
    }

    private IEnumerator MoveWave()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            float newY = Mathf.Sin(Time.time * waveFrequency) * waveHeight +
                         Mathf.Sin((Time.time * waveFrequency) + Mathf.PI / 3) * waveHeight +
                         Mathf.Sin((Time.time * waveFrequency) + 2 * Mathf.PI / 3) * waveHeight;

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (transform.position.x < outOfBoundsX)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wave collided with: " + other.name);

        if (other.CompareTag("Sandbag"))
        {
            HandleSandbagCollision(other);
        }
        else if (other.CompareTag("Player"))
        {
            HandlePlayerCollision(other);
        }
        else
        {
            Debug.Log("Wave collided with a non-sandbag and non-player object: " + other.name);
        }
    }

    private void HandleSandbagCollision(Collider sandbag)
    {
        int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;
        Debug.Log("Current sandbag count before destruction: " + sandbagCount);

        if (sandbagCount < 12)
        {
            Debug.Log("Collided with a sandbag: " + sandbag.name);
            Destroy(sandbag.gameObject);
            Debug.Log("Sandbag destroyed: " + sandbag.name);
        }

        // If there are 12 or more sandbags, show the finish panel
        if (GameObject.FindGameObjectsWithTag("Sandbag").Length >= 12)
        {
            Debug.Log("Sufficient sandbags remain. Showing finish panel.");
            panelManager?.ShowFinishPanel();
        }
    }

    private void HandlePlayerCollision(Collider player)
    {
        int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;
        Debug.Log("Wave collided with player. Current sandbag count: " + sandbagCount);

        if (sandbagCount < 11)
        {
            Debug.Log("Not enough sandbags. Destroying player: " + player.name);
            Destroy(player.gameObject);

            // Show the fail panel
            panelManager?.ShowFailPanel();
        }
        else
        {
            Debug.Log("Sufficient sandbags remain. Player will not be destroyed.");
        }
    }
}
/* using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour
{
    public float speed = 5f; // Speed of the leftward movement
    public float waveHeight = 0.5f; // Height of the wave oscillation
    public float waveFrequency = 1f; // Frequency of the wave oscillation
    public float startDelay = 1.5f; // Delay before the wave starts moving

    public GameObject failPanel; // Reference to the fail panel
    public GameObject finishPanel; // Reference to the finish panel

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
            HandleSandbagCollision(other);
        }
        // Check if the wave collides with the player
        else if (other.CompareTag("Player"))
        {
            HandlePlayerCollision(other);
        }
        else
        {
            Debug.Log("Wave collided with a non-sandbag and non-player object: " + other.name);
        }
    }

    private void HandleSandbagCollision(Collider sandbag)
    {
        // Check the current number of sandbags in the scene
        int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

        // Log the current count of sandbags
        Debug.Log("Current sandbag count before destruction: " + sandbagCount);

        // Only destroy the sandbag if there are fewer than 12 sandbags
        if (sandbagCount < 12)
        {
            Debug.Log("Collided with a sandbag: " + sandbag.name);
            Destroy(sandbag.gameObject); // Try to destroy the sandbag
            Debug.Log("Sandbag destroyed: " + sandbag.name);

            // Check if the sandbag was not destroyed (still exists in the scene)
            if (GameObject.FindGameObjectsWithTag("Sandbag").Length >= 12)
            {
                Debug.Log("Sandbag not destroyed. Showing finish panel.");
                if (finishPanel != null)
                {
                    finishPanel.SetActive(true); // Show the finish panel
                    Time.timeScale = 0f; // Pause the game
                }
            }
        }
        else
        {
            Debug.Log("Sufficient sandbags remain (12 or more). Sandbag will not be destroyed.");
        }
    }

    private void HandlePlayerCollision(Collider player)
    {
        // Check the current number of sandbags in the scene
        int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

        // Log the current count of sandbags
        Debug.Log("Wave collided with player. Current sandbag count: " + sandbagCount);

        // If there are fewer than 11 sandbags, destroy the player and show the fail panel
        if (sandbagCount < 11)
        {
            Debug.Log("Not enough sandbags. Destroying player: " + player.name);
            Destroy(player.gameObject); // Destroy the player

            // Show the fail panel
            if (failPanel != null)
            {
                Debug.Log("Showing fail panel.");
                failPanel.SetActive(true); // Show the fail panel
                Time.timeScale = 0f; // Pause the game
            }
        }
        else
        {
            Debug.Log("Sufficient sandbags remain (11 or more). Player will not be destroyed.");
        }
    }
} */
/* 
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
            if (sandbagCount < 12)
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
            if (sandbagCount < 11)
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

 */
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