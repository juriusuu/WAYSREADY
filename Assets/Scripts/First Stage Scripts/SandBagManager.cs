
using UnityEngine;
using System.Collections;

using System.Collections.Generic;


using UnityEngine;
using System.Collections;

public class SandBagManager : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    public Vector3 areaCenter; // Center of the allowed area
    public Vector3 areaSize; // Size of the allowed area

    public Transform dropPoint; // Assign a drop point in the inspector
    private bool canDrop = false; // Flag to check if the player can drop sandbags
    private bool hasDroppedSandbags = false; // Flag to check if sandbags have already been dropped

    public GameObject rainmakerPrefab;  // Assign your rainmaker prefab in the inspector
    public GameObject cloudPrefab; // Assign your cloud prefab in the inspector
    public GameObject wavePrefab;   // Assign your wave prefab in the inspector
    public float spawnDelay = 4f; // Delay before spawning the wave

    private void Start()
    {
        // Ensure the InventoryManager is assigned
        if (inventoryManager == null)
        {
            inventoryManager = InventoryManager.Instance; // Attempt to get the singleton instance
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager reference is not set in SandBagManager and could not be found!");
            }
            else
            {
                Debug.Log("InventoryManager successfully assigned in SandBagManager.");
            }
        }
    }

    // Method to drop a sandbag
    public void TryDropSandbag(string type)
    {
        // Check if the inventory manager is assigned
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference is not set in SandBagManager. Cannot drop sandbag.");
            return;
        }

        // Check if sandbags have already been dropped
        if (hasDroppedSandbags)
        {
            Debug.LogWarning("Sandbags have already been dropped. Cannot drop again.");
            return;
        }

        Debug.Log($"Attempting to drop {type} sandbag. Inventory Count: {inventoryManager.GetSandbagCount(type)}, Can Drop: {canDrop}");

        // Check if the inventory contains the specified type of sandbag and if there are any available
        if (inventoryManager.GetSandbagCount(type) > 0 && canDrop)
        {
            // Use the drop point's position
            Vector3 dropPosition = dropPoint.position;

            // Check if the drop position is valid (within the allowed area)
            if (IsWithinArea(dropPosition))
            {
                DropSandbag(dropPosition, type);
                hasDroppedSandbags = true; // Set the flag to true after dropping
            }
            else
            {
                Debug.LogWarning("Drop position is outside the allowed area.");
                Debug.Log($"Drop Position: {dropPosition}");
                Debug.Log($"Area Center: {areaCenter}, Area Size: {areaSize}");
            }
        }
        else
        {
            Debug.Log($"No {type} sandbags available to drop or not in drop area.");
        }
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Check if the position is within the defined area
        bool isWithinX = position.x >= areaCenter.x - areaSize.x / 2 && position.x <= areaCenter.x + areaSize.x / 2;
        bool isWithinZ = position.z >= areaCenter.z - areaSize.z / 2 && position.z <= areaCenter.z + areaSize.z / 2;
        bool isWithinY = position.y >= areaCenter.y - areaSize.y / 2 && position.y <= areaCenter.y + areaSize.y / 2;

        Debug.Log($"Checking position: {position}, IsWithinX: {isWithinX}, IsWithinZ: {isWithinZ}, IsWithinY: {isWithinY}");

        return isWithinX && isWithinZ; // Return true if within X and Z bounds
    }

    private void DropSandbag(Vector3 playerPosition, string type)
    {
        // Set the desired rotation for the sandbag
        Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

        // Get the number of sandbags available in the inventory
        int totalSandbags = inventoryManager.GetSandbagCount(type);
        Debug.Log($"Initial total sandbags: {totalSandbags}");


        StartCoroutine(SpawnRainmakerWithDelay(playerPosition));
        StartCoroutine(SpawnWave());
        // Destroy existing sandbags in the scene
        DestroyExistingSandbags();

        // If there are no sandbags, exit the method
        if (totalSandbags <= 0)
        {
            Debug.Log("No sandbags available to drop.");
            return;
        }

        // Define the effective width of the sandbag based on its scale
        float radius = 1.45f; // Set the radius to 2f to place sandbags very close to the player
        float stackingHeightIncrement = 0.6f; // Height increment for stacking sandbags
        int maxSandbagsPerLayer = 12; // Set maximum number of sandbags per layer to 12 for a circular formation

        int layers = Mathf.CeilToInt((float)totalSandbags / maxSandbagsPerLayer); // Calculate the number of layers
        Debug.Log($"Calculated layers: {layers}");

        for (int layer = 0; layer < layers; layer++)
        {
            // Calculate the current height for this layer
            float currentHeight = stackingHeightIncrement * layer;

            // Determine how many sandbags to place in this layer
            int sandbagsInLayer = Mathf.Min(maxSandbagsPerLayer, totalSandbags);
            Debug.Log($"Layer {layer}: Placing {sandbagsInLayer} sandbags.");

            // Loop to create sandbags in a circular pattern for this layer
            for (int i = 0; i < sandbagsInLayer; i++)
            {
                // Calculate the angle for the current sandbag
                float angle = i * (360f / sandbagsInLayer);
                float radian = angle * Mathf.Deg2Rad;

                // Calculate the position for the current sandbag
                Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, currentHeight, Mathf.Sin(radian) * radius);
                Vector3 sandbagPosition = playerPosition + offset;

                // Instantiate the sandbag at the calculated position with the specified rotation
                GameObject sandbag = Instantiate(sandbagPrefab, sandbagPosition, rotation);

                // Decrease the count in the inventory
                inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

                // Decrease the total sandbags left
                totalSandbags--;
                Debug.Log($"Dropped sandbag. Remaining sandbags: {totalSandbags}");
            }
        }

        // Log a message to the console indicating that sandbags have been placed
        Debug.Log($"Placed sandbags to form a round fortress around the player at position: {playerPosition} with rotation: {rotation.eulerAngles}");
    }

    private void DestroyExistingSandbags()
    {
        // Find all sandbags in the scene and destroy them
        GameObject[] existingSandbags = GameObject.FindGameObjectsWithTag("Sandbag"); // Assuming sandbags are tagged as "Sandbag"
        foreach (GameObject sandbag in existingSandbags)
        {
            Destroy(sandbag);
        }
        Debug.Log("Destroyed existing sandbags in the scene.");
    }

    // Trigger methods to check if the player is in the drop area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has the tag "Player"
        {
            canDrop = true; // Allow dropping sandbags
            Debug.Log("Player entered drop area. Can drop: " + canDrop);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDrop = false; // Disallow dropping sandbags
            Debug.Log("Player exited drop area. Can drop: " + canDrop);
        }
    }

    // Method to reset the state of the SandBagManager
    public void ResetState()
    {
        canDrop = false; // Reset the ability to drop sandbags
        hasDroppedSandbags = false; // Allow dropping sandbags again
        Debug.Log("SandBagManager state has been reset.");
    }

    private System.Collections.IEnumerator SpawnRainmakerWithDelay(Vector3 position)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

        GameObject rainmaker = null;
        GameObject cloud = null;

        if (rainmakerPrefab != null)
        {
            rainmaker = Instantiate(rainmakerPrefab, position, Quaternion.identity);
            Debug.Log("Rainmaker summoned at position: " + position);
        }

        // Instantiate the cloud prefab
        if (cloudPrefab != null)
        {
            // You can adjust the position of the cloud if needed
            Vector3 cloudPosition = position + new Vector3(0, 7, 0); // Example: spawn the cloud 7 units above the rainmaker

            // Set the desired rotation for the cloud
            Quaternion cloudRotation = Quaternion.Euler(-89.98f, 0, 0); // Set rotation to -89.98 degrees on the X-axis

            cloud = Instantiate(cloudPrefab, cloudPosition, cloudRotation);
            Debug.Log("Cloud summoned at position: " + cloudPosition + " with rotation: " + cloudRotation.eulerAngles);
        }

        // Wait for 10 seconds before destroying the rainmaker and cloud
        yield return new WaitForSeconds(10f);

        // Destroy the rainmaker and cloud if they were instantiated
        if (rainmaker != null)
        {
            Destroy(rainmaker);
            Debug.Log("Rainmaker destroyed.");
        }

        if (cloud != null)
        {
            Destroy(cloud);
            Debug.Log("Cloud destroyed.");
        }
    }


    private IEnumerator SpawnWave()
    {
        // Wait for the specified delay before spawning the wave
        yield return new WaitForSeconds(spawnDelay);

        // Instantiate the wave prefab at the desired position and rotation
        Vector3 wavePosition = new Vector3(11.07968f, 0.5f, 1.878922f); // Set the position
        GameObject wave = Instantiate(wavePrefab, wavePosition, Quaternion.identity); // Instantiate the wave prefab

        if (wave != null)
        {
            Debug.Log("Wave spawned successfully at position: " + wavePosition);
        }
        else
        {
            Debug.Log("Failed to spawn wave. Wave prefab is null.");
        }
    }
}
// Calculate the current height for this layer
/* Changed March 15
public class SandBagManager : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    public Vector3 areaCenter; // Center of the allowed area
    public Vector3 areaSize; // Size of the allowed area

    public Transform dropPoint; // Assign a drop point in the inspector
    private bool canDrop = false; // Flag to check if the player can drop sandbags
    private bool hasDroppedSandbags = false; // Flag to check if sandbags have already been dropped

    public GameObject rainmakerPrefab;  // Assign your rainmaker prefab in the inspector
    public GameObject cloudPrefab; // Assign your cloud prefab in the inspector

    public GameObject wavePrefab;   // Assign your wave prefab in the inspector

    public float spawnDelay = 4f; // Delay before spawning the wave

    // Method to drop a sandbag
    public void TryDropSandbag(string type)
    {
        // Check if the inventory manager is assigned
        if (inventoryManager == null)
        {
            Debug.Log("InventoryManager reference is not set in SandBagManager.");
            return;
        }

        // Check if sandbags have already been dropped
        if (hasDroppedSandbags)
        {
            Debug.LogWarning("Sandbags have already been dropped. Cannot drop again.");
            return;
        }

        Debug.Log($"Attempting to drop {type} sandbag. Inventory Count: {inventoryManager.GetSandbagCount(type)}, Can Drop: {canDrop}");

        // Check if the inventory contains the specified type of sandbag and if there are any available
        if (inventoryManager.GetSandbagCount(type) > 0 && canDrop)
        {
            // Use the drop point's position
            Vector3 dropPosition = dropPoint.position;

            // Check if the drop position is valid (within the allowed area)
            if (IsWithinArea(dropPosition))
            {
                DropSandbag(dropPosition, type);
                hasDroppedSandbags = true; // Set the flag to true after dropping
            }
            else
            {
                Debug.LogWarning("Drop position is outside the allowed area.");
                Debug.Log($"Drop Position: {dropPosition}");
                Debug.Log($"Area Center: {areaCenter}, Area Size: {areaSize}");
            }
        }
        else
        {
            Debug.Log($"No {type} sandbags available to drop or not in drop area.");
        }
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Check if the position is within the defined area
        bool isWithinX = position.x >= areaCenter.x - areaSize.x / 2 && position.x <= areaCenter.x + areaSize.x / 2;
        bool isWithinZ = position.z >= areaCenter.z - areaSize.z / 2 && position.z <= areaCenter.z + areaSize.z / 2;

        // Optional: Check if the Y position is within a certain range if needed
        bool isWithinY = position.y >= areaCenter.y - areaSize.y / 2 && position.y <= areaCenter.y + areaSize.y / 2;

        Debug.Log($"Checking position: {position}, IsWithinX: {isWithinX}, IsWithinZ: {isWithinZ}, IsWithinY: {isWithinY}");

        return isWithinX && isWithinZ; // Return true if within X and Z bounds
                                       // If you want to include Y, return isWithinX && isWithinZ && isWithinY;
    }
    private System.Collections.IEnumerator SpawnRainmakerWithDelay(Vector3 position)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second

        GameObject rainmaker = null;
        GameObject cloud = null;

        if (rainmakerPrefab != null)
        {
            rainmaker = Instantiate(rainmakerPrefab, position, Quaternion.identity);
            Debug.Log("Rainmaker summoned at position: " + position);
        }

        // Instantiate the cloud prefab
        if (cloudPrefab != null)
        {
            // You can adjust the position of the cloud if needed
            Vector3 cloudPosition = position + new Vector3(0, 7, 0); // Example: spawn the cloud 7 units above the rainmaker

            // Set the desired rotation for the cloud
            Quaternion cloudRotation = Quaternion.Euler(-89.98f, 0, 0); // Set rotation to -89.98 degrees on the X-axis

            cloud = Instantiate(cloudPrefab, cloudPosition, cloudRotation);
            Debug.Log("Cloud summoned at position: " + cloudPosition + " with rotation: " + cloudRotation.eulerAngles);
        }

        // Wait for 10 seconds before destroying the rainmaker and cloud
        yield return new WaitForSeconds(10f);

        // Destroy the rainmaker and cloud if they were instantiated
        if (rainmaker != null)
        {
            Destroy(rainmaker);
            Debug.Log("Rainmaker destroyed.");
        }

        if (cloud != null)
        {
            Destroy(cloud);
            Debug.Log("Cloud destroyed.");
        }
    }


    private IEnumerator SpawnWave()
    {
        // Wait for the specified delay before spawning the wave
        yield return new WaitForSeconds(spawnDelay);

        // Instantiate the wave prefab at the desired position and rotation
        Vector3 wavePosition = new Vector3(11.07968f, 0.5f, 1.878922f); // Set the position
        GameObject wave = Instantiate(wavePrefab, wavePosition, Quaternion.identity); // Instantiate the wave prefab

        if (wave != null)
        {
            Debug.Log("Wave spawned successfully at position: " + wavePosition);
        }
        else
        {
            Debug.Log("Failed to spawn wave. Wave prefab is null.");
        }
    }

    private void DropSandbag(Vector3 playerPosition, string type)
    {
        // Set the desired rotation for the sandbag
        Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

        // Get the number of sandbags available in the inventory
        int totalSandbags = inventoryManager.GetSandbagCount(type);
        Debug.Log($"Initial total sandbags: {totalSandbags}");

        // Destroy existing sandbags in the scene
        DestroyExistingSandbags();

        StartCoroutine(SpawnRainmakerWithDelay(playerPosition));
        StartCoroutine(SpawnWave());

        // If there are no sandbags, exit the method
        if (totalSandbags <= 0)
        {
            Debug.Log("No sandbags available to drop.");
            return;
        }

        // Define the effective width of the sandbag based on its scale
        float radius = 1.45f; // Set the radius to 2f to place sandbags very close to the player
        float stackingHeightIncrement = 0.6f; // Height increment for stacking sandbags
        int maxSandbagsPerLayer = 12; // Set maximum number of sandbags per layer to 12 for a circular formation

        int layers = Mathf.CeilToInt((float)totalSandbags / maxSandbagsPerLayer); // Calculate the number of layers
        Debug.Log($"Calculated layers: {layers}");

        for (int layer = 0; layer < layers; layer++)
        {
            // Calculate the current height for this layer
            float currentHeight = stackingHeightIncrement * layer;

            // Determine how many sandbags to place in this layer
            int sandbagsInLayer = Mathf.Min(maxSandbagsPerLayer, totalSandbags);
            Debug.Log($"Layer {layer}: Placing {sandbagsInLayer} sandbags.");

            // Loop to create sandbags in a circular pattern for this layer
            for (int i = 0; i < sandbagsInLayer; i++)
            {
                // Calculate the angle for the current sandbag
                float angle = i * (360f / sandbagsInLayer);
                float radian = angle * Mathf.Deg2Rad;

                // Calculate the position for the current sandbag
                Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, currentHeight, Mathf.Sin(radian) * radius);
                Vector3 sandbagPosition = playerPosition + offset;

                // Instantiate the sandbag at the calculated position with the specified rotation
                GameObject sandbag = Instantiate(sandbagPrefab, sandbagPosition, rotation);

                // Decrease the count in the inventory
                inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

                // Decrease the total sandbags left
                totalSandbags--;
                Debug.Log($"Dropped sandbag. Remaining sandbags: {totalSandbags}");
            }
        }

        // Log a message to the console indicating that sandbags have been placed
        Debug.Log($"Placed sandbags to form a round fortress around the player at position: {playerPosition} with rotation: {rotation.eulerAngles}");
    }

    private void DestroyExistingSandbags()
    {
        // Find all sandbags in the scene and destroy them
        GameObject[] existingSandbags = GameObject.FindGameObjectsWithTag("Sandbag"); // Assuming sandbags are tagged as "Sandbag"
        foreach (GameObject sandbag in existingSandbags)
        {
            Destroy(sandbag);
        }
        Debug.Log("Destroyed existing sandbags in the scene.");
    }
    // Trigger methods to check if the player is in the drop area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has the tag "Player"
        {
            canDrop = true; // Allow dropping sandbags
            Debug.Log("Player entered drop area. Can drop: " + canDrop);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDrop = false; // Disallow dropping sandbags
            Debug.Log("Player exited drop area. Can drop: " + canDrop);
        }
    }
    //updated March 15
    public void ResetState()
    {
        canDrop = false; // Reset the ability to drop sandbags
        hasDroppedSandbags = false; // Allow dropping sandbags again
        Debug.Log("SandBagManager state has been reset.");
    }
}


 */

