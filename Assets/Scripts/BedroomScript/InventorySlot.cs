using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // Reference to the Image component for the item's sprite
    public Text itemQuantityText; // Reference to the Text component for the item's quantity

    private string itemName; // The name of the item in this slot
    private int itemQuantity; // The quantity of the item in this slot

    // Setup the slot with item data
    public void Setup(string itemName, int itemQuantity, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemQuantity = itemQuantity;

        // Assign the sprite to the Image component
        if (itemImage != null)
        {
            if (itemSprite != null)
            {
                itemImage.sprite = itemSprite; // Assign the sprite to the Image
                itemImage.enabled = true; // Ensure the Image is enabled
                Debug.Log($"Setting up slot for {itemName} with sprite: {itemSprite.name}");
            }
            else
            {
                itemImage.sprite = null; // Clear the sprite
                itemImage.enabled = false; // Disable the Image if no sprite is provided
                Debug.LogWarning($"No sprite provided for {itemName}");
            }
        }
        else
        {
            Debug.LogError($"Image component is not assigned on {gameObject.name}. Cannot display sprite.");
        }

        // Update the quantity text
        if (itemQuantityText != null)
        {
            itemQuantityText.text = itemQuantity.ToString();
        }
        else
        {
            Debug.LogWarning($"Text component for quantity is not assigned on {gameObject.name}.");
        }
    }
    // Example: Handle slot interaction (e.g., clicking to use an item)
    public void OnSlotClicked()
    {
        Debug.Log($"Clicked on {itemName} with quantity {itemQuantity}");
        // Add logic for consuming or using the item
    }
}