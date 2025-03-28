using System.Collections.Generic; // Required for List
using UnityEngine;
using UnityEngine.UI; // Required for Button.

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
    }/* 
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
            if (item != null)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");
                item.OnPickupButtonPressed(); // Call the pickup method on the current item

                // Add the item to the inventory
                foreach (var pickupItem in item.Items)
                {
                    string itemName = pickupItem.name;
                    Sprite itemSprite = pickupItem.GetComponent<SpriteRenderer>()?.sprite; // Get the sprite from the GameObject
                    Debug.Log($"Adding item to inventory: {itemName}");
                    AddItemToInventory(itemName, itemSprite); // Add the item to the inventory UI
                    InventoryManagers.Instance.AddItem(itemName, 1); // Add the item to the inventory system
                    Debug.Log($"Added {itemName} to inventory.");
                }

                // Increment the index to process the next item
                currentItemIndex++;
                Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }
        }

        // Check if all items have been processed
        if (currentItemIndex >= pickupItems.Count)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    }

 */
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
                    Sprite itemSprite = pickupItem.GetComponent<SpriteRenderer>()?.sprite;
                    Debug.Log($"Adding item to inventory: {itemName}");
                    AddItemToInventory(itemName, itemSprite); // Add the item to the inventory UI
                                                              //    InventoryManagers.Instance.AddItem(itemName, 1); // Add the item to the inventory system
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

        // Set the item's image
        Image slotImage = newSlot.GetComponent<Image>();
        if (slotImage != null && itemSprite != null)
        {
            slotImage.sprite = itemSprite;
        }

        // Optionally, set the item's name or quantity as text
        Text slotText = newSlot.GetComponentInChildren<Text>();
        if (slotText != null)
        {
            slotText.text = itemName; // You can also display the quantity here
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
/* 
public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)
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

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");

        // Log the size of the pickupItems list and the names of all GameObjects
        if (pickupItems != null && pickupItems.Count > 0)
        {
            Debug.Log($"PickupItems list size: {pickupItems.Count}");
            foreach (var item in pickupItems)
            {
                if (item != null && item.Items.Count > 0)
                {
                    Debug.Log($"PickupItem: {item.Items[0].name}"); // Log the name of the first GameObject in the item's list
                }
                else
                {
                    Debug.LogWarning("One of the PickupItems is null or has no items.");
                }
            }
        }
        else
        {
            Debug.LogWarning("PickupItems list is empty or not assigned.");
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
            if (item != null)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");
                item.OnPickupButtonPressed(); // Call the pickup method on the current item

                // Remove the picked-up item from the list
                pickupItems.RemoveAt(currentItemIndex);
                Debug.Log($"Removed item at index {currentItemIndex}. Remaining items: {pickupItems.Count}");
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }
        }

        // Check if all items have been processed
        if (pickupItems.Count == 0)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    } */
/*     public void OnPickupButtonPressed()
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
            if (item != null)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");
                item.OnPickupButtonPressed(); // Call the pickup method on the current item
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }

            currentItemIndex++; // Move to the next PickupItems instance
            Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");
        }

        if (currentItemIndex >= pickupItems.Count)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    }
 */
/*     public void SetPickupItems(List<PickupItems> newPickupItems)
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
} */
/* 

    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");

        // Check if there are any PickupItems assigned
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        // Iterate through the pickupItems list
        while (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");

                // Iterate through all items in the current PickupItems instance
                foreach (var pickupItem in item.Items)
                {
                    string itemName = pickupItem.name;
                    InventoryManagers.Instance.AddItem(itemName, 1); // Add the item to the inventory
                    Debug.Log($"Picked up item: {itemName}");
                }

                // Mark the current PickupItems instance as picked up
                item.OnPickupButtonPressed();
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }

            currentItemIndex++; // Move to the next PickupItems instance
            Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");
        }

        // Check if all PickupItems instances have been processed
        if (currentItemIndex >= pickupItems.Count)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    } */


/* Without Co Pilot
public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)
    private int currentItemIndex = 0; // Track the current item index


public void SetPickupItems(List<PickupItems> pickupItems)
{
    if (pickupItems == null || pickupItems.Count == 0)
    {
        Debug.LogError("PickupItems list is null or empty!");
        return;
    }

    this.pickupItems = pickupItems;
    currentItemIndex = 0; // Reset the index when setting new items
    foreach (var item in pickupItems)
    {
        Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? string.Join(", ", item.Items.ConvertAll(i => i.name)) : "null"));
    }
}
}

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

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");
    }
    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");

        // Check if there are any PickupItems assigned
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        // Iterate through the pickupItems list
        while (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            {
                Debug.Log($"Processing PickupItems instance at index {currentItemIndex}");

                // Iterate through all items in the current PickupItems instance
                foreach (var pickupItem in item.Items)
                {
                    string itemName = pickupItem.name;
                    InventoryManagers.Instance.AddItem(itemName, 1); // Add the item to the inventory
                    Debug.Log($"Picked up item: {itemName}");
                }

                // Mark the current PickupItems instance as picked up
                item.OnPickupButtonPressed();
            }
            else
            {
                Debug.LogWarning($"PickupItems instance at index {currentItemIndex} is null.");
            }

            currentItemIndex++; // Move to the next PickupItems instance
            Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");
        }

        // Check if all PickupItems instances have been processed
        if (currentItemIndex >= pickupItems.Count)
        {
            Debug.Log("All PickupItems have been processed.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    } */
/*     public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");
        Debug.Log($"Current Item Index: {currentItemIndex + 1}, Total Items: {pickupItems.Count}");

        // Check if there are any PickupItems assigned
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }
 */
// Log current index and total items
/*    Debug.Log($"Current Item Index: {currentItemIndex + 1}, Total Items: {pickupItems.Count}");
*/
// Check if there are items left to pick up
/*         if (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            { */
/*               // Iterate through all items in the PickupItems object
              foreach (var pickupItem in item.Items)
              {
                  string itemName = pickupItem.name;
                  // Add the item with the current index + 1 as the quantity
                  InventoryManagers.Instance.AddItem(itemName, currentItemIndex + 1);
                  Debug.Log($"Picked up item: {itemName} with quantity: {currentItemIndex + 1}");
              } */
/* 
                Debug.Log($"Processing item at index {currentItemIndex}");

                // Call the pickup method on the current item
                item.OnPickupButtonPressed();
                currentItemIndex++; // Move to the next item
                Debug.Log($"CurrentItemIndex incremented to: {currentItemIndex}");


                // Check if all items have been picked up
                if (currentItemIndex >= pickupItems.Count)
                {
                    Debug.Log("All items have been picked up.");
                    gameObject.SetActive(false); // Deactivate the button when done
                }
            }
            else
            {
                Debug.LogWarning("One of the PickupItems is null.");
            }
        }
    } */

/*     public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");

        // Check if there are any PickupItems assigned
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        // Log current index and total items
        Debug.Log($"Current Item Index: {currentItemIndex}, Total Items: {pickupItems.Count}");

        // Check if there are items left to pick up
        if (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            {
                // Assuming item has a property or method to get its name
                string itemName = item.Items[0].name; // Adjust this based on your actual structure
                InventoryManagers.Instance.AddItem(itemName, 1); // Add the item to the inventory
                item.OnPickupButtonPressed(); // Pick up the current item
                Debug.Log($"Picked up item: {itemName}"); // Log the picked item
                currentItemIndex++; // Move to the next item

                // Check if all items have been picked up
                if (currentItemIndex >= pickupItems.Count)
                {
                    Debug.Log("All items have been picked up.");
                    gameObject.SetActive(false); // Deactivate the button when done
                }
            }
            else
            {
                Debug.LogWarning("One of the PickupItems is null.");
            }
        }
    } */
/*     public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");

        // Check if there are any PickupItems assigned
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        // Log current index and total items
        Debug.Log($"Current Item Index: {currentItemIndex}, Total Items: {pickupItems.Count}");

        // Check if there are items left to pick up
        if (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            {
                item.OnPickupButtonPressed(); // Pick up the current item
                Debug.Log($"Picked up item: {item.name}"); // Log the picked item
                currentItemIndex++; // Move to the next item

                // Check if all items have been picked up
                if (currentItemIndex >= pickupItems.Count)
                {
                    Debug.Log("All items have been picked up.");
                    gameObject.SetActive(false); // Deactivate the button when done
                }
            }
            else
            {
                Debug.LogWarning("One of the PickupItems is null.");
            }
        }
        else
        {
            Debug.Log("All items have been picked up.");
            gameObject.SetActive(false); // Deactivate the button when done
        }
    } */

// Method to set the PickupItems reference

/* public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)
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

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");
    }

    public void OnPickupButtonPressed()
    {
        Debug.Log("Pickup button pressed.");
        if (pickupItems == null || pickupItems.Count == 0)
        {
            Debug.Log("No PickupItems assigned in PickupButtons.");
            return;
        }

        // Check if there are items left to pick up
        if (currentItemIndex < pickupItems.Count)
        {
            var item = pickupItems[currentItemIndex];
            if (item != null)
            {
                item.OnPickupButtonPressed(); // Pick up the current item
                currentItemIndex++; // Move to the next item
            }
            else
            {
                Debug.Log("One of the PickupItems is null.");
            }
        }
        else
        {
            Debug.Log("All items have been picked up.");
            gameObject.SetActive(false); // Optionally deactivate the button when done
        }
    }

    // Method to set the PickupItems reference
    public void SetPickupItems(List<PickupItems> pickupItems)
    {
        this.pickupItems = pickupItems;
        currentItemIndex = 0; // Reset the index when setting new items
        foreach (var item in pickupItems)
        {
            Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? string.Join(", ", item.Items.ConvertAll(i => i.name)) : "null"));
        }
    }
} */
/* 
public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)

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

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");

        // Log the assigned PickupItems
        foreach (var item in pickupItems)
        {
            Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? string.Join(", ", item.Items.ConvertAll(i => i.name)) : "null"));
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

        // Iterate through the list of PickupItems and call OnPickupButtonPressed for each
        foreach (var item in pickupItems)
        {
            if (item != null)
            {
                item.OnPickupButtonPressed();
            }
            else
            {
                Debug.Log("One of the PickupItems is null.");
            }
        }
    }

    // Method to set the PickupItems reference
    public void SetPickupItems(List<PickupItems> pickupItems)
    {
        this.pickupItems = pickupItems;
        foreach (var item in pickupItems)
        {
            Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? string.Join(", ", item.Items.ConvertAll(i => i.name)) : "null"));
        }
    }
} */
/*
public class PickupButtons : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItem> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)

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

        // Add listener for button click
        button.onClick.AddListener(OnPickupButtonPressed);
        Debug.Log("Button listener added for pickup button.");

        // Log the assigned PickupItems
        foreach (var item in pickupItems)
        {
            Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? item.ItemName : "null"));
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

        // Iterate through the list of PickupItems and call OnPickupButtonPressed for each
        foreach (var item in pickupItems)
        {
            if (item != null)
            {
                item.OnPickupButtonPressed();
            }
            else
            {
                Debug.Log("One of the PickupItems is null.");
            }
        }
    }

    // Method to set the PickupItems reference
    public void SetPickupItems(List<PickupItem> pickupItems)
    {
        this.pickupItems = pickupItems;
        foreach (var item in pickupItems)
        {
            Debug.Log("PickupItem assigned in PickupButtons: " + (item != null ? item.ItemName : "null"));
        }
    }
} */