/*     private void DropSandbag(Vector3 playerPosition, string type)
    {
        // Set the desired rotation for the sandbag
        Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

        // Get the number of sandbags available in the inventory
        int totalSandbags = inventoryManager.GetSandbagCount(type);
        Debug.Log($"Initial total sandbags: {totalSandbags}");

        StartCoroutine(SpawnRainmakerWithDelay(playerPosition));
        StartCoroutine(SpawnWave());

        // If there are no sandbags, exit the method
        if (totalSandbags <= 0)
        {
            Debug.Log("No sandbags available to drop.");
            return;
        }

        // Define the effective width of the sandbag based on its scale
        float radius = 1.45f; // Set the radius to 2f to place sandbags very close to the player
        float stackingHeightIncrement = 0.6f; // Height increment for stacking sandbags
        int maxSandbagsPerLayer = 12; // Set maximum number of sandbags per layer to 12 for a circular formation

        int layers = Mathf.CeilToInt((float)totalSandbags / maxSandbagsPerLayer); // Calculate the number of layers
        Debug.Log($"Calculated layers: {layers}");

        for (int layer = 0; layer < layers; layer++)
        {
            // Calculate the current height for this layer
            float currentHeight = stackingHeightIncrement * layer;

            // Determine how many sandbags to place in this layer
            int sandbagsInLayer = Mathf.Min(maxSandbagsPerLayer, totalSandbags);
            Debug.Log($"Layer {layer}: Placing {sandbagsInLayer} sandbags.");

            // Loop to create sandbags in a circular pattern for this layer
            for (int i = 0; i < sandbagsInLayer; i++)
            {
                // Calculate the angle for the current sandbag
                float angle = i * (360f / sandbagsInLayer);
                float radian = angle * Mathf.Deg2Rad;

                // Calculate the position for the current sandbag
                Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, currentHeight, Mathf.Sin(radian) * radius);
                Vector3 sandbagPosition = playerPosition + offset;

                // Instantiate the sandbag at the calculated position with the specified rotation
                GameObject sandbag = Instantiate(sandbagPrefab, sandbagPosition, rotation);

                // Decrease the count in the inventory
                inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

                // Decrease the total sandbags left
                totalSandbags--;
                Debug.Log($"Dropped sandbag. Remaining sandbags: {totalSandbags}");
            }
        }

        // Log a message to the console indicating that sandbags have been placed
        Debug.Log($"Placed sandbags to form a round fortress around the player at position: {playerPosition} with rotation: {rotation.eulerAngles}");
    } */
