using UnityEngine;


public class PickupItem : MonoBehaviour
{
    // Private field
    [SerializeField] private string itemName; // Name of the item to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManager InventoryManager { get; set; } // Public property with a getter and setter

    // Public property for itemName
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    // Getter and Setter for hasBeenPickedUp
    public bool HasBeenPickedUp
    {
        get { return hasBeenPickedUp; }
        private set { hasBeenPickedUp = value; } // Private setter to prevent external modification
    }

    private void Awake()
    {
        // Attempt to find the InventoryManager if not set
        if (InventoryManager == null)
        {
            InventoryManager = InventoryManager.Instance; // Use the singleton instance
            if (InventoryManager == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (HasBeenPickedUp)
        {
            Debug.Log("Item has already been picked up.");
            return; // Exit if already picked up
        }

        Debug.Log("Initiating pickup for: " + ItemName);

        if (InventoryManager != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                InventoryManager.AddSandbag(ItemName, 1); // Add 1 sandbag to inventory
                Debug.Log($"Added {ItemName} to inventory.");
                HasBeenPickedUp = true; // Set the flag to true
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log($"Destroyed item {ItemName}");
            }
            else
            {

                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            InventoryManager = InventoryManager.Instance; // Ensure the InventoryManager instance is set
            if (InventoryManager == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
            else
            {
                Debug.Log("InventoryManager instance set successfully.");
            }

        }

    }

    private bool IsPickupSuccessful()
    {
        float distanceThreshold = 80.0f; // The maximum distance for pickup
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log($"Distance to player: {distance} (Threshold: {distanceThreshold})");
            return distance <= distanceThreshold; // Return true if within pickup distance
        }

        Debug.LogError("Player object missing");
        return false; // If player is not found, return false
    }

    public void ResetPickup()
    {
        hasBeenPickedUp = false; // Reset the pickup state
    }


}/*
public class PickupItem : MonoBehaviour
{
    public string itemName; // Name of the item to be picked up

    [SerializeField] private InventoryManager inventoryManager; // Reference to the InventoryManager

    public void SetInventoryManager(InventoryManager manager)
    {
        inventoryManager = manager;
        Debug.Log("InventoryManager set in PickupItem: " + (inventoryManager != null ? inventoryManager.gameObject.name : "null"));
    }

    private void Awake()
    {
        // Attempt to find the InventoryManager if not set
        if (inventoryManager == null)
        {
            inventoryManager = FindAnyObjectByType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError("No InventoryManagement found in scene!");
            }
            else
            {
                Debug.Log("Found: " + inventoryManager.gameObject.name);
                Debug.Log("Is enabled: " + inventoryManager.gameObject.activeInHierarchy);
                Debug.Log("Is destroyed: " + (inventoryManager.gameObject == null));
            }
        }
        else
        {
            Debug.Log("Already assigned: " + inventoryManager.gameObject.name);
            Debug.Log("Is enabled: " + inventoryManager.gameObject.activeInHierarchy);
            Debug.Log("Is destroyed: " + (inventoryManager.gameObject == null));
        }
    }

    public void OnPickupButtonPressed()
    {
        Debug.Log("Initiating pickup for: " + itemName);

        if (inventoryManager != null)
        {
            Debug.Log("Available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                inventoryManager.AddSandbag(itemName, 1);
                Debug.Log($"Added {itemName} to inventory.");
                Destroy(gameObject);
                Debug.Log($"Destroyed item {itemName}");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("Reference is null");
        }
    }

    private bool IsPickupSuccessful()
    {
        float distanceThreshold = 20.0f; // The maximum distance for pickup

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log($"Distance to player: {distance} (Threshold: {distanceThreshold})");
            return distance <= distanceThreshold; // Return true if within pickup distance
        }

        Debug.LogError("Player object missing");
        return false;
    }
}*/
/*
public class PickupItem : MonoBehaviour
{
    public string itemName; // Name of the item to be picked up

    private InventoryManager inventoryManager; // Reference to the InventoryManager
    private SpawnManager spawnManager; // Reference to the SpawnManager

    private void Start()
    {
        // Attempt to find the InventoryManager and SpawnManager if not set
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("No InventoryManager found in the scene!");
        }
        else
        {
            Debug.Log("InventoryManager found: " + inventoryManager.gameObject.name);
        }

        spawnManager = Object.FindFirstObjectByType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("No SpawnManager found in the scene!");
        }
        else
        {
            Debug.Log("SpawnManager found: " + spawnManager.gameObject.name);
        }
    }

    // Public method to be called on button click
    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup initiated for: " + itemName); // Debug log for pickup initiation

        if (inventoryManager != null)
        {
            if (IsPickupSuccessful())
            {
                inventoryManager.AddSandbag(itemName, 1); // Add 1 sandbag to inventory
                Debug.Log($"1 {itemName} has been picked up!"); // Debug log for item pickup
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log("Item destroyed: " + itemName); // Debug log for item destruction
            }
            else
            {
                Debug.Log("Pickup failed. Conditions not met."); // Log if pickup fails
            }
        }
        else
        {
            Debug.LogError("InventoryManager reference is null!"); // Error log if inventory manager is not set
        }
    }

    private bool IsPickupSuccessful()
    {
        // You can remove this distance check if you want the button to always work
        float pickupDistance = 10.0f; // Increase this value for testing
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log($"Distance to player: {distance}"); // Log the distance for debugging
            return distance <= pickupDistance; // Return true if within pickup distance
        }

        Debug.Log("Player not found."); // Log if player is not found
        return false; // If player is not found, return false
    }

    // Optional: Method to set the InventoryManager from outside
    public void SetInventoryManager(InventoryManager manager)
    {
        inventoryManager = manager;
    }
}

*/