using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)
    public GameObject inventoryPanel; // Reference to the Inventory Panel
    public GameObject inventorySlotPrefab; // Reference to the Inventory Slot Prefab
    private int currentItemIndex = 0; // Track the current item index

    private void Start()
    {
        // If button is not assigned, try to get it from the GameObject
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("Button not found!");
                return;
            }
        }

        // Clear existing listeners to avoid duplicate calls
        button.onClick.RemoveAllListeners();

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");

        // Initialize the inventory panel
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true); // Show the panel initially
        }
    }

    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");

        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        Debug.Log($"CurrentItemIndex: {currentItemIndex}, PickupItems.Count: {pickupItems.Count}");

        if (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null && !item.HasBeenPickedUp)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");
                item.OnPickupButtonPressed(); // Call the pickup method on the current item

                foreach (var pickupItem in item.Items)
                {
                    string itemName = pickupItem.name;

                    // Get the Image component
                    Image itemImage = pickupItem.GetComponent<Image>();

                    if (itemImage == null || itemImage.sprite == null)
                    {
                        Debug.LogWarning($"Item {itemName} does not have an Image component or sprite assigned.");
                        continue; // Skip this item if no Image or sprite is found
                    }

                    // Retrieve the sprite from the Image component
                    Sprite itemSprite = itemImage.sprite;

                    Debug.Log($"Adding item to inventory: {itemName}");
                    AddItemToInventory(itemName, itemSprite); // Add the item to the inventory UI
                    Debug.Log($"Added {itemName} to inventory.");
                }

                currentItemIndex++;
                Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");
            }
            else if (item != null && item.HasBeenPickedUp)
            {
                Debug.LogWarning($"Item {item.gameObject.name} has already been picked up.");
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }
        }

        if (currentItemIndex >= pickupItems.Count)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    }

    private void AddItemToInventory(string itemName, Sprite itemSprite)
    {
        if (inventoryPanel == null || inventorySlotPrefab == null)
        {
            Debug.LogWarning("Inventory panel or slot prefab is not assigned.");
            return;
        }

        // Create a new inventory slot
        GameObject newSlot = Instantiate(inventorySlotPrefab, inventoryPanel.transform);

        // Set the item's sprite using the Image component
        Image slotImage = newSlot.GetComponent<Image>();
        if (slotImage != null && itemSprite != null)
        {
            slotImage.sprite = itemSprite; // Assign the sprite to the Image component
            Debug.Log($"Assigned sprite for item: {itemName}");
        }
        else
        {
            Debug.LogWarning($"Failed to assign sprite for item: {itemName}. Ensure the prefab has an Image component and the sprite is not null.");
        }

        // Set the item's name in the text component
        Text itemNameText = newSlot.GetComponentInChildren<Text>();
        if (itemNameText != null)
        {
            itemNameText.text = itemName; // Set the item's name
        }
        else
        {
            Debug.LogWarning($"Failed to find Text component in inventory slot prefab for item: {itemName}");
        }
    }

    public void SetPickupItems(List<PickupItems> newPickupItems)
    {
        if (newPickupItems == null || newPickupItems.Count == 0)
        {
            Debug.LogWarning("PickupItems list is null or empty! No items assigned.");
            pickupItems = new List<PickupItems>(); // Assign an empty list to avoid null issues
            currentItemIndex = 0;
            return;
        }

        pickupItems = newPickupItems;
        currentItemIndex = 0; // Reset the index when setting new items
        Debug.Log($"PickupItems list set with {pickupItems.Count} items.");

        foreach (var item in pickupItems)
        {
            if (item != null && item.Items != null && item.Items.Count > 0)
            {
                Debug.Log($"PickupItem assigned: {item.gameObject.name} with items: {string.Join(", ", item.Items.ConvertAll(i => i.name))}");
            }
            else if (item != null)
            {
                Debug.LogWarning($"PickupItem assigned: {item.gameObject.name} but it has no items.");
            }
            else
            {
                Debug.LogWarning("One of the PickupItems is null.");
            }
        }
    }
}