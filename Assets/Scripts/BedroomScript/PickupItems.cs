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
    public bool HasBeenPickedUp { get; private set; } = false;

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

        // Determine if this is a new game or a loaded game
        if (GameStateManager.Instance.IsNewGame)
        {
            // New game: Save the initial state
            SaveState(true);
        }
        else
        {
            // Loaded game: Load the saved state
            LoadState();
        }
    }

    public void OnPickupButtonPressed()
    {
        if (hasBeenPickedUp)
        {
            Debug.LogWarning($"{gameObject.name} has already been picked up.");
            return; // Exit if the items have already been picked up
        }

        Debug.Log($"Initiating pickup for items: {string.Join(", ", items.ConvertAll(item => item.name))}");

        if (InventoryManagers != null)
        {
            Debug.Log("InventoryManager is available.");
            if (IsPickupSuccessful())
            {
                Debug.Log("Pickup successful.");
                foreach (var item in items)
                {
                    string itemName = item.name;

                    // Retrieve the Sprite from the item's SpriteRenderer or Image component
                    Sprite itemSprite = null;
                    var spriteRenderer = item.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        itemSprite = spriteRenderer.sprite;
                    }
                    else
                    {
                        var image = item.GetComponent<UnityEngine.UI.Image>();
                        if (image != null)
                        {
                            itemSprite = image.sprite;
                        }
                    }

                    if (itemSprite != null)
                    {
                        InventoryManagers.AddItem(itemName, 1, itemSprite); // Add the item to the inventory
                        Debug.Log($"Added {itemName} to inventory with sprite {itemSprite.name}.");
                    }
                    else
                    {
                        Debug.LogWarning($"No sprite found for item {itemName}. Adding without sprite.");
                        InventoryManagers.AddItem(itemName, 1, null); // Add the item without a sprite
                    }
                }

                hasBeenPickedUp = true; // Mark as picked up
                SaveState(false); // Save the state as inactive
                gameObject.SetActive(false); // Deactivate the GameObject
                Debug.Log($"Deactivated pickup items for {gameObject.name}");
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

    public void SaveState(bool isActive)
    {
        if (GameStateManager.Instance != null)
        {
            var state = new ObjectState
            {
                isActive = isActive,
                position = transform.position,
                rotation = transform.rotation
            };

            GameStateManager.Instance.SaveObjectState(gameObject.name, state);
        }
    }

    public void LoadState()
    {
        if (GameStateManager.Instance != null)
        {
            var state = GameStateManager.Instance.LoadObjectState(gameObject.name);
            if (state != null)
            {
                // Restore the saved state
                gameObject.SetActive(state.isActive);
                transform.position = state.position;
                transform.rotation = state.rotation;
                hasBeenPickedUp = !state.isActive;
            }
            else
            {
                // No saved state exists, so save the initial state
                SaveState(true);
            }
        }
    }
}

//April 15
/* using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PickupItems : MonoBehaviour
{
    [SerializeField] private List<GameObject> items; // List of items to be picked up
    private bool hasBeenPickedUp = false; // Flag to prevent double pickup

    // Public property for InventoryManager
    public InventoryManagers InventoryManagers { get; set; } // Public property with a getter and setter

    // Getter for items
    public List<GameObject> Items => items;

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
        if (hasBeenPickedUp)
        {
            Debug.LogWarning($"{gameObject.name} has already been picked up.");
            return; // Exit if the item has already been picked up
        }

        hasBeenPickedUp = true; // Mark the item as picked up
        Debug.Log($"{gameObject.name} has been picked up.");

        if (InventoryManagers != null)
        {
            string itemName = gameObject.name;

            // Clean up the item name (e.g., remove "(Clone)" if present)
            if (itemName.Contains("(Clone)"))
            {
                itemName = itemName.Replace("(Clone)", "").Trim();
            }

            // Get the sprite from the Image component
            Image itemImage = GetComponent<Image>();
            if (itemImage != null && itemImage.sprite != null)
            {
                Debug.Log($"Sprite found for {gameObject.name}: {itemImage.sprite.name}");

                // Add the item to the inventory with its sprite
                InventoryManagers.Instance.AddItem(itemName, 1, itemImage.sprite);

                // Check and update the inventory UI
                if (InventoryUIManager.Instance == null)
                {
                    Debug.LogError("No InventoryUIManager found in the scene.");
                }
                else
                {
                    Debug.Log("Updating Inventory UI.");
                    InventoryUIManager.Instance.UpdateInventoryUI();
                }
            }
            else
            {
                Debug.LogWarning($"No sprite found for {gameObject.name}");
            }
        }
        else
        {
            Debug.LogError("InventoryManager is not set.");
        }

        // Destroy the GameObject after saving its data
        Destroy(gameObject);
        Debug.Log($"Destroyed pickup item: {gameObject.name}");
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
 */
//April 15

/*    Debug.Log($"OnPickupButtonPressed called for {gameObject.name} at {Time.time}");
   if (HasBeenPickedUp)
   {
       Debug.Log("Items have already been picked up.");
       return; // Exit if already picked up
   }

   Debug.Log("Initiating pickup for items: " + string.Join(", ", items.ConvertAll(item => item.name)));
*/
/*    Debug.Log($"OnPickupButtonPressed called for {gameObject.name} at {Time.time}");
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
   } *//* 
public void OnPickupButtonPressed()
{
Debug.Log($"OnPickupButtonPressed called for {gameObject.name} at {Time.time}");

if (HasBeenPickedUp)
{
    Debug.LogWarning($"{gameObject.name} has already been picked up.");
    return; // Exit if the item has already been picked up
}

HasBeenPickedUp = true; // Mark the item as picked up
Debug.Log($"{gameObject.name} has been picked up.");
*/
/*     // Add the item to the inventory
    if (InventoryManagers.Instance != null)
    {
        string itemName = gameObject.name;

        // Clean up the item name (e.g., remove "(Clone)" if present)
        if (itemName.Contains("(Clone)"))
        {
            itemName = itemName.Replace("(Clone)", "").Trim();
        }

        Sprite itemSprite = GetComponent<SpriteRenderer>()?.sprite; // Get the sprite from the GameObject
        InventoryManagers.Instance.AddItem(itemName, 1, itemSprite); // Add the item to the inventory
        Debug.Log($"Added {itemName} to inventory.");
    }
    else
    {
        Debug.LogError("InventoryManager is not set.");
    }
*/
/*   if (InventoryManagers.Instance != null)
  {
      string itemName = gameObject.name;

      // Clean up the item name (e.g., remove "(Clone)" if present)
      if (itemName.Contains("(Clone)"))
      {
          itemName = itemName.Replace("(Clone)", "").Trim();
      }

      // Get the sprite from the GameObject
      Sprite itemSprite = GetComponent<SpriteRenderer>()?.sprite;
      if (itemSprite != null)
      {
          Debug.Log($"Sprite found for {gameObject.name}: {itemSprite.name}");
          InventoryManagers.Instance.AddItem(itemName, 1, itemSprite); // Add the item to the inventory
      }
      else
      {
          Debug.LogWarning($"No sprite found for {gameObject.name}");
      }
  }
  else
  {
      Debug.LogError("InventoryManager is not set.");
  }
  // Destroy the GameObject after pickup
  Destroy(gameObject);
  Debug.Log($"Destroyed pickup items for {gameObject.name}");
} *//* 

  public void OnPickupButtonPressed()
  {
      if (hasBeenPickedUp)
      {
          Debug.LogWarning($"{gameObject.name} has already been picked up.");
          return; // Exit if the item has already been picked up
      }

      hasBeenPickedUp = true; // Mark the item as picked up
      Debug.Log($"{gameObject.name} has been picked up.");

      if (InventoryManagers != null)
      {
          string itemName = gameObject.name;

          // Clean up the item name (e.g., remove "(Clone)" if present)
          if (itemName.Contains("(Clone)"))
          {
              itemName = itemName.Replace("(Clone)", "").Trim();
          }

          // Get the sprite from the Image component
          Image itemImage = GetComponent<Image>();
          if (itemImage != null && itemImage.sprite != null)
          {
              Debug.Log($"Sprite found for {gameObject.name}: {itemImage.sprite.name}");

              // Add the item to the inventory with its sprite
              InventoryManagers.Instance.AddItem(itemName, 1, itemImage.sprite);
          }
          else
          {
              Debug.LogWarning($"No sprite found for {gameObject.name}");
          }
      }
      else
      {
          Debug.LogError("InventoryManager is not set.");
      }

      // Destroy the GameObject after saving its data
      Destroy(gameObject);
      Debug.Log($"Destroyed pickup item: {gameObject.name}");
  } */

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