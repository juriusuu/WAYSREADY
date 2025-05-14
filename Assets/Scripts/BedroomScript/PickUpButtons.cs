using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupButtonss : MonoBehaviour
{
    public Button button; // Reference to the UI Button
    public List<PickupItems> pickupItems; // Public reference to a list of PickupItems (assign in Inspector)
    public GameObject inventoryPanel; // Reference to the Inventory Panel
    public GameObject inventorySlotPrefab; // Reference to the Inventory Slot Prefab
    public GameObject firstCompletionPanel; // First panel to show after all items are picked up
    public GameObject secondCompletionPanel; // Second panel to show after the first panel
    // public AudioSource completionAudio;  // Audio source to play when the panels are shown
    private int currentItemIndex = 0; // Track the current item index
    public AudioClip firstCompletionAudioClip; // Audio clip for the first completion panel
    public AudioClip secondCompletionAudioClip; // Audio clip for the second completion panel


    public List<PickupItems> timerDecreasingPickupItems; // List of items that decrease the timer
    public TaymerManager timerManager; // Reference to the TaymerManager to modify the timer

    private bool isPlayerNear = false; // Tracks if the player is near any pickup object

    private void Start()
    {
        // Ensure the button is hidden at the start
        if (button != null)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnPickupButtonPressed); // Add listener for button click
        }
        else
        {
            Debug.LogError("Button not found!");
        }

        // Ensure the completion panels are hidden at the start
        if (firstCompletionPanel != null)
        {
            firstCompletionPanel.SetActive(false);
        }
        if (secondCompletionPanel != null)
        {
            secondCompletionPanel.SetActive(false);
        }

        // Initialize the inventory panel
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true); // Show the panel initially
        }

        // Ensure the timer manager is assigned
        if (timerManager == null)
        {
            timerManager = FindObjectOfType<TaymerManager>();
            if (timerManager == null)
            {
                Debug.LogError("TaymerManager not found! Timer functionality will not work.");
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone of a pickup object
        if (other.CompareTag("PickUpItems"))
        {
            isPlayerNear = true;
            if (button != null)
            {
                button.gameObject.SetActive(true); // Show the button
            }
            Debug.Log("Player is near a pickup object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone of a pickup object
        if (other.CompareTag("PickUpItems"))
        {
            isPlayerNear = false;
            if (button != null)
            {
                button.gameObject.SetActive(false); // Hide the button
            }
            Debug.Log("Player left the pickup object.");
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

        /*   if (currentItemIndex >= pickupItems.Count)
          {
              Debug.Log("All PickupItems have been processed.");
              ShowFirstCompletionPanel(); // Show the first completion panel
              gameObject.SetActive(false); // Deactivate the button when done
          } */

        // Check if the inventory contains exactly 11 items
        if (InventoryHasRequiredItems())
        {
            Debug.Log("Inventory contains exactly 11 items. Showing completion panels.");
            ShowFirstCompletionPanel(); // Show the first completion panel
            gameObject.SetActive(false); // Deactivate the button when done
        }


        // Handle timer-decreasing items
        if (timerDecreasingPickupItems != null && timerDecreasingPickupItems.Count > 0)
        {
            foreach (var timerItem in timerDecreasingPickupItems)
            {
                if (timerItem != null && !timerItem.HasBeenPickedUp)
                {
                    Debug.Log($"Processing timer-decreasing item: {timerItem.gameObject.name}");
                    timerItem.OnPickupButtonPressed(); // Call the pickup method on the timer-decreasing item

                    if (timerManager != null)
                    {
                        timerManager.AddTime(-5f); // Decrease the timer by 5 seconds (adjust as needed)
                        Debug.Log($"Timer decreased by 5 seconds. Remaining time: {timerManager.remainingTime}");
                    }
                }
            }
        }
    }

    private bool InventoryHasRequiredItems()
    {
        // Assuming the inventory is managed by a separate InventoryManager
        var inventoryManager = FindObjectOfType<InventoryManagers>();
        if (inventoryManager != null)
        {
            int totalItems = inventoryManager.GetTotalItemCount(); // Replace with your method to get the total item count
            Debug.Log($"Total items in inventory: {totalItems}");
            return totalItems == 11; // Check if the inventory contains exactly 11 items
        }

        Debug.LogError("InventoryManager not found!");
        return false;
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


    private void ShowFirstCompletionPanel()
    {
        if (firstCompletionPanel != null)
        {
            firstCompletionPanel.SetActive(true); // Show the first completion panel
            Debug.Log("First completion panel displayed.");

            // Play the audio for the first completion panel
            if (firstCompletionAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(firstCompletionAudioClip, Camera.main.transform.position);
                Debug.Log("First completion audio played.");
            }

            // Hide the first panel and show the second panel after 2 seconds
            Invoke(nameof(ShowSecondCompletionPanel), 3f);
        }
    }


    private void ShowSecondCompletionPanel()
    {
        if (firstCompletionPanel != null)
        {
            firstCompletionPanel.SetActive(false); // Hide the first completion panel
            Debug.Log("First completion panel hidden.");
        }

        if (secondCompletionPanel != null)
        {
            secondCompletionPanel.SetActive(true); // Show the second completion panel
            Debug.Log("Second completion panel displayed.");

            // Play the audio for the second completion panel
            if (secondCompletionAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(secondCompletionAudioClip, Camera.main.transform.position);
                Debug.Log("Second completion audio played.");
            }

            // Hide the second panel after 2 seconds
            Invoke(nameof(HideSecondCompletionPanel), 5f);
        }
    }


    private void HideSecondCompletionPanel()
    {
        if (secondCompletionPanel != null)
        {
            secondCompletionPanel.SetActive(false); // Hide the second completion panel
            Debug.Log("Second completion panel hidden.");
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

/* using System.Collections.Generic;
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
} */