/*
public class SandBagManager : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    public Vector3 areaCenter; // Center of the allowed area
    public Vector3 areaSize; // Size of the allowed area

    public Transform dropPoint; // Assign a drop point in the inspector
    private bool canDrop = false; // Flag to check if the player can drop sandbags

    public GameObject rainmakerPrefab;  // Assign your rainmaker prefab in the inspector

    public GameObject cloudPrefab; // Assign your cloud prefab in the inspector


    // Method to drop a sandbag
    public void TryDropSandbag(string type)
    {
        // Check if the inventory manager is assigned
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager reference is not set in SandBagManager.");
            return;
        }
        Debug.Log($"Attempting to drop {type} sandbag. Inventory Count: {inventoryManager.GetSandbagCount(type)}, Can Drop: {canDrop}");

        // Check if the inventory contains the specified type of sandbag and if there are any available
        if (inventoryManager.GetSandbagCount(type) > 0 && canDrop)
        {
            // Use the drop point's position
            Vector3 dropPosition = dropPoint.position;

            // Check if the drop position is valid (within the allowed area)
            if (IsWithinArea(dropPosition))
            {
                DropSandbag(dropPosition, type);
            }
            else
            {
                Debug.LogWarning("Drop position is outside the allowed area.");
                Debug.Log($"Drop Position: {dropPosition}");
                Debug.Log($"Area Center: {areaCenter}, Area Size: {areaSize}");
            }
        }
        else
        {
            Debug.Log($"No {type} sandbags available to drop or not in drop area.");
        }
    }

    private System.Collections.IEnumerator SpawnRainmakerWithDelay(Vector3 position)
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds

        if (rainmakerPrefab != null)
        {
            Instantiate(rainmakerPrefab, position, Quaternion.identity);
            Debug.Log("Rainmaker summoned at position: " + position);
        }

        // Instantiate the cloud prefab
        if (cloudPrefab != null)
        {
            // You can adjust the position of the cloud if needed
            Vector3 cloudPosition = position + new Vector3(0, 7, 0); // Example: spawn the cloud 7 units above the rainmaker
            Instantiate(cloudPrefab, cloudPosition, Quaternion.identity);
            Debug.Log("Cloud summoned at position: " + cloudPosition);
        }
    }


    private bool IsWithinArea(Vector3 position)
    {
        // Check if the position is within the defined area
        bool isWithinX = position.x >= areaCenter.x - areaSize.x / 2 && position.x <= areaCenter.x + areaSize.x / 2;
        bool isWithinZ = position.z >= areaCenter.z - areaSize.z / 2 && position.z <= areaCenter.z + areaSize.z / 2;

        // Optional: Check if the Y position is within a certain range if needed
        bool isWithinY = position.y >= areaCenter.y - areaSize.y / 2 && position.y <= areaCenter.y + areaSize.y / 2;

        Debug.Log($"Checking position: {position}, IsWithinX: {isWithinX}, IsWithinZ: {isWithinZ}, IsWithinY: {isWithinY}");

        return isWithinX && isWithinZ; // Return true if within X and Z bounds
                                       // If you want to include Y, return isWithinX && isWithinZ && isWithinY;
    }
    private void DropSandbag(Vector3 playerPosition, string type)
    {
        // Set the desired rotation for the sandbag
        Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

        // Get the number of sandbags available in the inventory
        int totalSandbags = inventoryManager.GetSandbagCount(type);
        Debug.Log($"Initial total sandbags: {totalSandbags}");

        StartCoroutine(SpawnRainmakerWithDelay(playerPosition));

        // If there are no sandbags, exit the method
        if (totalSandbags <= 0)
        {
            Debug.Log("No sandbags available to drop.");
            return;
        }

        // Define the effective width of the sandbag based on its scale
        float radius = 1.45f; // Set the radius to 2f to place sandbags very close to the player
        float stackingHeightIncrement = 0.6f; // Height increment for stacking sandbags
        int maxSandbagsPerLayer = 12; // Set maximum number of sandbags per layer to 12 for a circular formation

        int layers = Mathf.CeilToInt((float)totalSandbags / maxSandbagsPerLayer); // Calculate the number of layers
        Debug.Log($"Calculated layers: {layers}");

        for (int layer = 0; layer < layers; layer++)
        {
            // Calculate the current height for this layer
            float currentHeight = stackingHeightIncrement * layer;

            // Determine how many sandbags to place in this layer
            int sandbagsInLayer = Mathf.Min(maxSandbagsPerLayer, totalSandbags);
            Debug.Log($"Layer {layer}: Placing {sandbagsInLayer} sandbags.");

            // Loop to create sandbags in a circular pattern for this layer
            for (int i = 0; i < sandbagsInLayer; i++)
            {
                // Calculate the angle for the current sandbag
                float angle = i * (360f / sandbagsInLayer);
                float radian = angle * Mathf.Deg2Rad;

                // Calculate the position for the current sandbag
                Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, currentHeight, Mathf.Sin(radian) * radius);
                Vector3 sandbagPosition = playerPosition + offset;

                // Instantiate the sandbag at the calculated position with the specified rotation
                GameObject sandbag = Instantiate(sandbagPrefab, sandbagPosition, rotation);

                // Decrease the count in the inventory
                inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

                // Decrease the total sandbags left
                totalSandbags--;
                Debug.Log($"Dropped sandbag. Remaining sandbags: {totalSandbags}");
            }
        }

        // Log a message to the console indicating that sandbags have been placed
        Debug.Log($"Placed sandbags to form a round fortress around the player at position: {playerPosition} with rotation: {rotation.eulerAngles}");
    }

    // Trigger methods to check if the player is in the drop area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming your player has the tag "Player"
        {
            canDrop = true; // Allow dropping sandbags
            Debug.Log("Player entered drop area. Can drop: " + canDrop);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDrop = false; // Disallow dropping sandbags
            Debug.Log("Player exited drop area. Can drop: " + canDrop);
        }
    }
}
/*

/*    private void DropSandbag(Vector3 playerPosition, string type)
{
    // Set the desired rotation for the sandbag
    Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

    // Get the number of sandbags available in the inventory
    int totalSandbags = inventoryManager.GetSandbagCount(type);

    StartCoroutine(SpawnRainmakerWithDelay(playerPosition));

    // If there are no sandbags, exit the method
    if (totalSandbags <= 0)
    {
        Debug.Log("No sandbags available to drop.");
        return;
    }

    // Define the effective width of the sandbag based on its scale
    float sandbagWidth = 40f; // Effective width of the sandbag
    float radius = 1.45f; // Set the radius to 2f to place sandbags very close to the player
    float stackingHeightIncrement = 0.6f; // Height increment for stacking sandbags
    int maxSandbagsPerLayer = 12; // Set maximum number of sandbags per layer to 12 for a circular formation

    int layers = Mathf.CeilToInt((float)totalSandbags / maxSandbagsPerLayer); // Calculate the number of layers

    for (int layer = 0; layer < layers; layer++)
    {
        // Calculate the current height for this layer
        float currentHeight = stackingHeightIncrement * layer;

        // Determine how many sandbags to place in this layer
        int sandbagsInLayer = Mathf.Min(maxSandbagsPerLayer, totalSandbags);

        // Loop to create sandbags in a circular pattern for this layer
        for (int i = 0; i < sandbagsInLayer; i++)
        {
            // Calculate the angle for the current sandbag
            float angle = i * (360f / sandbagsInLayer);
            float radian = angle * Mathf.Deg2Rad;

            // Calculate the position for the current sandbag
            Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, currentHeight, Mathf.Sin(radian) * radius);
            Vector3 sandbagPosition = playerPosition + offset;

            // Instantiate the sandbag at the calculated position with the specified rotation
            GameObject sandbag = Instantiate(sandbagPrefab, sandbagPosition, rotation);

            // No scaling applied, sandbag will be at its original size

            // Decrease the count in the inventory
            inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

            // Decrease the total sandbags left
            totalSandbags--;
        }
    }

    // Log a message to the console indicating that sandbags have been placed
    Debug.Log($"Placed sandbags to form a round igloo fortress around the player at position: {playerPosition} with rotation: {rotation.eulerAngles}");
}
*/


