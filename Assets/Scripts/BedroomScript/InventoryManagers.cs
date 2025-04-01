using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagers : MonoBehaviour
{
    private static InventoryManagers _instance; // Singleton instance
    private Dictionary<string, int> itemInventory = new Dictionary<string, int>(); // Dictionary to hold item types and their quantities
    private int totalItemsToCollect = 11; // Total number of unique items to collect

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

    public static InventoryManagers Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("InventoryManager instance is null!");
            }
            return _instance;
        }
    }

    public void AddItem(string itemName, int quantity)
    {
        Debug.Log($"AddItem called for: {itemName}, Quantity: {quantity}");

        if (itemInventory.ContainsKey(itemName))
        {
            itemInventory[itemName] += quantity;
            Debug.Log($"Updated {itemName} in inventory. New total: {itemInventory[itemName]}");
        }
        else
        {
            itemInventory[itemName] = quantity;
            Debug.Log($"Added new item {itemName} with quantity: {quantity}");
        }

        Debug.Log($"Total items in inventory after addition: {GetTotalItemCount()}");
    }

    public int GetTotalItemCount()
    {
        int totalCount = 0;
        foreach (var item in itemInventory)
        {
            totalCount += item.Value; // Sum up all item quantities
        }
        return totalCount;
    }

    public int GetItemCount(string itemName)
    {
        int count = itemInventory.ContainsKey(itemName) ? itemInventory[itemName] : 0; // Return count or 0 if not found
        Debug.Log($"Current count for {itemName}: {count}");
        return count;
    }

    public void ResetInventory()
    {
        itemInventory.Clear(); // Clear the inventory
        Debug.Log("Inventory has been reset.");
    }

    /*    public void DisplayInventory()
       {
           Debug.Log("Current Inventory:");
           foreach (var item in itemInventory)
           {
               Debug.Log($"{item.Key}: {item.Value}");
           }
       } */
    public void DisplayInventory()
    {
        Debug.Log("Current Inventory:");
        foreach (var item in itemInventory)
        {
            Debug.Log($"{item.Key}: {item.Value}");

            // Find the GameObject for the item and enable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = true; // Enable the SpriteRenderer
                }
            }
        }
    }
    public void HideInventory()
    {
        Debug.Log("Hiding Inventory...");
        foreach (var item in itemInventory)
        {
            // Find the GameObject for the item and disable its SpriteRenderer
            GameObject itemObject = GameObject.Find(item.Key);
            if (itemObject != null)
            {
                SpriteRenderer spriteRenderer = itemObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false; // Disable the SpriteRenderer
                }
            }
        }
    }
}