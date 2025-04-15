using UnityEngine;
using Supercyan.FreeSample; // Include the namespace for CharacterIt

public class WaveCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Triggered by: " + other.name);

        // Check if the object that entered the trigger is the wave itself
        if (other.CompareTag("Wave"))
        {
            // Count the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Current sandbag count: " + sandbagCount);

            // Check if the current count of sandbags is greater than or equal to 12
            if (sandbagCount >= 12)
            {
                Debug.Log("Wave destroyed due to high sandbag count.");
                Destroy(other.gameObject); // Destroy the wave
            }
            else if (sandbagCount < 11) // Change this condition
            {
                // If there are fewer than 11 sandbags, destroy the player
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player by tag
                if (player != null)
                {
                    Debug.Log("Not enough sandbags. Destroying player.");
                    CharacterIt characterController = player.GetComponent<CharacterIt>(); // Get the CharacterIt component
                    if (characterController != null)
                    {

                        characterController.HandlePlayerDeath(); // Call the HandlePlayerDeath method
                    }
                    else
                    {
                        Debug.LogWarning("CharacterIt component not found on player!");
                    }
                }
                else
                {
                    Debug.LogWarning("Player not found!");
                }
            }
            else
            {
                Debug.Log("Wave remains intact. Current sandbag count is: " + sandbagCount);
            }

            // Log the number of sandbags left after the collision
            sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length; // Recount sandbags
            Debug.Log("Sandbags left after wave collision: " + sandbagCount);
        }
    }
}
/* public class WaveCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Triggered by: " + other.name);

        // Check if the object that entered the trigger is the wave itself
        if (other.CompareTag("Wave"))
        {
            // Count the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Current sandbag count: " + sandbagCount);

            // Check if the current count of sandbags is greater than or equal to 36
            if (sandbagCount >= 36)
            {
                Debug.Log("Wave destroyed due to high sandbag count.");
                Destroy(other.gameObject); // Destroy the wave
            }
            else if (sandbagCount < 35) // Change this condition
            {
                // If there are fewer than 35 sandbags, destroy the player
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player by tag
                if (player != null)
                {
                    Debug.Log("Not enough sandbags. Destroying player.");
                    CharacterIt characterController = player.GetComponent<CharacterIt>(); // Get the CharacterIt component
                    if (characterController != null)
                    {
                        characterController.HandlePlayerDeath(); // Call the HandlePlayerDeath method
                    }
                    else
                    {
                        Debug.LogWarning("CharacterIt component not found on player!");
                    }
                }
                else
                {
                    Debug.LogWarning("Player not found!");
                }
            }
            else
            {
                Debug.Log("Wave remains intact. Current sandbag count is: " + sandbagCount);
            }

            // Log the number of sandbags left after the collision
            sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length; // Recount sandbags
            Debug.Log("Sandbags left after wave collision: " + sandbagCount);
        }
    }
} */

/*public class WaveCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Log the name of the object that entered the trigger
        Debug.Log("Triggered by: " + other.name);

        // Check if the object that entered the trigger is the wave itself
        if (other.CompareTag("Wave"))
        {
            // Count the current number of sandbags in the scene
            int sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length;

            // Log the current count of sandbags
            Debug.Log("Current sandbag count: " + sandbagCount);

            // Check if the current count of sandbags is greater than or equal to 36
            if (sandbagCount >= 36)
            {
                Debug.Log("Wave destroyed due to high sandbag count.");
                Destroy(other.gameObject); // Destroy the wave
            }
            else if (sandbagCount < 35) // Change this condition
            {
                // If there are fewer than 35 sandbags, destroy the player
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player by tag
                if (player != null)
                {
                    Debug.Log("Not enough sandbags. Destroying player.");
                    Destroy(player); // Destroy the player
                }
                else
                {
                    Debug.LogWarning("Player not found!");
                }
            }
            else
            {
                Debug.Log("Wave remains intact. Current sandbag count is: " + sandbagCount);
            }

            // Log the number of sandbags left after the collision
            sandbagCount = GameObject.FindGameObjectsWithTag("Sandbag").Length; // Recount sandbags
            Debug.Log("Sandbags left after wave collision: " + sandbagCount);
        }
    }
}

*/