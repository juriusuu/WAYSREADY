using UnityEngine;
using UnityEngine.UI;

public class DropOff : MonoBehaviour
{
    public SandBagManager sandBagManager; // Reference to the SandBagManager
    public string sandbagType = "Default"; // The type of sandbag to drop

    void Start()
    {
        // Get the Button component and add a listener for the click event
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnDropButtonPressed);
    }

    public void OnDropButtonPressed()
    {
        if (sandBagManager != null)
        {
            sandBagManager.TryDropSandbag(sandbagType); // Call the method from SandBagManager
        }
        else
        {
            Debug.LogError("SandBagManager reference is not set in DropOffButton.");
        }
    }
}

/*
  public InventoryManager inventoryManager; // Reference to the InventoryManager
    public string sandbagType; // The type of sandbag to drop

    // This method will be called when the button is pressed
    public void OnDropButtonPressed()
    {
        Debug.Log("Drop button pressed.");
        if (inventoryManager != null)
        {
            Debug.Log($"Attempting to drop {sandbagType} sandbag.");
            inventoryManager.TryDropSandbag(sandbagType);
        }
        else
        {
            Debug.LogError("InventoryManager reference is not set in DropoffButton.");
        }
    }

*/