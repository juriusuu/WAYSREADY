using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance; // Singleton instance
    private Dictionary<string, int> sandbagInventory = new Dictionary<string, int>();
    public GameObject sandbagPrefab; // Assign your sandbag prefab in the inspector
    public float stackingHeight = 0.5f; // Height to stack sandbags

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
            Debug.Log("InventoryManager instance created.");
        }
        else
        {
            Debug.LogWarning("Another instance of InventoryManager was destroyed.");
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("InventoryManager instance is null!");
            }
            return _instance;
        }
    }

    // Method to add sandbags to the inventory
    public void AddSandbag(string type, int quantity)
    {
        Debug.Log($"Attempting to add {quantity} {type} sandbags to inventory.");
        if (sandbagInventory.ContainsKey(type))
        {
            sandbagInventory[type] += quantity;
            Debug.Log($"Updated {type} sandbags in inventory. New total: {sandbagInventory[type]}");
        }
        else
        {
            sandbagInventory[type] = quantity;
            Debug.Log($"Added new type {type} with quantity: {quantity}");
        }
    }


    // Method to get the count of a specific type of sandbag
    public int GetSandbagCount(string type)
    {
        int count = sandbagInventory.ContainsKey(type) ? sandbagInventory[type] : 0;
        Debug.Log($"Current count for {type} sandbags: {count}");
        return count;
    }


    public void ResetInventory()
    {
        sandbagInventory.Clear(); // Clear the inventory
        Debug.Log("Inventory has been reset.");
    }
}


/*     private void DropSandbag(Vector3 position, string type)
    {
        // Instantiate the sandbag at the desired position
        GameObject sandbag = Instantiate(sandbagPrefab, position + new Vector3(0, stackingHeight * GetSandbagCount(type), 0), Quaternion.identity);
        sandbagInventory[type]--; // Decrease the count in the inventory

        // Log a message to the console indicating that a sandbag has been placed
        Debug.Log($"Placed a {type} sandbag at position: {position}");

        // Remove the type from the dictionary if the count reaches zero
        if (sandbagInventory[type] <= 0)
        {
            sandbagInventory.Remove(type);
            Debug.Log($"All {type} sandbags have been used.");
        }
    }
 */
/*
    public void TryDropSandbag(string type)
    {
        Debug.Log($"Attempting to drop {type} sandbag.");

        // Check if the inventory contains the specified type of sandbag and if there are any available
        if (sandbagInventory.ContainsKey(type) && sandbagInventory[type] > 0)
        {
            // Use the drop point's position instead of raycasting
            Vector3 dropPosition = dropPoint.position; // Assuming dropPoint is a Transform assigned in the inspector

            // Optionally, check if the drop position is valid (within the allowed area)
            if (IsWithinArea(dropPosition))
            {
                DropSandbag(dropPosition, type);
            }
            else
            {
                Debug.Log("Drop position is outside the allowed area.");
            }
        }
        else
        {
            Debug.Log($"No {type} sandbags available to drop.");
        }
    }
/*
    /*
        // Method to drop a sandbag
        public void TryDropSandbag(string type)
        {
            Debug.Log($"Attempting to drop {type} sandbag.");
            if (sandbagInventory.ContainsKey(type) && sandbagInventory[type] > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
                {
                    Debug.Log($"Raycast hit: {hit.collider.name}");
                    if (hit.collider.CompareTag("Ground")) // Ensure you have a ground object tagged as "Ground"
                    {
                        DropSandbag(hit.point, type);
                    }
                    else
                    {
                        Debug.Log("Hit object is not tagged as Ground.");
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit anything.");
                }
            }
            else
            {
                Debug.Log($"No {type} sandbags available to drop.");
            }
        }
        */


/*
public class InventoryManager : MonoBehaviour
{
    // Dictionary to store the count of each item type
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public void AddItem(string itemName)
    {
        // Check if the item is already in the inventory
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++; // Increment the count for the item
            Debug.Log("Item already in inventory: " + itemName); // Log if item is already in inventory
        }
        else
        {
            inventory[itemName] = 1; // Add the item with a count of 1
            Debug.Log("Picked up: " + itemName); // Log the item picked up
        }

        // Log the current count of the item in the inventory
        Debug.Log("Current count of " + itemName + " in inventory: " + inventory[itemName]);
    }

    // Optional: Method to remove an item from the inventory
    public void RemoveItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]--;
            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName); // Remove the item if count is zero
            }
            Debug.Log("Removed: " + itemName); // Log the item removed
        }
        else
        {
            Debug.Log("Item not found in inventory: " + itemName); // Log if item is not found
        }
    }

    // Optional: Method to display the current inventory
    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in inventory)
        {
            Debug.Log(item.Key + ": " + item.Value); // Log each item and its count in the inventory
        }
    }
}
// Not incrementing
*/
/*
public class InventoryManager : MonoBehaviour
{
    public List<string> inventory = new List<string>(); // List to store picked up items

    public void AddItem(string itemName)
    {
        // Check if the item is already in the inventory
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            Debug.Log("Picked up: " + itemName); // Log the item picked up
        }
        else
        {
            Debug.Log("Item already in inventory: " + itemName); // Log if item is already in inventory
        }

        // Log the current count of the item in the inventory
        int itemCount = CountItem(itemName);
        Debug.Log("Current count of " + itemName + " in inventory: " + itemCount);
    }

    // Method to count how many of a specific item are in the inventory
    private int CountItem(string itemName)
    {
        int count = 0;
        foreach (string item in inventory)
        {
            if (item == itemName)
            {
                count++;
            }
        }
        return count;
    }

    // Optional: Method to remove an item from the inventory
    public void RemoveItem(string itemName)
    {
        if (inventory.Contains(itemName))
        {
            inventory.Remove(itemName);
            Debug.Log("Removed: " + itemName); // Log the item removed
        }
        else
        {
            Debug.Log("Item not found in inventory: " + itemName); // Log if item is not found
        }
    }

    // Optional: Method to display the current inventory
    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (string item in inventory)
        {
            Debug.Log(item); // Log each item in the inventory
        }
    }
}

*/