/* private bool IsWithinArea(Vector3 position)
 {
     // Check if the position is within the defined area
     return position.x >= areaCenter.x - areaSize.x / 2 &&
            position.x <= areaCenter.x + areaSize.x / 2 &&
            position.z >= areaCenter.z - areaSize.z / 2 &&
            position.z <= areaCenter.z + areaSize.z / 2;
 }
 */

/*
    private void DropSandbag(Vector3 position, string type)
    {
        // Set the desired rotation for the sandbag
        Quaternion rotation = Quaternion.Euler(270, 0, 0); // Set the rotation to 270 degrees on the X-axis

        // Instantiate the sandbag at the desired position with the specified rotation
        GameObject sandbag = Instantiate(sandbagPrefab, position + new Vector3(0, stackingHeight * inventoryManager.GetSandbagCount(type), 0), rotation);

        // Decrease the count in the inventory
        inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

        // Log a message to the console indicating that a sandbag has been placed
        Debug.Log($"Placed a {type} sandbag at position: {position} with rotation: {rotation.eulerAngles}");
    }

    */
/*
    private void DropSandbag(Vector3 position, string type)


    {
        // Instantiate the sandbag at the desired position
        GameObject sandbag = Instantiate(sandbagPrefab, position + new Vector3(0, stackingHeight * inventoryManager.GetSandbagCount(type), 0), Quaternion.identity);

        // Decrease the count in the inventory
        inventoryManager.AddSandbag(type, -1); // Decrease the count by 1

        // Log a message to the console indicating that a sandbag has been placed
        Debug.Log($"Placed a {type} sandbag at position: {position}");
    }
*/

