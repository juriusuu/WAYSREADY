using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Required for List<T>

public class InventoryUIManager : MonoBehaviour
{
    public GameObject canvasInventory; // Reference to the entire CanvasInventory
    public Transform inventoryPanel; // Reference to the Inventory Panel (parent for slots)
    public GameObject inventorySlotPrefab; // Reference to the Inventory Slot Prefab
    public static InventoryUIManager Instance { get; private set; } // Singleton instance

    private List<GameObject> slotPool = new List<GameObject>(); // Pool of inventory slots for reuse
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes
            Debug.Log("InventoryUIManager initialized successfully.");
        }
        else
        {
            Debug.LogWarning("Duplicate InventoryUIManager detected. Destroying this instance.");
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Check for missing references
        if (canvasInventory == null)
        {
            Debug.LogError("CanvasInventory is not assigned!");
        }
        else
        {
            DontDestroyOnLoad(canvasInventory); // Ensure CanvasInventory persists across scenes
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel is not assigned!");
        }

        if (inventorySlotPrefab == null)
        {
            Debug.LogError("InventorySlotPrefab is not assigned!");
        }
    }

    private void Start()
    {
        // Ensure the CanvasInventory is hidden at the start
        if (canvasInventory != null)
        {
            canvasInventory.SetActive(false);
        }

        // Delay updating the inventory UI
        Invoke(nameof(UpdateInventoryUI), 0.1f);
    }
    /* 
        public void UpdateInventoryUI()
        {
            if (canvasInventory == null || inventoryPanel == null) return;

            // Fetch the inventory from InventoryManagers
            var inventory = InventoryManagers.Instance.GetInventory();
            Debug.Log($"Updating Inventory UI with {inventory.Count} items.");

            // Ensure the pool has enough slots
            while (slotPool.Count < inventory.Count)
            {
                GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel);
                newSlot.SetActive(false); // Initially deactivate the slot
                slotPool.Add(newSlot);
            }

            // Update the slots with inventory data
            int index = 0;
            foreach (var item in inventory)
            {
                if (item.Key == null || item.Value.sprite == null)
                {
                    Debug.LogWarning($"Skipping invalid item: {item.Key}");
                    continue;
                }

                GameObject slot = slotPool[index];
                slot.SetActive(true); // Activate the slot
                InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
                if (slotComponent != null)
                {
                    slotComponent.Setup(item.Key, item.Value.quantity, item.Value.sprite);
                }
                else
                {
                    Debug.LogError("InventorySlot script is missing on the prefab!");
                }
                index++;
            }

            // Deactivate unused slots
            for (int i = index; i < slotPool.Count; i++)
            {
                slotPool[i].SetActive(false);
            }

            Debug.Log("Inventory UI updated.");
        } */
    public void UpdateInventoryUI()
    {
        if (canvasInventory == null || inventoryPanel == null)
        {
            Debug.LogError("CanvasInventory or InventoryPanel is not assigned!");
            return;
        }

        // Fetch the inventory from InventoryManagers
        var inventory = InventoryManagers.Instance.GetInventory();
        Debug.Log($"Updating Inventory UI with {inventory.Count} items.");

        // Ensure the pool has enough slots
        while (slotPool.Count < inventory.Count)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel);
            newSlot.SetActive(false); // Initially deactivate the slot
            slotPool.Add(newSlot);
            Debug.Log($"Created new inventory slot: {newSlot.name}");
        }

        // Update the slots with inventory data
        int index = 0;
        foreach (var item in inventory)
        {
            if (item.Key == null || item.Value.sprite == null)
            {
                Debug.LogWarning($"Skipping invalid item: {item.Key}");
                continue;
            }

            GameObject slot = slotPool[index];
            slot.SetActive(true); // Activate the slot
            InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
            if (slotComponent != null)
            {
                slotComponent.Setup(item.Key, item.Value.quantity, item.Value.sprite);
                Debug.Log($"Updated slot {index} with item: {item.Key}, quantity: {item.Value.quantity}");
            }
            else
            {
                Debug.LogError("InventorySlot script is missing on the prefab!");
            }
            index++;
        }

        // Deactivate unused slots
        for (int i = index; i < slotPool.Count; i++)
        {
            slotPool[i].SetActive(false);
            Debug.Log($"Deactivated unused slot {i}");
        }

        Debug.Log("Inventory UI updated.");
    }
    private Sprite GetItemSprite(string itemName)
    {
        // Fetch the sprite from the GameObject in the scene
        GameObject itemObject = GameObject.Find(itemName);
        if (itemObject != null)
        {
            Image itemImage = itemObject.GetComponent<Image>();
            if (itemImage != null)
            {
                Debug.Log($"Sprite found for {itemName}: {itemImage.sprite.name}");
                return itemImage.sprite;
            }
            else
            {
                Debug.LogWarning($"Image component not found on GameObject: {itemName}");
            }
        }
        else
        {
            Debug.LogWarning($"GameObject not found for item: {itemName}");
        }

        return null; // Return null if the sprite could not be found
    }

    public void OpenInventory()
    {
        if (canvasInventory != null)
        {
            canvasInventory.SetActive(true); // Show the entire CanvasInventory
            UpdateInventoryUI(); // Refresh the UI when opening
            Debug.Log("Inventory canvas opened.");
        }
    }

    public void CloseInventory()
    {
        if (canvasInventory != null)
        {
            canvasInventory.SetActive(false); // Hide the entire CanvasInventory
            Debug.Log("Inventory canvas closed.");
        }
    }

    /*     public void ToggleInventory()
        {
            if (canvasInventory == null)
            {
                Debug.LogError("CanvasInventory is not assigned! Cannot toggle inventory.");
                return;
            }

            bool isActive = canvasInventory.activeSelf;
            canvasInventory.SetActive(!isActive);

            if (isActive)
            {
                Debug.Log("Inventory canvas closed.");
            }
            else
            {
                UpdateInventoryUI(); // Refresh the UI when opening
                Debug.Log("Inventory canvas opened.");
            }
        } */
    public void ToggleInventory()
    {
        if (canvasInventory == null)
        {
            Debug.LogError("CanvasInventory is not assigned! Cannot toggle inventory.");
            return;
        }

        bool isActive = canvasInventory.activeSelf;
        canvasInventory.SetActive(!isActive);

        if (isActive)
        {
            Debug.Log("Inventory canvas closed.");
        }
        else
        {
            UpdateInventoryUI(); // Refresh the UI when opening

            // Ensure all slots are active
            foreach (GameObject slot in slotPool)
            {
                slot.SetActive(true);
            }

            Debug.Log("Inventory canvas opened.");
        }
    }
}