using UnityEngine;

public class InventoryButtonHandler : MonoBehaviour
{
    public void OnInventoryButtonClick()
    {
        if (InventoryUIManager.Instance != null)
        {
            InventoryUIManager.Instance.ToggleInventory(); // Call the ToggleInventory method
        }
        else
        {
            Debug.LogError("InventoryUIManager instance not found! Ensure it is initialized and marked as DontDestroyOnLoad.");
        }
    }
}