/*
public class SandBagManager : MonoBehaviour
{
    private Dictionary<string, int> sandbagInventory = new Dictionary<string, int>();
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    public Vector3 areaCenter; // Center of the allowed area
    public Vector3 areaSize; // Size of the allowed area

    public Transform dropPoint; // Assign a drop point in the inspector

    // Method to add sandbags to the inventory
    public void AddSandbag(string type, int quantity)
    {
        if (sandbagInventory.ContainsKey(type))
        {
            sandbagInventory[type] += quantity;
        }
        else
        {
            sandbagInventory[type] = quantity;
        }
    }

    // Method to drop a sandbag
    public void TryDropSandbag(string type)
    {
        if (sandbagInventory.ContainsKey(type) && sandbagInventory[type] > 0)
        {
            // Use the drop point instead of raycasting
            Vector3 dropPosition = dropPoint.position;

            if (IsWithinArea(dropPosition))
            {
                DropSandbag(dropPosition, type);
            }
            else
            {
                Debug.LogWarning("Drop position is outside the allowed area.");
            }
        }
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Check if the position is within the defined area
        return position.x >= areaCenter.x - areaSize.x / 2 &&
               position.x <= areaCenter.x + areaSize.x / 2 &&
               position.z >= areaCenter.z - areaSize.z / 2 &&
               position.z <= areaCenter.z + areaSize.z / 2;
    }

    private void DropSandbag(Vector3 position, string type)
    {
        // Instantiate the sandbag at the desired position
        GameObject sandbag = Instantiate(sandbagPrefab, position + new Vector3(0, stackingHeight * GetSandbagCount(type), 0), Quaternion.identity);

        // Decrease the count in the inventory
        sandbagInventory[type]--;

        // Log a message to the console indicating that a sandbag has been placed
        Debug.Log($"Placed a {type} sandbag at position: {position}");

        // Remove the type from the dictionary if the count reaches zero
        if (sandbagInventory[type] <= 0)
        {
            sandbagInventory.Remove(type);
            Debug.Log($"All {type} sandbags have been used.");
        }
    }

    // Method to get the count of a specific type of sandbag
    public int GetSandbagCount(string type)
    {
        return sandbagInventory.ContainsKey(type) ? sandbagInventory[type] : 0;
    }
}
*/


