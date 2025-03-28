using UnityEngine;
using System.Collections;
public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    private Coroutine spawnCoroutine; // To keep track of the coroutine
    private int currentSpawnedCount = 0; // Track the number of currently spawned objects
    private bool isSpawning = false; // Track if spawning is in progress
    private bool hasSpawned = false; // Track if spawning has already occurred

    [SerializeField] private InventoryManager inventoryManager; // Reference to the InventoryManager
    [SerializeField] private GameObject pickUpButton; // Reference to the existing UI Button in the scene
    private PickupItem currentPickupItem; // Reference to the currently selected PickupItem

    void Start()
    {
        // Optionally, you can start spawning automatically at the beginning
        // StartSpawning();
    }

    public void StartSpawning() // Ensure this method is public
    {
        // Check if spawning has already occurred
        if (hasSpawned)
        {
            Debug.LogWarning("You can only spawn once. Spawning has already occurred.");
            return;
        }

        // Check if spawning is already in progress
        if (isSpawning)
        {
            Debug.LogWarning("Spawning is already in progress. Cannot start again.");
            return;
        }

        // If a coroutine is already running, stop it
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        // Set the spawning state to true
        isSpawning = true;

        // Log the start of the spawning process
        Debug.Log("Starting to spawn " + numberOfObjectsToSpawn + " objects.");

        // Start the spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = Quaternion.Euler(270, 0, 0); // Adjust the angles as needed

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            PickupItem pickupItem = spawnedObject.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                pickupItem.InventoryManager = inventoryManager; // Set the inventory manager
            }
            else
            {
                Debug.LogError("PickupItem component not found on the spawned object.");
            }

            currentSpawnedCount++; // Increment the count of spawned objects

            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }

        // Log the completion of the spawning process
        Debug.Log("Finished spawning " + currentSpawnedCount + " objects.");

        // Set hasSpawned to true after spawning is complete
        hasSpawned = true;

        // Reset the spawning state after spawning is complete
        isSpawning = false;

        // Optionally, disable the button after spawning
        // pickUpButton.SetActive(false);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            spawnPosition.y = hit.point.y + spawnHeight; // Adjust to be above the ground
        }
        else
        {
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }

    // Method to set the current pickup item
    public void SetCurrentPickupItem(PickupItem item)
    {
        currentPickupItem = item;
        Debug.Log("Current pickup item set: " + (currentPickupItem != null ? currentPickupItem.ItemName : "null"));
    }

    // Method to pick up the current item
    public void PickUpCurrentItem()
    {
        if (currentPickupItem != null)
        {
            currentPickupItem.OnPickupButtonPressed(); // Call the pickup method on the current item
            currentPickupItem = null; // Clear the reference after picking up
        }
        else
        {
            Debug.LogWarning("No current pickup item to pick up.");
        }
    }

    // Method to get the current count of spawned sandbags
    public int GetCurrentSpawnedCount()
    {
        return currentSpawnedCount;
    }
}
/*
public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    private Coroutine spawnCoroutine; // To keep track of the coroutine
    private int currentSpawnedCount = 0; // Track the number of currently spawned objects

    [SerializeField] private InventoryManager inventoryManager; // Reference to the InventoryManager
    [SerializeField] private GameObject pickUpButton; // Reference to the existing UI Button in the scene
    private PickupItem currentPickupItem; // Reference to the currently selected PickupItem

    void Start()
    {
        // Optionally, you can start spawning automatically at the beginning
        // StartSpawning();
    }

    public void StartSpawning() // Ensure this method is public
    {
        // If a coroutine is already running, stop it
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        // Start the spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Quaternion spawnRotation = Quaternion.Euler(270, 0, 0); // Adjust the angles as needed

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            PickupItem pickupItem = spawnedObject.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                pickupItem.InventoryManager = inventoryManager; // Set the inventory manager
            }
            else
            {
                Debug.LogError("PickupItem component not found on the spawned object.");
            }

            currentSpawnedCount++; // Increment the count of spawned objects

            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            spawnPosition.y = hit.point.y + spawnHeight; // Adjust to be above the ground
        }
        else
        {
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }

    // Method to set the current pickup item
    public void SetCurrentPickupItem(PickupItem item)
    {
        currentPickupItem = item;
        Debug.Log("Current pickup item set: " + (currentPickupItem != null ? currentPickupItem.ItemName : "null"));
    }

    // Method to pick up the current item
    public void PickUpCurrentItem()
    {
        if (currentPickupItem != null)
        {
            currentPickupItem.OnPickupButtonPressed(); // Call the pickup method on the current item
            currentPickupItem = null; // Clear the reference after picking up
        }
        else
        {
            Debug.LogWarning("No current pickup item to pick up.");
        }
    }

    // Method to get the current count of spawned sandbags
    public int GetCurrentSpawnedCount()
    {
        return currentSpawnedCount;
    }
}*/
/*
public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    private Coroutine spawnCoroutine; // To keep track of the coroutine

    [SerializeField] private InventoryManager inventoryManager; // Reference to the InventoryManager
    [SerializeField] private PickupButton pickupButton; // Reference to the PickupButton

    void Start()
    {
        // Optionally, you can start spawning automatically at the beginning
        // StartSpawning();
    }

    public void StartSpawning() // Ensure this method is public
    {
        // If a coroutine is already running, stop it
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        // Start the spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Set the rotation to make the object horizontal
            Quaternion spawnRotation = Quaternion.Euler(270, -180, 0); // Adjust the angles as needed

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            PickupItem pickupItem = spawnedObject.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                pickupItem.InventoryManager = inventoryManager; // Set the inventory manager using the public property
            }

            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the collider
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        // Generate a random position within the bounds
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // Start with the specified spawn height
        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        // Perform a raycast downwards to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            // If we hit something, adjust the Y position to be above the ground
            spawnPosition.y = hit.point.y + spawnHeight; // Add the spawn height to ensure it's above the ground
        }
        else
        {
            // If no ground is detected, you can set a default height or log an error
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }
}
*/
/*
public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    private Coroutine spawnCoroutine; // To keep track of the coroutine
    public InventoryManager inventoryManager; // Reference to the InventoryManager
    public PickupButton pickupButton; // Reference to the PickupButton

    void Start()
    {
        // Optionally, you can start spawning automatically at the beginning
        // StartSpawning();
    }

    public void StartSpawning() // Ensure this method is public
    {
        // If a coroutine is already running, stop it
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        // Start the spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Set the rotation to make the object horizontal
            Quaternion spawnRotation = Quaternion.Euler(270, -180, 0); // Adjust the angles as needed

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            PickupItem pickupItem = spawnedObject.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                pickupItem.inventoryManager = inventoryManager; // Set the inventory manager
            }

            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the collider
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        // Generate a random position within the bounds
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // Start with the specified spawn height
        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        // Perform a raycast downwards to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            // If we hit something, adjust the Y position to be above the ground
            spawnPosition.y = hit.point.y + spawnHeight; // Add the spawn height to ensure it's above the ground
        }
        else
        {
            // If no ground is detected, you can set a default height or log an error
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }
}*/
/* public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    private Coroutine spawnCoroutine; // To keep track of the coroutine

    void Start()
    {
        // Optionally, you can start spawning automatically at the beginning
        // StartCoroutine(SpawnObjects());
    }

    public void StartSpawning() // Ensure this method is public
    {
        // If a coroutine is already running, stop it
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        // Start the spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Set the rotation to make the sandbag horizontal and counterclockwise
            Quaternion spawnRotation = Quaternion.Euler(270, -180, 0); // Adjust the angles as needed

            Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the collider
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        // Generate a random position within the bounds
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // Start with the specified spawn height
        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        // Perform a raycast downwards to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            // If we hit something, adjust the Y position to be above the ground
            spawnPosition.y = hit.point.y + spawnHeight; // Add the spawn height to ensure it's above the ground
        }
        else
        {
            // If no ground is detected, you can set a default height or log an error
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }
}
*/
/*
public class SpawnManager : MonoBehaviour
{
    public GameObject objectToSpawn; // The object you want to spawn
    public BoxCollider spawnArea; // The collider defining the spawn area
    public int numberOfObjectsToSpawn = 10; // Number of objects to spawn
    public float spawnInterval = 5f; // Time interval between spawns
    public float spawnHeight = 0.5f; // Height at which to spawn the objects
    public float groundCheckDistance = 10f; // Distance to check for the ground

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        for (int i = 0; i < numberOfObjectsToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Set the rotation to make the sandbag horizontal and counterclockwise
            Quaternion spawnRotation = Quaternion.Euler(270, -180, 0); // Adjust the angles as needed

            Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get the bounds of the collider
        Vector3 center = spawnArea.bounds.center;
        Vector3 size = spawnArea.bounds.size;

        // Generate a random position within the bounds
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // Start with the specified spawn height
        Vector3 spawnPosition = new Vector3(x, spawnHeight, z);

        // Perform a raycast downwards to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance))
        {
            // If we hit something, adjust the Y position to be above the ground
            spawnPosition.y = hit.point.y + spawnHeight; // Add the spawn height to ensure it's above the ground
        }
        else
        {
            // If no ground is detected, you can set a default height or log an error
            spawnPosition.y = spawnHeight; // Fallback to the default spawn height
        }

        return spawnPosition;
    }
}
*/