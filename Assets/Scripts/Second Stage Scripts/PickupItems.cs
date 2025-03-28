

using System.Collections.Generic;
using UnityEngine;


public class PickupItems : MonoBehaviour
{
    [SerializeField] private List<GameObject> items; // List of items to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for items
    public List<GameObject> Items => items;

    // Getter and Setter for hasBeenPickedUp
    /*  public bool HasBeenPickedUp
     {
         get => hasBeenPickedUp;
         private set => hasBeenPickedUp = value; // Private setter to prevent external modification
     } */
    public bool HasBeenPickedUp { get; private set; } = false;

    // Method to get a copy of the items list
    public List<GameObject> GetPickupItems()
    {
        // Return a copy of the items list to avoid external modification
        return new List<GameObject>(items);
    }

    private void Awake()
    {
        // Attempt to find the InventoryManager if not set
        if (InventoryManagers == null)
        {
            InventoryManagers = InventoryManagers.Instance; // Use the singleton instance
            if (InventoryManagers == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        /*    Debug.Log($"OnPickupButtonPressed called for {gameObject.name} at {Time.time}");
           if (HasBeenPickedUp)
           {
               Debug.Log("Items have already been picked up.");
               return; // Exit if already picked up
           }

           Debug.Log("Initiating pickup for items: " + string.Join(", ", items.ConvertAll(item => item.name)));
    */
        Debug.Log($"OnPickupButtonPressed called for {gameObject.name} at {Time.time}");
        if (HasBeenPickedUp)
        {
            Debug.LogWarning($"{gameObject.name} has already been picked up.");
            return; // Exit if the item has already been picked up
        }

        HasBeenPickedUp = true; // Mark the item as picked up
        Debug.Log($"{gameObject.name} has been picked up.");
        // Additional logic for picking up the item
        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var item in items)
                {
                    string itemName = item.name;
                    InventoryManagers.AddItem(itemName, 1); // Add the item to the inventory
                    Debug.Log($"Added {itemName} to inventory.");
                    Debug.Log($"Item in list: {item.name}");
                }
                HasBeenPickedUp = true; // Mark as picked up
                Debug.Log($"Setting hasBeenPickedUp to true for {gameObject.name}");
                Destroy(gameObject); // Destroy the GameObject after pickup
                Debug.Log($"Destroyed pickup items for {gameObject.name}");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
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
}
//Using GameObjects not Strings
/* public class PickupItems : MonoBehaviour
{
    // Private field to hold multiple item GameObjects
    [SerializeField] private List<GameObject> items; // List of items to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for items
    public List<GameObject> Items
    {
        get { return items; }
    }

    // Getter and Setter for hasBeenPickedUp
    public bool HasBeenPickedUp
    {
        get { return hasBeenPickedUp; }
        private set { hasBeenPickedUp = value; } // Private setter to prevent external modification
    }

    // Method to get the picked up items
    public List<GameObject> GetPickupItems()
    {
        // Return a copy of the items list to avoid external modification
        return new List<GameObject>(items);
    }

    private void Awake()
    {
        // Attempt to find the InventoryManager if not set
        if (InventoryManagers == null)
        {
            InventoryManagers = InventoryManagers.Instance; // Use the singleton instance
            if (InventoryManagers == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (HasBeenPickedUp)
        {
            Debug.Log("Items have already been picked up.");
            return; // Exit if already picked up
        }

        Debug.Log("Initiating pickup for items: " + string.Join(", ", items.ConvertAll(item => item.name)));

        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var item in items)
                {
                    string itemName = item.name; // Get the name of the GameObject
                    InventoryManagers.AddItem(itemName, 1); // Add 1 item to inventory for each item
                    Debug.Log($"Added {itemName} to inventory.");
                }
                HasBeenPickedUp = true; // Set the flag to true
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log($"Destroyed pickup items.");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
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
} */
/* public class PickupItems : MonoBehaviour
{
    // Private field to hold multiple item names
    [SerializeField] private List<string> itemNames; // List of item names to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for itemNames
    public List<string> ItemNames
    {
        get { return itemNames; }
    }

    // Getter and Setter for hasBeenPickedUp
    public bool HasBeenPickedUp
    {
        get { return hasBeenPickedUp; }
        private set { hasBeenPickedUp = value; } // Private setter to prevent external modification
    }
    public List<PickupItem> GetPickupItems()
    {
        // Replace this with the actual logic to retrieve the items
        return new List<PickupItem>();
    }
    private void Awake()
    {
        // Attempt to find the InventoryManager if not set
        if (InventoryManagers == null)
        {
            InventoryManagers = InventoryManagers.Instance; // Use the singleton instance
            if (InventoryManagers == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (HasBeenPickedUp)
        {
            Debug.Log("Items have already been picked up.");
            return; // Exit if already picked up
        }

        Debug.Log("Initiating pickup for items: " + string.Join(", ", itemNames));

        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var itemName in itemNames)
                {
                    InventoryManagers.AddItem(itemName, 1); // Add 1 item to inventory for each item name
                    Debug.Log($"Added {itemName} to inventory.");
                }
                HasBeenPickedUp = true; // Set the flag to true
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log($"Destroyed pickup items.");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
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
} */
/* public class PickupItems : MonoBehaviour
{
    // Private field to hold multiple PickupItem objects
    [SerializeField] private List<PickupItem> items; // List of PickupItem objects
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for items
    public List<PickupItem> Items
    {
        get { return items; }
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
        if (InventoryManagers == null)
        {
            InventoryManagers = InventoryManagers.Instance; // Use the singleton instance
            if (InventoryManagers == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (HasBeenPickedUp)
        {
            Debug.Log("Items have already been picked up.");
            return; // Exit if already picked up
        }

        Debug.Log("Initiating pickup for items.");

        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var item in items)
                {
                    InventoryManagers.AddItem(item.ItemName, 1); // Assuming PickupItem has an ItemName property
                    Debug.Log($"Added {item.ItemName} to inventory.");
                }
                HasBeenPickedUp = true; // Set the flag to true
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log($"Destroyed pickup items.");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
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
} */
/* 
public class PickupItems : MonoBehaviour
{
    // Private field to hold multiple item names
    [SerializeField] private List<string> itemNames; // List of item names to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for itemNames
    public List<string> ItemNames
    {
        get { return itemNames; }
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
        if (InventoryManagers == null)
        {
            InventoryManagers = InventoryManagers.Instance; // Use the singleton instance
            if (InventoryManagers == null)
            {
                Debug.LogError("No InventoryManager found in the scene!");
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (HasBeenPickedUp)
        {
            Debug.Log("Items have already been picked up.");
            return; // Exit if already picked up
        }

        Debug.Log("Initiating pickup for items: " + string.Join(", ", itemNames));

        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var itemName in itemNames)
                {
                    InventoryManagers.AddItem(itemName, 1); // Add 1 item to inventory for each item name
                    Debug.Log($"Added {itemName} to inventory.");
                }
                HasBeenPickedUp = true; // Set the flag to true
                Destroy(gameObject); // Destroy the item after picking it up
                Debug.Log($"Destroyed pickup items.");
            }
            else
            {
                Debug.Log("Pickup failed. Player too far away.");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
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
} */