/*
public class SandBagManager : MonoBehaviour
{
    private Dictionary<string, int> sandbagInventory = new Dictionary<string, int>();
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    public Vector3 areaCenter; // Center of the allowed area
    public Vector3 areaSize; // Size of the allowed area

    // Method to add sandbags to the inventory
    public void AddSandbag(string type, int quantity)
    {
        if (sandbagInventory.ContainsKey(type))
        {
            sandbagInventory[type] += quantity;
        }
        else
        {
            sandbagInventory[type] = quantity;
        }
    }

    // Method to drop a sandbag
    public void TryDropSandbag(string type)
    {
        if (sandbagInventory.ContainsKey(type) && sandbagInventory[type] > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
            {
                if (hit.collider.CompareTag("Ground") && IsWithinArea(hit.point))
                {
                    DropSandbag(hit.point, type);
                }
            }
        }
    }

    private bool IsWithinArea(Vector3 position)
    {
        // Check if the position is within the defined area
        return position.x >= areaCenter.x - areaSize.x / 2 &&
               position.x <= areaCenter.x + areaSize.x / 2 &&
               position.z >= areaCenter.z - areaSize.z / 2 &&
               position.z <= areaCenter.z + areaSize.z / 2;
    }

    private void DropSandbag(Vector3 position, string type)
    {
        // Instantiate the sandbag at the desired position
        GameObject sandbag = Instantiate(sandbagPrefab, position + new Vector3(0, stackingHeight * GetSandbagCount(type), 0), Quaternion.identity);

        // Decrease the count in the inventory
        sandbagInventory[type]--;

        // Log a message to the console indicating that a sandbag has been placed
        Debug.Log($"Placed a {type} sandbag at position: {position}");

        // Remove the type from the dictionary if the count reaches zero
        if (sandbagInventory[type] <= 0)
        {
            sandbagInventory.Remove(type);
            Debug.Log($"All {type} sandbags have been used.");
        }
    }

    // Method to get the count of a specific type of sandbag
    public int GetSandbagCount(string type)
    {
        return sandbagInventory.ContainsKey(type) ? sandbagInventory[type] : 0;
    }
}
*/