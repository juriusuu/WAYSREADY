using UnityEngine;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel; // Reference to the Inventory Panel

    private void Start()
    {
        // Ensure the inventory panel is hidden at the start
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true); // Show the inventory panel
            InventoryManagers.Instance.DisplayInventory(); // Enable SpriteRenderers for inventory items
            Debug.Log("Inventory panel opened.");
        }
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Hide the inventory panel
            InventoryManagers.Instance.HideInventory(); // Disable SpriteRenderers for inventory items
            Debug.Log("Inventory panel closed.");
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);

            if (isActive)
            {
                InventoryManagers.Instance.HideInventory(); // Hide inventory items
                Debug.Log("Inventory panel closed.");
            }
            else
            {
                InventoryManagers.Instance.DisplayInventory(); // Show inventory items
                Debug.Log("Inventory panel opened.");
            }
        }
    }
}
/* 
public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;  */// Reference to the Inventory Panel
/*     public Button openInventoryButton; // Reference to the button that opens the inventory
    public Button closeInventoryButton; // Optional: Button to close the inventory

    private void Start()
    {
        // Ensure the inventory panel is hidden at the start
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        // Add a listener to the open inventory button
        if (openInventoryButton != null)
        {
            openInventoryButton.onClick.AddListener(OpenInventory);
        }

        // Add a listener to the close inventory button (if it exists)
        if (closeInventoryButton != null)
        {
            closeInventoryButton.onClick.AddListener(CloseInventory);
        }
    }

    public void OpenInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true); // Show the inventory panel
            Debug.Log("Inventory panel opened.");
        }
    }

    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Hide the inventory panel
            Debug.Log("Inventory panel closed.");
        }
    }
 */
/*     public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
            Debug.Log(isActive ? "Inventory panel closed." : "Inventory panel opened.");
        }
    }
} */